

using Markdig;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using ZSN.AI.Core.Common.DependencyInjection;
using ChatHistory = Microsoft.SemanticKernel.ChatCompletion.ChatHistory;
using ZSN.AI.Core.Utils;
using ZSN.AI.Entity;
using ZSN.AI.Entity.Model.Constant;
using ZSN.AI.BLL;
using ZSN.AI.Core.Interface;
//using ZSN.AI.Core.Common.Bge;
using Newtonsoft.Json;
using ZSN.Utils.Core.Extensions;
using DocumentFormat.OpenXml.Wordprocessing;
using LLama.Common;
using Document = Microsoft.KernelMemory.Document;
using AuthorRole = Microsoft.SemanticKernel.ChatCompletion.AuthorRole;
using StackExchange.Redis;
using Microsoft.Identity.Client;
using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using System;

namespace ZSN.AI.Core.Service
{
    [ServiceDescription(typeof(IChatService), ServiceLifetime.Scoped)]
    public class ChatService(
        IKernelService _kernelService,
        IKMService _kMService
        ) : IChatService
    {

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="ModelConfig"></param>
        /// <param name="history"></param>
        /// <param name="Function"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<string> SendChatAsync(LargeModelConfig ModelConfig, ChatHistory history, CallFunction? Function = null)
        {
            var _kernel = _kernelService.GetKernel(ModelConfig.Model);
            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var temperature = ModelConfig.Temperature / 100;
            var topP = ModelConfig.TopPCoefficient / 100;

            OpenAIPromptExecutionSettings settings = new() { Temperature = temperature, TopP = topP };

            List<string> completionList = new List<string>();
            if (ModelConfig.SemanticFunction.Count>0 || ModelConfig.NativeFunction.Count>0 || Function!=null)
            {
                _kernelService.ImportFunctions(ModelConfig, _kernel);

                if (Function!=null)
                {
                    _kernelService.ImportFunctions( _kernel,Function.FunctionClass, Function.FunctionName);
                }

                //插件加载检查
                foreach (var plugin in _kernel.Plugins)
                {
                    Console.WriteLine("plugin: " + plugin.Name);
                    foreach (var function in plugin)
                    {
                        Console.WriteLine("  - prompt function: " + function.Name);
                    }
                }
                settings.ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions;// ToolCallBehavior.EnableKernelFunctions;
                while (true)
                {
                    ChatMessageContent result = await chat.GetChatMessageContentAsync(history, settings, _kernel);
                    if (!result.Content.IsNullOrEmpty())
                    {
                        string chunkCompletion = result.Content.ConvertToString();
                        completionList.Add(chunkCompletion);
                        foreach (var content in completionList)
                        {
                            yield return content.ConvertToString();
                        }
                        break;
                    }

                    history.Add(result);

                    IEnumerable<FunctionCallContent> functionCalls = FunctionCallContent.GetFunctionCalls(result);
                    if (!functionCalls.Any())
                    {
                        break;
                    }

                    foreach (FunctionCallContent functionCall in functionCalls)
                    {
                        try
                        {
                            FunctionResultContent resultContent = await functionCall.InvokeAsync(_kernel);

                            history.Add(resultContent.ToChatMessage());
                        }
                        catch (Exception ex)
                        {
                            history.Add(new FunctionResultContent(functionCall, ex).ToChatMessage());
                        }
                    }
                }
            }
            else
            {
                var chatResult = chat.GetStreamingChatMessageContentsAsync(history, settings, _kernel);
                await foreach (var content in chatResult)
                {
                    yield return content.ConvertToString();
                }
            }
        }

        private async Task<IEnumerable<string>> GenerateResponsesAsync(Kernel _kernel, KernelFunction func, KernelArguments arguments)
        {
            try
            {
                // 执行函数
                FunctionResult chatResult = await func.InvokeAsync(_kernel, arguments);

                return new[] { chatResult.ToString() };
            }
            catch (Exception ex)
            {
                // 异常时返回错误信息
                return new[] { $"生成回答时发生错误：{ex.Message}" };
            }
        }

        /// <summary>
        /// 通过多个知识库获取提问的相关内容，并根据对话记录重新组织返回
        /// </summary>
        /// <param name="KnowledgeBaseUnits"></param>
        /// <param name="ChatModel"></param>
        /// <param name="questions"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<string> SendKmsAsync(List<KnowledgeBaseUnit> KnowledgeBaseUnits, LargeModelConfig ChatModel, string questions, ChatHistory history)
        {
            var dataMsg = new StringBuilder();
            dataMsg.AppendLine("##通过知识库找到相关的内容：");
            foreach (var kms in KnowledgeBaseUnits)
            {
                var relevantSourceList = await _kMService.GetRelevantSourceList(kms.LargeModelUnit, questions, kms.KnowledgeBaseInfo.KnowledgeBaseID);
                
                if (relevantSourceList.Count > 0)
                {
                    dataMsg.AppendLine("#知识库名称：\""+ kms.KnowledgeBaseInfo.Name + "\"");
                    dataMsg.AppendLine("#知识库概述（概述可以快速理解这个知识库的作用以及只是范围）:\"" + kms.KnowledgeBaseInfo.Description + "\"");
                    dataMsg.AppendLine("#从知识库：\"" + kms.KnowledgeBaseInfo.Name + "\"中找到的相关内容如下：");
                    foreach (var item in relevantSourceList)
                    {
                        dataMsg.AppendLine(item.ToString());
                    }
                    dataMsg.AppendLine("");
                }
            }

            var _kernel = _kernelService.GetKernelByAIModelID(ChatModel.Model.LargeModelID);

            var temperature = ChatModel.Temperature / 100;
            var topP = ChatModel.TopPCoefficient / 100;

            OpenAIPromptExecutionSettings settings = new() { Temperature = temperature, TopP = topP };

            string prompt = @"
#你是一个专业的知识库助手，可以根据用户提问、对话历史和知识库内容，提供更贴近用户提问的精准回答。
#用户提问：
{{$input}}

#知识库检索内容：
{{$doc}}

#对话历史：
{{$history}}

";
            KernelFunction func = _kernel.CreateFunctionFromPrompt(
                prompt,
                settings
                );
            var recentHistory = history.TakeLast(5);
            var arguments = new KernelArguments()
            {
                ["doc"] = dataMsg.ToString(),
                ["history"] = string.Join("\n", recentHistory.Select(x => $"{x.Role}: {x.Content}")),
                ["input"] = questions
            };
            
            var responses = await GenerateResponsesAsync(_kernel,func, arguments);
            foreach (var response in responses)
            {
                yield return response;
            }
        }

        

        /*
        public async IAsyncEnumerable<StreamingKernelContent> SendKmsAsync(LargeModelUnit ModelUnit, string questions, ChatHistory history, string filePath, List<RelevantSource> relevantSources = null)
        {
            relevantSources?.Clear();
            var relevantSourceList = await _kMService.GetRelevantSourceList(ModelUnit, questions);
            var _kernel = _kernelService.GetKernel(ModelUnit.ChatModel.Model);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var memory = _kMService.GetMemory(ModelUnit);

                // 匹配GUID的正则表达式
                string pattern = @"\b[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}\b";

                // 使用正则表达式找到匹配
                Match match = Regex.Match(filePath, pattern);
                if (match.Success)
                {
                    var fileId = match.Value;

                    var status=await  memory.IsDocumentReadyAsync(fileId, index: KmsConstantcs.KmsIndex);
                    if (!status)
                    {
                        var result = await memory.ImportDocumentAsync(new Document(fileId).AddFile(filePath)
                                  .AddTag(KmsConstantcs.AppIdTag, ModelUnit.ChatModel.Id)
                                  .AddTag(KmsConstantcs.FileIdTag, fileId)
                                  , index: KmsConstantcs.FileIndex);
                    }

                    var filters = new List<MemoryFilter>() {
                        new MemoryFilter().ByTag(KmsConstantcs.AppIdTag, ModelUnit.ChatModel.Id),
                        new MemoryFilter().ByTag(KmsConstantcs.FileIdTag, fileId)
                    };

                    var searchResult = await memory.SearchAsync(questions, index: KmsConstantcs.FileIndex, filters: filters);
                    relevantSourceList.AddRange(searchResult.Results.SelectMany(item => item.Partitions.Select(part => new RelevantSource()
                    {
                        SourceName = item.SourceName,
                        Text = Markdown.ToHtml(part.Text),
                        Relevance = part.Relevance
                    })));
                    ModelUnit.ChatModel.Prompt = KmsConstantcs.KmsPrompt;
                }
            }


            var dataMsg = new StringBuilder();
            if (relevantSourceList.Count>0)
            {
                if (ModelUnit.RerankModel.Model.LargeModelID>0)
                {
                    var rerankModel= LargeModelInfoBussiness.GetModel(ModelUnit.RerankModel.Model.LargeModelID);
                    //BegRerankConfig.LoadModel(ModelUnit.RerankModel.Model.EndPoint, rerankModel.ModelName);
                    //进行rerank
                    foreach (var item in relevantSourceList)
                    {
                        List<string> rerank = new List<string>();
                        rerank.Add(questions);
                        rerank.Add(item.Text);
                        //item.RerankScore = BegRerankConfig.Rerank(rerank);
                      
                    }
                    relevantSourceList = relevantSourceList.OrderByDescending(p => p.RerankScore).Take(ModelUnit.RerankModel.MaxMatchesCount).ToList();
                }
                    
                bool isSearch = false;
                foreach (var item in relevantSourceList)
                {
                    if (ModelUnit.RerankModel.Model.LargeModelID > 0)
                    {
                        //匹配重排后相似度
                        if (item.RerankScore >= ModelUnit.RerankModel.Relevance / 100)
                        {
                            dataMsg.AppendLine(item.ToString());
                            isSearch = true;
                        }
                    }
                    else 
                    {
                        //匹配相似度
                        if (item.Relevance >= ModelUnit.RerankModel.Relevance / 100)
                        {
                            dataMsg.AppendLine(item.ToString());
                            isSearch = true;
                        }
                    }
                }

                
                //处理markdown显示
                relevantSources?.AddRange(relevantSourceList);
                Dictionary<string, string> fileDic = new Dictionary<string, string>();
                foreach (var item in relevantSourceList)
                {
                    if (fileDic.ContainsKey(item.SourceName))
                    {
                        item.SourceName = fileDic[item.SourceName];
                    }
                    else
                    {
                        var fileDetail = _kmsDetails_Repositories.GetFirst(p => p.FileGuidName == item.SourceName);
                        if (fileDetail.IsNotNull())
                        {
                            string fileName = fileDetail.FileName;
                            fileDic.Add(item.SourceName, fileName);
                            item.SourceName = fileName;
                        }       
                    }
                    item.Text = Markdown.ToHtml(item.Text);
                }
                

                if (isSearch)
                {
                    //KernelFunction jsonFun = _kernel.Plugins.GetFunction("KMSPlugin", "Ask1");
                    var temperature = ModelUnit.RerankModel.Temperature / 100;//存的是0~100需要缩小
                    OpenAIPromptExecutionSettings settings = new() { Temperature = temperature };
                    var func = _kernel.CreateFunctionFromPrompt(ModelUnit.RerankModel.Prompt??"" , settings);

                    var chatResult = _kernel.InvokeStreamingAsync(function: func,
                        arguments: new KernelArguments() { ["doc"] = dataMsg.ToString(), ["history"] = string.Join("\n", history.Select(x => x.Role + ": " + x.Content)), ["input"] = questions });

                    await foreach (var content in chatResult)
                    {
                        yield return content;
                    }
                }
                else
                {
                    yield return new StreamingTextContent(KmsConstantcs.KmsSearchNull);
                }
            }
            else
            {
                yield return new StreamingTextContent(KmsConstantcs.KmsSearchNull);
            }
        }
            */
        /// <summary>
        /// 组织会话记录
        /// </summary>
        /// <param name="MessageList"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public ChatHistory GetChatHistory(List<AppChatLogInfo> MessageList, ChatHistory history)
        {
            for(int i = 0;i<MessageList.Count;i++)
            {
                var item = MessageList[i];
                string _content = JsonConvert.DeserializeObject<GptMsg>(item.Content.ConvertToString()).content;

                switch (item.Direction) {

                    case 0:
                        history.AddUserMessage(_content);
                        break;
                    case 1:
                        history.AddAssistantMessage(_content);
                        break;
                    case 2:
                        history.AddMessage(AuthorRole.Tool,_content);
                        break;
                }
                
            }
            return history;
        }
        public ChatHistory GetChatHistory(List<AppChatSummaryInfo> MessageList, ChatHistory history)
        {
            for (int i = 0; i < MessageList.Count; i++)
            {
                var item = MessageList[i];
                string _content = JsonConvert.DeserializeObject<GptMsg>(item.Content.ConvertToString()).content;

                history.AddMessage(AuthorRole.Assistant, _content);

            }
            return history;
        }

        public async IAsyncEnumerable<string> HistorySummarize(LargeModelConfig ModelConfig, ChatHistory history)
        {
            var _kernel = _kernelService.GetKernel(ModelConfig.Model);
            
            yield return await _kernelService.HistorySummarize(_kernel, history);
        }

        public async IAsyncEnumerable<string> FunctionCall(LargeModelConfig ModelConfig, CallFunction callFunction, KernelArguments kernelArguments = null) {

            var _kernel = _kernelService.GetKernel(ModelConfig.Model);
            _kernel.Plugins.AddFromObject(callFunction.FunctionClass, callFunction.FunctionClassName);

            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var temperature = ModelConfig.Temperature / 100;
            var topP = ModelConfig.TopPCoefficient / 100;

            OpenAIPromptExecutionSettings settings = new() { Temperature = temperature, TopP = topP };
            List<string> completionList = new List<string>();
            settings.ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions;

            var history = new ChatHistory();
            history.AddSystemMessage(callFunction.Prompt);
            history.AddUserMessage(callFunction.Input);

            while (true)
            {
                ChatMessageContent result = await chat.GetChatMessageContentAsync(history, settings, _kernel);
                if (!result.Content.IsNullOrEmpty())
                {
                    string chunkCompletion = result.Content.ConvertToString();
                    completionList.Add(chunkCompletion);
                    foreach (var content in completionList)
                    {
                        yield return content.ConvertToString();
                    }
                    break;
                }

                history.Add(result);

                IEnumerable<FunctionCallContent> functionCalls = FunctionCallContent.GetFunctionCalls(result);
                if (!functionCalls.Any())
                {
                    break;
                }

                foreach (FunctionCallContent functionCall in functionCalls)
                {
                    try
                    {
                        FunctionResultContent resultContent = await functionCall.InvokeAsync(_kernel);

                        history.Add(resultContent.ToChatMessage());
                    }
                    catch (Exception ex)
                    {
                        history.Add(new FunctionResultContent(functionCall, ex).ToChatMessage());
                    }
                }
            }

        }
    
        public async IAsyncEnumerable<string> PromptFunctionCall(LargeModelConfig ModelConfig, CallFunction callFunction, KernelArguments kernelArguments = null)
        {
            var _kernel = _kernelService.GetKernel(ModelConfig.Model);

            var temperature = ModelConfig.Temperature / 100;
            var topP = ModelConfig.TopPCoefficient / 100;

            OpenAIPromptExecutionSettings settings = new() { Temperature = temperature, TopP = topP };
            List<string> completionList = new List<string>();

            string prompt = callFunction.Prompt;

            if (kernelArguments == null)
            {
                kernelArguments = new KernelArguments()
                {
                    ["input"] = callFunction.Input
                };
            }

            KernelFunction func = _kernel.CreateFunctionFromPrompt(
                prompt,
                settings
                );

            var responses = await GenerateResponsesAsync(_kernel, func, kernelArguments);
            foreach (var response in responses)
            {
                yield return response;
            }
        }
    }
}

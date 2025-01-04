
using ZSN.AI.Core.Interface;
using LLama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextGeneration;
using RestSharp;
using System;
using ServiceLifetime = ZSN.AI.Core.Common.DependencyInjection.ServiceLifetime;

using System.Reflection;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.KernelMemory;

using Microsoft.SemanticKernel.ChatCompletion;

using Microsoft.Extensions.Logging;
using ZSN.AI.Core.Common.DependencyInjection;
using ZSN.AI.Entity;
using ZSN.AI.Core.Utils;
using ZSN.AI.BLL;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.RegularExpressions;
using Microsoft.SemanticKernel.Plugins.Core;
using ZSN.AI.Plugins;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net.Plugin;

namespace ZSN.AI.Core.Service
{
    [ServiceDescription(typeof(IKernelService), ServiceLifetime.Scoped)]
    public class KernelService : IKernelService
    {
        private readonly FunctionService _functionService;
        private readonly IServiceProvider _serviceProvider;
        private Kernel _kernel;

        public KernelService(
              FunctionService functionService,
              IServiceProvider serviceProvider)
        {
            _functionService = functionService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取kernel实例，依赖注入不好按每个用户去Import不同的插件，所以每次new一个新的kernel
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public Kernel GetKernel(LargeModelInfo ModelInfo)
        {
            var chatHttpClient = OpenAIHttpClientHandlerUtil.GetHttpClient(ModelInfo.EndPoint);

            var builder = Kernel.CreateBuilder();
            WithTextGenerationByAIType(builder, ModelInfo, chatHttpClient);

            RegisterPluginsWithBase(builder);
            _kernel = builder.Build();
            return _kernel;

        }

        public Kernel GetKernelByAIModelID(int modelid)
        {
            LargeModelInfo ModelInfo = LargeModelInfoBussiness.GetModel(modelid);
            var chatHttpClient = OpenAIHttpClientHandlerUtil.GetHttpClient(ModelInfo.EndPoint);
            var builder = Kernel.CreateBuilder();
            WithTextGenerationByAIType(builder, ModelInfo, chatHttpClient);

            RegisterPluginsWithBase(builder);
            _kernel = builder.Build();
            return _kernel;
        }

        private void WithTextGenerationByAIType(IKernelBuilder builder, LargeModelInfo chatModel, HttpClient chatHttpClient)
        {
            
            switch (chatModel.ModelOrganizationID)
            {
                /*
                case (int)ZSN.AI.Entity.Model.Enum.AIType.OpenAI:
                    builder.AddOpenAIChatCompletion(
                       modelId: chatModel.ModelName,
                       apiKey: chatModel.ModelKey,
                       httpClient: chatHttpClient);
                    break;

                case (int)ZSN.AI.Entity.Model.Enum.AIType.AzureOpenAI:
                    builder.AddAzureOpenAIChatCompletion(
                        deploymentName: chatModel.ModelName,
                        apiKey: chatModel.ModelKey,
                        endpoint: chatModel.EndPoint
                        );
                    break;

                case (int)ZSN.AI.Entity.Model.Enum.AIType.LLamaFactory:
                    builder.AddOpenAIChatCompletion(
                     modelId: chatModel.ModelName,
                     apiKey: "NotNull",
                     httpClient: chatHttpClient
                       );
                    break;
                */
                case ZSN.AI.Entity.Model.Enum.AIType.Ollama:
                    
                    builder.AddOpenAIChatCompletion(
                       modelId: chatModel.ModelName,
                       apiKey: chatModel.ModelKey,
                       httpClient: chatHttpClient
                     );
                    break;
            }
        }

        /// <summary>
        /// 根据app配置的插件，导入插件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="_kernel"></param>
        public void ImportFunctions(LargeModelConfig ModelConfig, Kernel _kernel)
        {
            //插件不能重复注册，否则会异常
            if (_kernel.Plugins.Any(p => p.Name == "Functions"))
            {
                return;
            }
            List<KernelFunction> functions = new List<KernelFunction>();

            //API插件
            //ImportApiFunction(ModelInfo, functions);
            //本地函数插件
            ImportNativeFunction(ModelConfig, functions);

            _kernel.ImportPluginFromFunctions("Functions", functions);
        }
        public void ImportFunctions(Kernel _kernel, object type,string pluginName)
        {
            _kernel.Plugins.AddFromObject(type, pluginName);
        }
        /*
        /// <summary>
        /// 导入API插件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="functions"></param>
        private void ImportApiFunction(LargeModelInfo ModelInfo, List<KernelFunction> functions)
        {
            if (!string.IsNullOrWhiteSpace(ModelInfo.ApiFunctionList))
            {
                //开启自动插件调用
                var apiIdList = ModelInfo.ApiFunctionList.Split(",");
                var apiList = _apis_Repositories.GetList(p => apiIdList.Contains(p.Id));

                foreach (var api in apiList)
                {
                    var returnType = new KernelReturnParameterMetadata() { Description = api.OutputPrompt };
                    switch (api.Method)
                    {
                        case HttpMethodType.Get:

                            var getParametes = new List<KernelParameterMetadata>() {
                                     new KernelParameterMetadata("jsonbody"){
                                      ParameterType=typeof(string),
                                      Description=$"背景文档:{Environment.NewLine}{api.InputPrompt} {Environment.NewLine}提取出对应的json格式字符串，参考如下格式:{Environment.NewLine}{api.Query}"
                                    }
                                };
                            functions.Add(_kernel.CreateFunctionFromMethod((string jsonbody) =>
                            {
                                try
                                {
                                    //将json 转换为query参数
                                    var queryString = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonbody);
                                    RestClient client = new RestClient();
                                    RestRequest request = new RestRequest(api.Url, Method.Get);
                                    foreach (var header in api.Header.ConvertToString().Split("\n"))
                                    {
                                        var headerArray = header.Split(":");
                                        if (headerArray.Length == 2)
                                        {
                                            request.AddHeader(headerArray[0], headerArray[1]);
                                        }
                                    }
                                    //这里应该还要处理一次参数提取，等后面再迭代
                                    foreach (var q in queryString)
                                    {
                                        request.AddQueryParameter(q.Key, q.Value);
                                    }
                                    var result = client.Execute(request);
                                    return result.Content;
                                }
                                catch (System.Exception ex)
                                {
                                    return "调用失败：" + ex.Message;
                                }
                            }, api.Name, api.Describe, getParametes, returnType));
                            break;
                        case HttpMethodType.Post:
                            //处理json body
                            var postParametes = new List<KernelParameterMetadata>() {
                                    new KernelParameterMetadata("jsonbody"){
                                      ParameterType=typeof(string),
                                      Description=$"背景文档:{Environment.NewLine}{api.InputPrompt} {Environment.NewLine}提取出对应的json格式字符串，参考如下格式:{Environment.NewLine}{api.JsonBody}"
                                    }
                                };
                            functions.Add(_kernel.CreateFunctionFromMethod((string jsonBody) =>
                            {
                                try
                                {
                                    _logger.LogInformation(jsonBody);
                                    RestClient client = new RestClient();
                                    RestRequest request = new RestRequest(api.Url, Method.Post);
                                    foreach (var header in api.Header.ConvertToString().Split("\n"))
                                    {
                                        var headerArray = header.Split(":");
                                        if (headerArray.Length == 2)
                                        {
                                            request.AddHeader(headerArray[0], headerArray[1]);
                                        }
                                    }
                                    //这里应该还要处理一次参数提取，等后面再迭代
                                    request.AddJsonBody(jsonBody.ConvertToString());
                                    var result = client.Execute(request);
                                    return result.Content;
                                }
                                catch (System.Exception ex)
                                {
                                    return "调用失败：" + ex.Message;
                                }
                            }, api.Name, api.Describe, postParametes, returnType));
                            break;
                    }
                }
            }

        }
        */

        /// <summary>
        /// 导入原生插件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="functions"></param>
        private void ImportNativeFunction(LargeModelConfig ModelConfig, List<KernelFunction> functions)
        {
            if (ModelConfig.NativeFunction!=null)//需要添加判断应用是否开启了本地函数插件
            {
                //var nativeIdList = ModelConfig.NativeFunctionID.Split(",");

                _functionService.SearchMarkedMethods();
                using var scope = _serviceProvider.CreateScope();
                string pattern = "[^a-zA-Z0-9_]";

                foreach (var func in _functionService.Functions)
                {
                    if (ModelConfig.NativeFunction.Find(f => func.Key.Contains(Regex.Replace(f.Namespace + "_" + f.ClassName, pattern, "_"))) != null)
                    {
                        var methodInfo = _functionService.MethodInfos[func.Key];
                        var parameters = methodInfo.Parameters.Select(x => new KernelParameterMetadata(x.ParameterName) { ParameterType = x.ParameterType, Description = x.Description });
                        var returnType = new KernelReturnParameterMetadata() { ParameterType = methodInfo.ReturnType.ParameterType, Description = methodInfo.ReturnType.Description };
                        var target = ActivatorUtilities.CreateInstance(scope.ServiceProvider, func.Value.DeclaringType);
                        functions.Add(_kernel.CreateFunctionFromMethod(func.Value, target, func.Key, methodInfo.Description, parameters, returnType));
                    }
                }
            }
        }

        /// <summary>
        /// 注册默认插件
        /// </summary>
        /// <param name="kernel"></param>
        private void RegisterPluginsWithBase(IKernelBuilder kernel)
        {
#pragma warning disable SKEXP0050 // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。
            //kernel.Plugins.AddFromObject(new BasePlugin(), "BasePlugin");
            kernel.Plugins.AddFromObject(new ConversationSummaryPlugin(), "ConversationSummaryPlugin");
            //kernel.Plugins.AddFromType<Microsoft.SemanticKernel.Plugins.Core.TimePlugin>();
            //kernel.Plugins.AddFromType<Microsoft.SemanticKernel.Plugins.Core.HttpPlugin>();
            //kernel.Plugins.AddFromType<Microsoft.SemanticKernel.Plugins.Core.MathPlugin>();
            //kernel.Plugins.AddFromType<Microsoft.SemanticKernel.Plugins.Core.FileIOPlugin>();
#pragma warning restore SKEXP0050 // 类型仅用于评估，在将来的更新中可能会被更改或删除。取消此诊断以继续。
            //kernel.Plugins.AddFromPromptDirectory(System.IO.Path.Combine(RepoFiles.SamplePluginsPath(), "SemanticFunction"));
        }

        /// <summary>
        /// 会话总结
        /// </summary>
        /// <param name="_kernel"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task<string> HistorySummarize(Kernel _kernel, ChatHistory history)
        {
            KernelFunction sunFun = _kernel.Plugins.GetFunction("ConversationSummaryPlugin", "SummarizeConversation");
            var summary = await _kernel.InvokeAsync(sunFun, new() { ["input"] = $"内容是：{string.Join("\n", history.Select(x => x.Role + ": " + x.Content))} {Environment.NewLine}请注意，找出讨论的要点和得出的任何结论，不要加入其他常识，摘要是纯文本形式，用中文总结" });
            string his = summary.GetValue<string>();
            return his;
        }

        public async Task<string> PromptFunctionCall(Kernel _kernel, CallFunction callFunction, KernelArguments parameter)
        {
            KernelFunction sunFun = _kernel.Plugins.GetFunction(callFunction.FunctionClassName, callFunction.FunctionName);

            var call = await _kernel.InvokeAsync(sunFun, parameter);
            return call.GetValue<string>();
        }
    }
}

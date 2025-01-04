using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Google.Protobuf.WellKnownTypes;
using Lucene.Net.Util.Fst;
using Microsoft.Identity.Client;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System.Reflection;
using System.Text;

using ZSN.AI.Core.Interface;
using ZSN.AI.Core.Repositories;
using ZSN.AI.Core.Utils;
using ZSN.AI.Plugins;
using ZSN.Utils.Core.Extensions;
using ZSN.AI.BLL;
using ZSN.AI.Entity;

namespace ZSN.AI.Node
{
    public class Excution
    {
        private readonly IChatService _chatService;
        public Excution(IChatService chatService)
        {
            _chatService = chatService;
        }

        private string ReplacePromptValue(string prompt, List<Inputs> inputs)
        {

            foreach (Inputs input in inputs)
            {
                string _varname = input.sourceId.IsNullOrEmpty() ? input.varname : input.sourceId;
                prompt = prompt.Replace("{{" + _varname + "}}", input.value);
            }
            return prompt;
        }

        public string StartNode(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID=data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    StartData nodeData = JsonConvert.DeserializeObject<StartData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        nodeData.prompt = this.ReplacePromptValue(nodeData.prompt, inputs);
                        
                        List<Output> outputs = new List<Output>();
                        outputs.Add(new Output { varname = "prompt", value = nodeData.prompt });
                        outputs.Add(new Output { varname = "currentTime", value = DateTime.Now.ToDateTimeString() });

                        Outputs.Add(JsonConvert.SerializeObject(outputs));
                        Logs.Add(nodeData.prompt);


                        WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, config, outputs,  Logs);

                    }
                }
                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }
        public string AgentStartNode(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID = data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    AgentStartData nodeData = JsonConvert.DeserializeObject<AgentStartData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        nodeData.prompt = this.ReplacePromptValue(nodeData.prompt, inputs);
                        Outputs.Add(nodeData.prompt);
                        Logs.Add(nodeData.prompt);

                        List<Output> outputs = new List<Output>();
                        outputs.Add(new Output { varname = "prompt", value = nodeData.prompt });
                        outputs.Add(new Output { varname = "currentTime", value = DateTime.Now.ToDateTimeString() });

                        WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, config, outputs, Logs);

                    }
                }
                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }
        public async Task<string> EndNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID = data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    EndData nodeData = JsonConvert.DeserializeObject<EndData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        nodeData.prompt = this.ReplacePromptValue(nodeData.prompt, inputs);
                        Outputs.Add(nodeData.prompt);
                        Logs.Add(nodeData.prompt);

                        GptMsg _msg = new GptMsg();
                        _msg.role = AuthorRole.Assistant.ToString();
                        _msg.content = nodeData.prompt;

                        AppChatLogInfoBussiness.Add(AppID, SessionID, AuthorRole.User.ToString(),  _msg);

                        TaskInfoBussiness.updateTask(TaskID, TaskState.Completed, new Results()
                        {
                            Data = nodeData.prompt
                        });
                    }
                }

                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }

        public async Task<string> AgentEndNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID = data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    AgentEndData nodeData = JsonConvert.DeserializeObject<AgentEndData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        nodeData.prompt = this.ReplacePromptValue(nodeData.prompt, inputs);
                        Outputs.Add(nodeData.prompt);
                        Logs.Add(nodeData.prompt);

                        excutionRecordStatus = ExcutionRecordStatus.Success;

                        TaskInfoBussiness.updateTask(TaskID, TaskState.Completed, new Results()
                        {
                            Data = nodeData.prompt
                        });
                    }
                }
                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            try
            {
                Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
                return RecordID;
            }
            finally
            {
                //处理子的任务回溯，WorkFlow->Agent->MainAI
                //检查一次与当前节点相同ProcessesID的任务是否已完成，如果完成将结果整理反馈给MainAI

                List<TaskInfo> taskInfos = TaskInfoBussiness.GetListByFromMainTaskID(FromMainTaskID);

                List<TaskInfo> agentStart_Tasks = taskInfos.FindAll(t => t.TaskType == NodeType.AgentStart);
                List<TaskInfo> agentEnd_Tasks = taskInfos.FindAll(t => t.TaskType == NodeType.AgentEnd);

                //开始和结束数量相等，继续
                if(agentStart_Tasks.Count == agentEnd_Tasks.Count)
                {
                    bool AgentWorkflowCompleted = true;
                    int CompletedTotal = 0;
                    var dataMsg = new StringBuilder();

                    foreach (TaskInfo task in agentEnd_Tasks)
                    {
                        if (task.State == TaskState.Waiting || task.State == TaskState.Processing)
                        {
                            AgentWorkflowCompleted = false;
                        }
                        if (task.State == TaskState.Completed)
                        {
                            dataMsg.AppendLine("---start---");
                            dataMsg.AppendLine(task.Results.Data.ToString());
                            dataMsg.AppendLine("---end---");
                            dataMsg.AppendLine("");
                        }
                    }
                    //已经全部完成，把结果反馈给上层节点（fromTask）的下一节点
                    if (AgentWorkflowCompleted)
                    {
                        //处理子工作流返回的结果
                        
                        WorkflowNodeInfo nodeInfo = WorkflowNodeInfoBussiness.GetModel(AgentNodeID);
                        if (nodeInfo != null)
                        {

                            NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(nodeInfo.Config.ToString());
                            AgentData agentData = JsonConvert.DeserializeObject<AgentData>(nodeConfig.data.ToString());

                            nodeConfig.fromNodeType = NodeType.AgentEnd;
                            string agentName = agentData.label;

                            List<Output> outputs = new List<Output>();
                            outputs.Add(new Output { varname = "results", value = dataMsg.ToString() });
                            outputs.Add(new Output { varname = "currentTime", value = DateTime.Now.ToDateTimeString() });
                            outputs.Add(new Output { varname = "agentName", value = agentName });


                            WorkflowNodeInfoBussiness.AgentEndToNextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, nodeConfig, outputs, Logs);
                        }
                        
                    }
                }
            }
        }
        public async Task<string> LargeModelNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID=data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    LargeModelData nodeData = JsonConvert.DeserializeObject<LargeModelData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        nodeData.prompt = this.ReplacePromptValue(nodeData.prompt, inputs);
                        string results = "";

                        if (nodeData.model?.LargeModelID > 0)
                        {
                            ChatHistory history = new ChatHistory();
                            LargeModelInfo largeModel = LargeModelInfoBussiness.GetModel(nodeData.model.LargeModelID);

                            //设置系统提示词
                            if (!nodeData.prompt.IsNullOrEmpty())
                            {
                                history.AddSystemMessage(nodeData.prompt);
                            }
                            LargeModelConfig modelConfig = new LargeModelConfig();
                            modelConfig.Id = largeModel.LargeModelID.ToString();
                            modelConfig.Model = largeModel;
                            modelConfig.SemanticFunction = nodeData.SemanticFunction;
                            modelConfig.NativeFunction = nodeData.NativeFunction;
                            modelConfig.Temperature = nodeData.temperature;
                            modelConfig.TopPCoefficient = nodeData.topp;


                            var chatResult = _chatService.SendChatAsync(modelConfig, history);
                            StringBuilder rawContent = new StringBuilder();
                            Chats info = null;
                            List<Chats> MessageList = [];

                            await foreach (var content in chatResult)
                            {
                                if (info == null)
                                {
                                    rawContent.Append(content.ConvertToString());
                                    info = new Chats();
                                    info.Id = Guid.NewGuid().ToString();
                                    info.UserName = AuthorRole.Assistant.ToString();
                                    info.AppId = AppID;
                                    info.Context = content.ConvertToString();
                                    info.CreateTime = DateTime.Now;

                                    MessageList.Add(info);
                                }
                                else
                                {
                                    rawContent.Append(content.ConvertToString());
                                }
                                info.Context = rawContent.ToString();
                            }
                            results = info.Context;
                        }

                        Outputs.Add(results);
                        Logs.Add(results);

                        List<Output> outputs = new List<Output>();
                        outputs.Add(new Output { varname = "results", value = results });

                        WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, config, outputs, Logs);

                    }
                }
                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }

        public async Task<string> MainAINodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID=data.TaskID, SessionID = data.SessionID, ProcessesID = data.ProcessesID.IsNullOrEmpty() ? Guid.NewGuid().ToString() : data.ProcessesID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);
            int ChatCount = 0;

            List<ChatMessageContent> _Message = new List<ChatMessageContent>();
            AppChatSessionInfo appChatSession = new AppChatSessionInfo();

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    MainAIData nodeData = JsonConvert.DeserializeObject<MainAIData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        string results = "";
                        nodeData.prompt = this.ReplacePromptValue(nodeData.prompt, inputs);


                        if (nodeData.model?.LargeModelID > 0)
                        {
                            ChatHistory history = new ChatHistory();
                            LargeModelInfo largeModel = LargeModelInfoBussiness.GetModel(nodeData.model.LargeModelID);

                            //设置系统提示词
                            if (!nodeData.prompt.IsNullOrEmpty())
                            {
                                history.AddSystemMessage(nodeData.prompt);
                            }
                            LargeModelConfig modelConfig = new LargeModelConfig();
                            modelConfig.Id = largeModel.LargeModelID.ToString();
                            modelConfig.Model = largeModel;
                            modelConfig.SemanticFunction = nodeData.SemanticFunction;
                            modelConfig.NativeFunction = nodeData.NativeFunction;
                            modelConfig.Temperature = nodeData.temperature;
                            modelConfig.TopPCoefficient = nodeData.topp;

                            appChatSession = AppChatSessionInfoBussiness.GetModel(SessionID);
                            List<AppChatLogInfo> appChatLogs = AppChatLogInfoBussiness.GetListBySessionID(AppID, SessionID);
                            List<AppChatSummaryInfo> appChatSummaries = AppChatSummaryInfoBussiness.GetListBySessionID(AppID, SessionID);
                            var dataMsg = new StringBuilder();
                            CallFunction callFunction = new CallFunction();

                            ChatCount = appChatLogs.Count;
                            //过滤已被总结的记录
                            if (appChatLogs is not null && appChatLogs.Count > 0 && appChatSummaries is not null && appChatSummaries.Count > 0)
                            {
                                // 提取 appChatSummaries 中的所有 ChatLogIDList 并转为 HashSet<string>
                                var summaryIds = appChatSummaries
                                    .SelectMany(summary => summary.ChatLogIDList.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                    .ToHashSet(StringComparer.OrdinalIgnoreCase); // 不区分大小写

                                // 过滤 appChatLogs 中的记录
                                appChatLogs = appChatLogs.Where(log => !summaryIds.Contains("\"" + log.ChatLogID + "\"")).ToList();

                            }

                            history = _chatService.GetChatHistory(appChatLogs, history);
                            history = _chatService.GetChatHistory(appChatSummaries, history);

                            //是否存在用户输入
                            Inputs userInput = inputs.FirstOrDefault(input => input.varname == "prompt");
                            if (userInput != null)
                            {
                                history.AddUserMessage(userInput.value);
                            }

                            //是否自行Agent节点
                            bool excution_agent = true;
                            bool is_Agent_Return = config.fromNodeType == NodeType.Agent;

                            //先对用户提问进行预处理
                            if (!is_Agent_Return && userInput != null)
                            {
                                dataMsg.Clear();

                                dataMsg.AppendLine("#注意：根据用户的提问，如果是问候、寒暄之类非专业领域或者特定需要说明解释的内容，可以直接回答，回答内容简洁，使用礼貌用语，其他内容一律只回答:\"no answer\"");
                                dataMsg.AppendLine("#用户的提问:");
                                dataMsg.AppendLine("{{$input}}");

                                callFunction = new CallFunction();
                                callFunction.Prompt = dataMsg.ToString();
                                callFunction.Input = userInput.value;

                                modelConfig.Temperature = 0;//不允许大模型自由发挥

                                var functionCallRe = _chatService.PromptFunctionCall(modelConfig, callFunction);
                                string response_str = "";
                                await foreach (var response in functionCallRe)
                                {
                                    response_str += response;
                                }
                                if (response_str.IndexOf("no answer") > -1)
                                {
                                    excution_agent = true;
                                }
                                else
                                {
                                    results = response_str;
                                    excution_agent = false;
                                }

                            }


                            //如果上一节点是Agent则对其反馈的结果进行梳理，判断是否满足要求，满足要求则返回梳理后的结果，不满足重新调用Agent，并提出进一步要求
                            if (is_Agent_Return)
                            {
                                var agentName = inputs.FirstOrDefault(a => a.varname == "agentName");
                                var agentResults = inputs.FirstOrDefault(a => a.varname == "results");
                                excution_agent = false;
                                string input_str = userInput.value;
                                dataMsg.Clear();
                                dataMsg.AppendLine($"##经过Agent({agentName.varname})的处理返回了如下结果，你可以判断返回的结果是否满足提出的问题，如果满足，可以根据结果再根据上下文组织优化后返回结果，如果无法满足，只需返回\"$$$无法满足$$$\"。");
                                
                                dataMsg.AppendLine($"#Agent({agentName.varname})处理的结果:");
                                dataMsg.AppendLine($"{agentResults.value}");

                                dataMsg.AppendLine("#提出的问题:");
                                dataMsg.AppendLine("{{$input}}");
                                dataMsg.AppendLine("");

                                dataMsg.AppendLine("##你可以判断返回的结果是否满足提出的问题，如果满足，可以根据结果再根据上下文将Agent处理的结果重新组织优化后返回结果，如果无法满足，只需返回\"$$$无法满足$$$\"。");

                                callFunction = new CallFunction();
                                callFunction.Prompt = dataMsg.ToString(); //string.Join("\n", history.Select(x => x.Role + ": " + x.Content))+"\n"+ dataMsg.ToString();
                                callFunction.Input = input_str;

                                var functionCallRe =  _chatService.PromptFunctionCall(modelConfig, callFunction);
                                string response_str = "";
                                await foreach(var response in functionCallRe)
                                {
                                    response_str += response;
                                }

                                if (response_str.IndexOf("$$$无法满足$$$") >-1) {

                                    excution_agent = true;
                                }
                                else
                                {
                                    results = response_str;
                                    excution_agent = false;
                                    history.AddAssistantMessage(response_str);
                                }
                            }
                            
                            if (excution_agent)
                            {
                                //下一节点如果有Agent工作流，则由大模型自行判断执行其中的Agent工作流
                                KernelFunction function = null;
                                List<WorkflowEdgeInfo> edgeList = WorkflowEdgeInfoBussiness.GetListBySourceNodeId(config.id);
                                if (edgeList != null && edgeList.Count > 0)
                                {
                                    List<WorkflowNodeInfo> targetNodeList = WorkflowNodeInfoBussiness.GetListByNodeID(string.Join(",", edgeList.Select(x => $"'{x.TargetNodeId}'")));
                                    if (targetNodeList != null)
                                    {
                                        bool existAgentNode = false;
                                        dataMsg.Clear();
                                        dataMsg.AppendLine("#注意：当你无法准确的回答时，不能随意回答，需要调用特定的Agent进行处理。执行Agent，并传入相应的参数。");
                                        dataMsg.AppendLine("#选择调用哪个Agent可以通过Agent的参数列表中的Agent的Name和Description来进行判断，你觉得有需要某个或者多个Agent来帮你回答，就使用调用函数调用即可。");

                                        dataMsg.AppendLine("##本次任务的共用标识参数 开始##");
                                        dataMsg.AppendLine($"AppID:{AppID}");
                                        dataMsg.AppendLine($"TaskID:{TaskID}");
                                        dataMsg.AppendLine($"FromMainTaskID:{TaskID}");
                                        dataMsg.AppendLine($"SessionID:{SessionID}");
                                        dataMsg.AppendLine($"Inputs:{userInput.value}");
                                        dataMsg.AppendLine("##本次任务的共用标识参数 结束##");

                                        dataMsg.AppendLine("##Agent的参数列表 开始##");

                                        foreach (WorkflowNodeInfo node in targetNodeList)
                                        {
                                            if (node != null && node.NodeType == NodeType.Agent)
                                            {
                                                existAgentNode = true;
                                                NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(node.Config.ToString());
                                                if (nodeConfig != null)
                                                {
                                                    AgentData agentData = JsonConvert.DeserializeObject<AgentData>(nodeConfig.data.ToString());
                                                    if (agentData != null)
                                                    {
                                                        dataMsg.AppendLine($"#{agentData.label}:");
                                                        dataMsg.AppendLine($"AgentName:{agentData.label}");
                                                        dataMsg.AppendLine($"Description:{agentData.agent.Description}");
                                                        dataMsg.AppendLine($"ProcessesID:{Guid.NewGuid().ToString()}");//每一个Agent分配一个ProcessesID
                                                        dataMsg.AppendLine($"AgentNodeID:{node.NodeID}");
                                                        dataMsg.AppendLine("");
                                                    }
                                                }
                                            }
                                        }

                                        dataMsg.AppendLine("##Agent的参数列表 结束##");
                                        dataMsg.AppendLine("");

                                        dataMsg.AppendLine("#Agent的参数列表说明：每个Agent都有一个\"AgentName\"是Agent的名称，\"Description\"是Agent的具体功能作用的描述，\"ProcessesID\"是将要下发给Agent任务时需要传入的工作编号，\"AgentNodeID\"是Agent的节点编号");
                                        dataMsg.AppendLine("#请记住：");
                                        dataMsg.AppendLine("1.使用函数时要准确传递参数");
                                        dataMsg.AppendLine("2.只在需要实时数据时才调用函数");
                                        dataMsg.AppendLine("");

                                        dataMsg.AppendLine("#注意:如果执行了Agent，请直接返回执行函数后返回的\"NewTaskID\"值，返回格式{\"NewTaskID\":\"执行函数后返回的NewTaskID值\"}，如果调用多个Agent则将\"NewTaskID\"用逗号隔开拼接在一起，返回例子：{\"NewTaskID\":\"NewTaskID1\",\"NewTaskID2\"}。");

                                        if (existAgentNode)
                                        {
                                            history.AddSystemMessage(dataMsg.ToString());
                                        }

                                    }
                                }
                                
                                callFunction.FunctionClass = new BasePlugin();
                                callFunction.FunctionName = "excution_agent";
                            }
                            else
                            {
                                callFunction = null;
                            }


                            if (results.IsNullOrEmpty())
                            {
                                var chatResult = _chatService.SendChatAsync(modelConfig, history, callFunction);
                                StringBuilder rawContent = new StringBuilder();
                                Chats info = null;
                                List<Chats> MessageList = [];

                                await foreach (var content in chatResult)
                                {
                                    if (info == null)
                                    {
                                        rawContent.Append(content.ConvertToString());
                                        info = new Chats();
                                        info.Id = Guid.NewGuid().ToString();
                                        info.UserName = AuthorRole.Assistant.ToString();
                                        info.AppId = AppID;
                                        info.Context = content.ConvertToString();
                                        info.CreateTime = DateTime.Now;

                                        MessageList.Add(info);
                                    }
                                    else
                                    {
                                        rawContent.Append(content.ConvertToString());
                                    }
                                    info.Context = rawContent.ToString();
                                }
                                results = info.Context;
                            }
                        }

                        Outputs.Add(results);
                        Logs.Add(results);

                        //当主AI返回正常结果则进入下一节点，否则丢弃
                        if (!(results.IndexOf("{\"NewTaskID\":") >-1))
                        {
                            List<Output> outputs = new List<Output>();
                            outputs.Add(new Output { varname = "results", value = results });
                            outputs.Add(new Output { varname = "complete_type", value = "app" });

                            WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID: "", config, outputs, Logs);
                        }
                        
                    }
                }

                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }

        public async Task<string> PluginsNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID=data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    PluginsData nodeData = JsonConvert.DeserializeObject<PluginsData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        string results = "";

                        if (nodeData.plugins != null)
                        {
                            string namespaceName = nodeData.plugins.Namespace;
                            string className = nodeData.plugins.ClassName;
                            string methodName = nodeData.plugins.EntryFunctionName;

                            var parameters = new List<object>();
                            foreach (var input in inputs)
                            {
                                parameters.Add(Convert.ChangeType(input.value, System.Type.GetType(input.type)));
                            }

                            System.Type type = System.Type.GetType(namespaceName + "." + className);
                            if (type != null) {
                                MethodInfo methodInfo = type.GetMethod(methodName);
                                if (methodInfo != null) {
                                    object result = methodInfo.Invoke(null, new object[] { JsonConvert.SerializeObject(parameters) });

                                    results = JsonConvert.SerializeObject(result);
                                }
                            }
                        }

                        List<Output> outputs = new List<Output>();
                        outputs.Add(new Output { varname = "results", value = results });

                        WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, config, outputs, Logs);
                    }
                }

                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }

        public async Task<string> SelectorNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID = data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    SelectorData nodeData = JsonConvert.DeserializeObject<SelectorData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        string results = "";

                        //获取所有下节点
                        List<WorkflowEdgeInfo> edgeList = WorkflowEdgeInfoBussiness.GetListBySourceNodeId(config.id);
                        if (edgeList != null && edgeList.Count > 0)
                        {
                            List<string> TargetNodeId = new List<string>();
                            string selectorId = "";

                            var comparisonActions = new Dictionary<string, Func<string, string, bool>>()
                                {
                                    { "=", (inputValue, selectorValue) => inputValue == selectorValue },
                                    { "!=", (inputValue, selectorValue) => inputValue != selectorValue },
                                    { ">", (inputValue, selectorValue) => decimal.Parse(inputValue) > decimal.Parse(selectorValue) },
                                    { "<", (inputValue, selectorValue) => decimal.Parse(inputValue) < decimal.Parse(selectorValue) },
                                    { "~=", (inputValue, selectorValue) => inputValue.Contains(selectorValue) },
                                    { "!~=", (inputValue, selectorValue) => !inputValue.Contains(selectorValue) }
                                };

                            //条件判断
                            foreach (var selector in nodeData.selector)
                            {
                                var input = inputs.FirstOrDefault(i => i.varname == selector.varname);
                                if (input == null) continue;

                                var comparison = selector.comparison;
                                var inputValue = input.value;
                                var selectorValue = selector.value;

                                if (comparisonActions.TryGetValue(comparison, out var action) && action(inputValue, selectorValue))
                                {
                                    selectorId = selector.id;
                                    break;
                                }
                            }
                            if (!selectorId.IsNullOrEmpty())
                            {
                                //获取TargetNodeId
                                foreach (var edge in edgeList)
                                {
                                    var _config = JObject.Parse(JsonConvert.SerializeObject(edge.Config));

                                    if (selectorId == (string)_config["sourceHandle"])
                                    {
                                        TargetNodeId.Add(edge.TargetNodeId);
                                    }
                                }

                                List<WorkflowNodeInfo> targetNodeList = WorkflowNodeInfoBussiness.GetListByNodeID(string.Join(",", TargetNodeId.Select(id => $"'{id}'")));
                                if (targetNodeList != null)
                                {
                                    //把选择器的输入参数转为输出参数
                                    List<Output> outputs = inputs.Select(i => new Output { varname = i.varname, value = i.value, type = i.type, txt = i.txt }).ToList();

                                    foreach (var node in targetNodeList)
                                    {
                                        NodeConfig targetNode = new NodeConfig() { id = node.NodeID, mainid = config.mainid, workflowid = node.WorkflowID, type = node.NodeType, data = node.Config };

                                        string newTaskID = TaskInfoBussiness.toTask(config, outputs, targetNode, AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID);

                                        Logs.Add($"{newTaskID}");
                                    }
                                }
                            }
                        }
                    }
                }

                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }

        public async Task<string> AgentNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID=data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    AgentData nodeData = JsonConvert.DeserializeObject<AgentData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        string results = "";

                        AgentNodeID = AgentNodeID.IsNullOrEmpty() ? config.id: "";
                        string AgentID = nodeData.agent.AgentID;

                        //判断是否为AgentEndTask，即Agent子任务结束的返回
                        if (config.fromNodeType == NodeType.AgentEnd)
                        {
                            //获取任务开始MainAI的prompt
                            TaskInfo mainAITask = TaskInfoBussiness.GetModel(FromMainTaskID);
                            List<Inputs> mainAIInput = mainAITask.TaskConfig.Data.Inputs;

                            inputs = inputs.Concat(mainAIInput).ToList();

                            List<Output> outputs = inputs.Select(input => new Output
                            {
                                
                                varname = input.varname, 
                                value = input.value,
                                type = input.type,
                                txt = input.txt
                            }).ToList();

                            ProcessesID = mainAITask.TaskConfig.Data.ProcessesID;//回顾主任务线

                            WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, config, outputs, Logs);
                        }
                        else
                        {
                            //多个工作流的处理
                            var dataMsg = new StringBuilder();
                            List<WorkflowInfo> workflowInfos = WorkflowInfoBussiness.GetListByAgentID(AgentID);
                            if (workflowInfos != null)
                            {
                                //如果存在多个工作流，则由Agent主导模型判断调用工作流
                                if (workflowInfos.Count > 1)
                                {
                                    //List<WorkFlowProcesses> workFlowProcesses = new List<WorkFlowProcesses>();
                                    var kernelArguments = new KernelArguments();

                                    dataMsg.AppendLine("#你是一个任务分配能力极强的管理人员，可以合理的按照\"{{$input}}\"所描述的任务要求分析出需要使用一个或者多个工作流（Workflow）来完成工作。");
                                    dataMsg.AppendLine("");

                                    dataMsg.AppendLine("#这些是上个工作节点输出的一些参数");
                                    foreach (var input in inputs)
                                    {
                                        kernelArguments.Add(input.varname, input.value);

                                        dataMsg.AppendLine($"{input.varname}:");
                                        dataMsg.AppendLine($"{{${input.varname}}}");
                                        dataMsg.AppendLine("");
                                    }
                                    dataMsg.AppendLine("");

                                    dataMsg.AppendLine("#这些是本次任务的共用标识参数");
                                    dataMsg.AppendLine($"AppID:{AppID}");
                                    dataMsg.AppendLine($"SessionID:{SessionID}");
                                    dataMsg.AppendLine("");

                                    dataMsg.AppendLine("#这些是可以选择的工作流（Workflow）的参数列表，每一个工作流都有一个Workflow_Name来描述该工作流的名称，Description来描述该工作流具体可以处理解决的问题,WorkflowID是该工作流的标识符，是需要你判断后返回的参数。");
                                    dataMsg.AppendLine("WorkflowInfoList:[");
                                    foreach (var workflowInfo in workflowInfos)
                                    {
                                        //string _ProcessesID = Guid.NewGuid().ToString();
                                        dataMsg.AppendLine($"#{workflowInfo.WorkflowName}");
                                        dataMsg.AppendLine($"Workflow_Name:{workflowInfo.WorkflowName}");
                                        dataMsg.AppendLine($"Description:{workflowInfo.Description}");
                                        //dataMsg.AppendLine($"ProcessesID:{_ProcessesID}");
                                        dataMsg.AppendLine($"WorkflowID:{workflowInfo.WorkflowID}");

                                        //workFlowProcesses.Add(new WorkFlowProcesses() { WorkflowID = workflowInfo.WorkflowID, ProcessesID = _ProcessesID });
                                    }
                                    dataMsg.AppendLine("]");
                                    dataMsg.AppendLine("");

                                    dataMsg.AppendLine("#你可以根据任务要求判断是否与具体工作流(Workflow)相关（通过WorkflowInfoList中的Workflow_Name以及Description来分析是与合任务要求相关）,如果你觉得多个工作流相关则返回多个WorkflowID，并用逗号(\",\")隔开，不用返回WorkflowID以外的其他信息。");
                                    dataMsg.AppendLine("");

                                    CallFunction callFunction = new CallFunction();
                                    callFunction.Prompt = dataMsg.ToString();
                                    callFunction.Input = "" + inputs.Find(x => x.varname == "input")?.value;

                                    LargeModelConfig modelConfig = new LargeModelConfig();
                                    modelConfig.Model = LargeModelInfoBussiness.GetModel(nodeData.agent.SessionModelID ?? 0);
                                    modelConfig.Temperature = nodeData.agent.TemperatureCoefficient;
                                    modelConfig.TopPCoefficient = nodeData.agent.TopPCoefficient;

                                    string functionCallRe = _chatService.PromptFunctionCall(modelConfig, callFunction, kernelArguments).ConvertToString();
                                    if (!functionCallRe.IsNullOrEmpty())
                                    {
                                        string[] WorkflowIDs = functionCallRe.Trim().Split(",");
                                        List<string> TaskIDs = new List<string>();
                                        foreach (var workflowID in WorkflowIDs)
                                        {
                                            string NewTaskID = "";
                                            WorkflowNodeInfo workflowNode = WorkflowNodeInfoBussiness.GetWorkFlowAgentStartNode(workflowID);
                                            if (workflowNode != null)
                                            {
                                                NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(workflowNode.Config.ToString());
                                                if (nodeConfig != null)
                                                {
                                                    List<Output> outputs = new List<Output>();

                                                    NodeConfig targetNode = nodeConfig;

                                                    //需要传值给StartNode(由于下一节点的输入参数需要跟上一节点的输出参数匹配，StartNode作为下一节点，输入参数名称是input)，所以这个理的Output.varname=input
                                                    outputs.Add(new Output() { varname = "input", value = inputs.FirstOrDefault(x => x.varname == "input").value });

                                                    string _ProcessesID = Guid.NewGuid().ToString();

                                                    NewTaskID = TaskInfoBussiness.toTask(config, outputs, targetNode, AppID, SessionID,
                                                        _ProcessesID,
                                                        TaskID,
                                                        FromMainTaskID,
                                                        AgentNodeID);

                                                    TaskIDs.Add(NewTaskID);
                                                }

                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    WorkflowNodeInfo nodeInfo = WorkflowNodeInfoBussiness.GetAgentStartNode(nodeData.agent.AgentID);
                                    if (nodeInfo != null)
                                    {
                                        if (nodeInfo.Config != null)
                                        {
                                            NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(nodeInfo.Config.ToString());
                                            if (nodeConfig != null)
                                            {
                                                if (nodeConfig.data != null)
                                                {
                                                    StartData startData = JsonConvert.DeserializeObject<StartData>(nodeConfig.data.ToString());
                                                    if (startData != null)
                                                    {
                                                        startData.prompt = this.ReplacePromptValue(startData.prompt, inputs);
                                                        Outputs.Add(startData.prompt);
                                                        Logs.Add(startData.prompt);

                                                        List<Output> outputs = new List<Output>();
                                                        outputs.Add(new Output { varname = "results", value = startData.prompt });
                                                        outputs.Add(new Output { varname = "currentTime", value = DateTime.Now.ToDateTimeString() });
                                                        outputs.Add(new Output { varname = "agentName", value = nodeData.label });

                                                        WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, nodeConfig, outputs, Logs);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }

        public async Task<string> KnowledgeBaseNodeAsync(NodeConfig config, TaskData data)
        {
            string RecordID = "";
            List<string> Outputs = new List<string>();
            List<string> Logs = new List<string>();
            ExcutionRecordStatus excutionRecordStatus = new ExcutionRecordStatus();

            string AppID = data.AppID, TaskID = data.TaskID, SessionID = data.SessionID, ProcessesID=data.ProcessesID, AgentNodeID = data.AgentNodeID, FromMainTaskID = data.FromMainTaskID;
            List<Inputs> inputs = data.Inputs;
            RecordID = Utils.newExcutionRecord(SessionID, config, ProcessesID);
            string results = "";

            try
            {
                if (!AppID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty())
                {
                    KnowledgeBaseData nodeData = JsonConvert.DeserializeObject<KnowledgeBaseData>(config.data.ToString());
                    if (nodeData != null)
                    {
                        List<KnowledgeBaseUnit> KnowledgeBaseUnits = new List<KnowledgeBaseUnit>();

                        foreach (var knowledgeBase in nodeData.knowledgeBase) {
                            KnowledgeBaseUnit knowledgeBaseUnit = new KnowledgeBaseUnit();

                            knowledgeBaseUnit.KnowledgeBaseInfo = knowledgeBase;
                            var chatModel = LargeModelInfoBussiness.GetModel(knowledgeBase.PreprocessModelID);
                            var embedModel = LargeModelInfoBussiness.GetModel(knowledgeBase.VectorModelID);

                            knowledgeBaseUnit.LargeModelUnit = new LargeModelUnit();
                            knowledgeBaseUnit.LargeModelUnit.ChatModel = new LargeModelConfig()
                            {
                                Id = knowledgeBase.PreprocessModelID.ToString(),
                                Model = chatModel,
                                TopPCoefficient = nodeData.topp,
                                Temperature = nodeData.temperature,
                                //Prompt = nodeData.prompt
                            };

                            knowledgeBaseUnit.LargeModelUnit.VectorModel = new LargeModelConfig()
                            {
                                Id = knowledgeBase.VectorModelID.ToString(),
                                Model = embedModel,
                                TopPCoefficient = nodeData.topp,
                                Temperature = nodeData.temperature,
                                //Prompt = nodeData.prompt
                            };

                            //knowledgeBaseUnit.LargeModelUnit.RerankModel = new LargeModelConfig();

                            KnowledgeBaseUnits.Add(knowledgeBaseUnit);
                        }

                        LargeModelConfig ChatModel = new LargeModelConfig();
                        ChatModel.Id = nodeData.model.LargeModelID.ToString();
                        ChatModel.Model = nodeData.model;
                        ChatModel.TopPCoefficient = nodeData.topp;
                        ChatModel.Temperature = nodeData.temperature;
                        ChatModel.Prompt = nodeData.prompt;

                        string questions = nodeData.prompt.IsNullOrEmpty() ? JsonConvert.SerializeObject(inputs) : this.ReplacePromptValue(nodeData.prompt, inputs);

                        ChatHistory history = new ChatHistory();
                        List<AppChatLogInfo> appChatLogs = AppChatLogInfoBussiness.GetListBySessionID(AppID, SessionID);
                        history = _chatService.GetChatHistory(appChatLogs, history);

                        var chatResult = _chatService.SendKmsAsync(KnowledgeBaseUnits, ChatModel, questions, history);
                        StringBuilder rawContent = new StringBuilder();
                        Chats info = null;
                        List<Chats> MessageList = [];

                        await foreach (var content in chatResult)
                        {
                            if (info == null)
                            {
                                rawContent.Append(content.ConvertToString());
                                info = new Chats();
                                info.Id = Guid.NewGuid().ToString();
                                info.UserName = AuthorRole.Assistant.ToString();
                                info.AppId = AppID;
                                info.Context = content.ConvertToString();
                                info.CreateTime = DateTime.Now;

                                MessageList.Add(info);
                            }
                            else
                            {
                                rawContent.Append(content.ConvertToString());
                            }
                            info.Context = rawContent.ToString();
                        }
                        results = info.Context;
                    }
                    Outputs.Add(results);
                    Logs.Add(results);

                    List<Output> outputs = new List<Output>();
                    outputs.Add(new Output { varname = "results", value = results });

                    WorkflowNodeInfoBussiness.NextNode(AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID, config, outputs, Logs);
                }

                excutionRecordStatus = ExcutionRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Logs.Add(ex.Message);
                excutionRecordStatus = ExcutionRecordStatus.Fail;
            }

            Utils.updateExcutionRecord(RecordID, excutionRecordStatus, Outputs, Logs);
            return RecordID;
        }
    }
}

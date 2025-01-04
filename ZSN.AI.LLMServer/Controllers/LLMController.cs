
using ZSN.AI.LLMServer.Controllers;
using ZSN.Utils.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZSN.AI.BLL;
using ZSN.AI.Entity;
using System.Net;
using ZSN.Utils.Core.Extensions;
using ZSN.AI.Service.Attributes;
using ZSN.AI.LLMServer.Attributes;
using ZSN.AI.Service.Controllers;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using ZSN.AI.Core.Repositories;
using ZSN.AI.Core.Interface;
using ZSN.AI.Core.Utils;
using Markdig;
using ZSN.AI.LLMServer.Helpers;
using StackExchange.Redis;
using ZSN.AI.Entity.Chat;
using ZSN.AI.Node;
using Microsoft.Extensions.Logging;

namespace ZSN.AI.LLMServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LLMController : ApiBaseController
    {
        private readonly IChatService _chatService;
        public LLMController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HiddenApi]
        [HttpGet]
        public IActionResult Index()
        {
            return BuildSuccessResult(new { msg = "success" });
        }
        
        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("application/json")]
        [APIChecker(Token = false)]
        public JsonMsg<IReadOnlyList<MessageData>> ChatSync([FromBody] PostData paramValue)
        {
            List<MessageData> messageDataList = new List<MessageData>();

            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                int LargeModelID = jObject.JsonGetValue<int>("LargeModelID", 0);
                GptMsg Inputs = jObject.JsonGetValue<GptMsg>("Messages");
                string SessionID = jObject.JsonGetValue<string>("SessionID");
                string MemberID = jObject.JsonGetValue<string>("MemberID");

                LargeModelConfig modelConfig = jObject.JsonGetValue<LargeModelConfig>("Config", null);

                AppChatSessionInfo appChatSession = new AppChatSessionInfo();

                string AppID = SettingsService.Current.AppID;
                MessageData messageData = new MessageData();
                messageData.AppID = AppID;
                messageData.ProcessesID = Guid.NewGuid().ToString();

                WorkflowNodeInfo nodeInfo = WorkflowNodeInfoBussiness.GetAppStartNode(App.AppID);
                if (nodeInfo != null)
                {
                    if (nodeInfo.Config != null)
                    {
                        NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(nodeInfo.Config.ToString());
                        if (nodeConfig != null)
                        {
                            if (nodeConfig.data != null)
                            {
                                if (SessionID.IsNullOrEmpty())
                                {
                                    SessionID = Guid.NewGuid().ToString();
                                    appChatSession.AppID = AppID;
                                    appChatSession.ChatSessionID = SessionID;
                                    appChatSession.MemberID = MemberID;
                                    appChatSession.DesensitizedName = "";
                                    appChatSession.IsCoCreate = 0;
                                    appChatSession.SystemStatus = 0;
                                    appChatSession.CreateTime = DateTime.Now;

                                    AppChatSessionInfoBussiness.Add(appChatSession);
                                }
                                else
                                {
                                    appChatSession = AppChatSessionInfoBussiness.GetModel(SessionID);
                                }

                                messageData.SessionID = SessionID;

                                TaskData data = new TaskData() { AppID = AppID, SessionID = SessionID, ProcessesID = messageData.ProcessesID, AgentNodeID = "" };
                                data.Inputs = new List<Inputs>();
                                data.Inputs.Add(new Inputs() { value = Inputs.content, varname = "input" });

                                AppChatLogInfoBussiness.Add(AppID, SessionID, AuthorRole.User.ToString(), Inputs, LargeModelID);

                                Excution excutionNode = new Excution(_chatService);

                                string RecordID = excutionNode.StartNode(nodeConfig, data);

                                messageData.Content = messageData.ProcessesID;

                            }
                        }
                    }
                    messageDataList.Add(messageData);
                }
                return JsonMsg<IReadOnlyList<MessageData>>.OK(messageDataList, SessionID: SessionID);
            }
            else
            {
                return JsonMsg<IReadOnlyList<MessageData>>.Error(null, ErrorCode.DataFormatError);
            }
        }
        
        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("application/json")]
        [APIChecker(Token = false)]
        public async Task<JsonMsg<IReadOnlyList<ChatMessageContent>>> ChatAsync([FromBody] PostData paramValue)
        {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                int LargeModelID = jObject.JsonGetValue<int>("LargeModelID", 0);
                GptMsg Inputs = jObject.JsonGetValue<GptMsg>("Messages");
                string SessionID = jObject.JsonGetValue<string>("SessionID");
                string MemberID = jObject.JsonGetValue<string>("MemberID");

                LargeModelConfig modelConfig = jObject.JsonGetValue<LargeModelConfig>("Config", null);
                ChatHistory history = new ChatHistory();
                List<ChatMessageContent> _Message = new List<ChatMessageContent>();
                AppChatSessionInfo appChatSession = new AppChatSessionInfo();

                int ChatCount = 0;
                string AppID = SettingsService.Current.AppID;
                string _Outpus = "";

                if (LargeModelID <= 0)
                {
                    if (this.App != null)
                    {
                        LargeModelID = this.App.SessionModelID;
                    }
                    else
                    {
                        return JsonMsg<IReadOnlyList<ChatMessageContent>>.Error(null, ErrorCode.DataEmpty);
                    }
                }

                LargeModelInfo largeModel = LargeModelInfoBussiness.GetModel(LargeModelID);
                if (largeModel == null)
                {
                    return JsonMsg<IReadOnlyList<ChatMessageContent>>.Error(null, ErrorCode.NoModel);
                }

                if (Inputs == null)
                {
                    return JsonMsg<IReadOnlyList<ChatMessageContent>>.Error(null, ErrorCode.NoInputs);
                }

                history.AddSystemMessage(this.App.Prompt);

                if (modelConfig == null)//无外部参数，则使用配置参数
                {
                    modelConfig = new LargeModelConfig();
                    modelConfig.Id = largeModel.LargeModelID.ToString();
                    modelConfig.Prompt = App.Prompt;

                    modelConfig.Temperature = (double)App.TemperatureCoefficient;
                    modelConfig.TopPCoefficient = (double)App.TopPCoefficient;

                    //获取APP工作流的主控AI的定义
                    WorkflowNodeInfo nodeInfo = WorkflowNodeInfoBussiness.GetAppMainAINode(App.AppID);
                    if (nodeInfo != null)
                    {
                        if (nodeInfo.Config != null)
                        {
                            NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(nodeInfo.Config.ToString());
                            if (nodeConfig != null)
                            {
                                if (nodeConfig.data != null)
                                {
                                    MainAIData mainAIData = JsonConvert.DeserializeObject<MainAIData>(nodeConfig.data.ToString());
                                    if (mainAIData != null)
                                    {
                                        modelConfig.SemanticFunction = mainAIData.SemanticFunction;
                                        modelConfig.NativeFunction = mainAIData.NativeFunction;

                                        //以APP工作流的配置参数为最终参数
                                        modelConfig.Prompt = mainAIData.prompt;
                                        modelConfig.Temperature = mainAIData.temperature;
                                        modelConfig.TopPCoefficient = mainAIData.topp;
                                    }
                                }
                            }
                        }
                    }
                }

                if (modelConfig.Prompt.IsNullOrEmpty()) { history.AddSystemMessage(modelConfig.Prompt); }

                modelConfig.Model = largeModel;

                if (SessionID.IsNullOrEmpty())
                {
                    SessionID = Guid.NewGuid().ToString();
                    appChatSession.AppID = AppID;
                    appChatSession.ChatSessionID = SessionID;
                    appChatSession.MemberID = MemberID;
                    appChatSession.DesensitizedName = "";
                    appChatSession.IsCoCreate = 0;
                    appChatSession.SystemStatus = 0;
                    appChatSession.CreateTime = DateTime.Now;

                    AppChatSessionInfoBussiness.Add(appChatSession);
                }
                else
                {
                    appChatSession = AppChatSessionInfoBussiness.GetModel(SessionID);
                    List<AppChatLogInfo> appChatLogs = AppChatLogInfoBussiness.GetListBySessionID(AppID, SessionID);
                    List<AppChatSummaryInfo> appChatSummaries = AppChatSummaryInfoBussiness.GetListBySessionID(AppID, SessionID);

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

                }

                history.AddUserMessage(Inputs.content);

                List<Chats> MessageList = [];
                Chats info = null;
                var chatResult = _chatService.SendChatAsync(modelConfig, history);
                StringBuilder rawContent = new StringBuilder();
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
                _Outpus = info.Context;

                AppChatLogInfo appChat = new AppChatLogInfo();
                appChat.ChatLogID = Guid.NewGuid().ToString();
                appChat.AppID = AppID;
                appChat.ChatSessionID = SessionID;
                appChat.Direction = 0;
                appChat.Role = AuthorRole.User.ToString();
                appChat.LargeModelID = LargeModelID;
                appChat.Content = JsonConvert.SerializeObject(Inputs);
                appChat.CreateTime = DateTime.Now;
                appChat.LogOrder = ChatCount++;

                AppChatLogInfoBussiness.Add(appChat);

                GptMsg _msg = new GptMsg();
                _msg.role = AuthorRole.Assistant.ToString();
                _msg.content = info.Context;

                appChat = new AppChatLogInfo();
                appChat.ChatLogID = Guid.NewGuid().ToString();
                appChat.AppID = AppID;
                appChat.ChatSessionID = SessionID;
                appChat.Direction = 1;
                appChat.Role = AuthorRole.Assistant.ToString();
                appChat.LargeModelID = LargeModelID;
                appChat.Content = _msg;
                appChat.CreateTime = DateTime.Now;
                appChat.LogOrder = ChatCount++;

                AppChatLogInfoBussiness.Add(appChat);

                _Message.Clear();
                _Message.Add(new(AuthorRole.Assistant, _Outpus, modelId: largeModel.Name));

                Console.WriteLine(JsonConvert.SerializeObject(history));
                Console.WriteLine("========");

                //向记录员AI下发工作任务
                WorkflowNodeInfo reporterNodeInfo = WorkflowNodeInfoBussiness.GetAppReporterNode(App.AppID);
                if (reporterNodeInfo != null)
                {
                    if (reporterNodeInfo.Config != null)
                    {
                        NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(reporterNodeInfo.Config.ToString());
                        if (nodeConfig != null)
                        {
                            if (nodeConfig.data != null)
                            {
                                ReporterData reporterData = JsonConvert.DeserializeObject<ReporterData>(nodeConfig.data.ToString());

                                if (reporterData.enable && history.Count >= reporterData.recordslength)
                                {
                                    TaskData taskData = new TaskData();
                                    taskData.AppID = AppID;
                                    taskData.SessionID = SessionID;

                                    TaskInfo taskInfo = new TaskInfo();
                                    taskInfo.TaskType = NodeType.Reporter;
                                    taskInfo.TaskConfig = new TaskConfig();
                                    taskInfo.TaskConfig.NodeConfig = nodeConfig;
                                    taskInfo.LoopType = LoopType.NOLoop;
                                    taskInfo.RepeatValue = 1;
                                    taskInfo.RedoCount = 0;

                                    taskInfo.TaskConfig.Data = taskData;

                                    TaskInfoBussiness.Add(taskInfo);
                                }
                            }
                        }
                    }
                }
                return JsonMsg<IReadOnlyList<ChatMessageContent>>.OK(_Message, SessionID: SessionID);

            }
            else
            {
                return JsonMsg<IReadOnlyList<ChatMessageContent>>.Error(null, ErrorCode.DataFormatError);
            }
        }

        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("application/json")]
        [APIChecker(Token = false)]
        public JsonMsg<IReadOnlyList<TextContent>> Text([FromBody] PostData paramValue)
        {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                int LargeModelID = jObject.JsonGetValue<int>("LargeModelID", 0);
                string Inputs = jObject.JsonGetValue<string>("Inputs");
                string Prompt = jObject.JsonGetValue<string>("Prompt");

                string _Outpus = "";

                LargeModelInfo largeModel = LargeModelInfoBussiness.GetModel(LargeModelID);
                if (largeModel == null)
                {

                    return JsonMsg<IReadOnlyList<TextContent>>.Error(null, ErrorCode.NoModel);
                }
                if (Inputs.IsNullOrEmpty())
                {
                    return JsonMsg<IReadOnlyList<TextContent>>.Error(null, ErrorCode.NoInputs);
                }

                List<TextContent> _Message = new List<TextContent>();
                TextContent text = new TextContent();


                text.Text = _Outpus;

                _Message.Add(text);

                return JsonMsg<IReadOnlyList<TextContent>>.OK(_Message);
            }
            else
            {
                return JsonMsg<IReadOnlyList<TextContent>>.Error(null, ErrorCode.DataFormatError);
            }
        }

        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("application/json")]
        [APIChecker(Token = false)]
        public JsonMsg<IReadOnlyList<Embeddings>> Embedding([FromBody] PostData paramValue)
        {

            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                int LargeModelID = jObject.JsonGetValue<int>("LargeModelID", 0);
                string Inputs = jObject.JsonGetValue<string>("Inputs");

                if (LargeModelID <= 0)
                {
                    return JsonMsg<IReadOnlyList<Embeddings>>.Error(null, ErrorCode.DataEmpty);
                }

                LargeModelInfo largeModel = LargeModelInfoBussiness.GetModel(LargeModelID);
                if (largeModel == null)
                {

                    return JsonMsg<IReadOnlyList<Embeddings>>.Error(null, ErrorCode.NoModel);
                }

                if (Inputs.IsNullOrEmpty())
                {
                    return JsonMsg<IReadOnlyList<Embeddings>>.Error(null, ErrorCode.NoInputs);
                }

                decimal[] _Outpus = new decimal[0];
                List<Embeddings> _Message = new List<Embeddings>();
                Embeddings _Data = new Embeddings();

                _Data.embedding = _Outpus;

                _Message.Add(_Data);

                return JsonMsg<IReadOnlyList<Embeddings>>.OK(_Message);
            }
            else
            {
                return JsonMsg<IReadOnlyList<Embeddings>>.Error(null, ErrorCode.DataFormatError);
            }
        }

        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("application/json")]
        [APIChecker(Token = false)]
        public JsonMsg<ProcessInfo> GetNodeLogs([FromBody] PostData paramValue)
        {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                string AppID = SettingsService.Current.AppID;
                string SessionID = jObject.JsonGetValue<string>("SessionID").SecureSQL();
                string ProcessesID = jObject.JsonGetValue<string>("ProcessesID").SecureSQL();
                string MemberID = jObject.JsonGetValue<string>("MemberID").SecureSQL();

                if (!SessionID.IsNullOrEmpty() && !ProcessesID.IsNullOrEmpty())
                {
                    List<WorkflowNodeExcutionRecordInfo> workflowNodeExcutionRecords = new List<WorkflowNodeExcutionRecordInfo>();

                    workflowNodeExcutionRecords = WorkflowNodeExcutionRecordInfoBussiness.GetListBySessionIDProcessesID(SessionID, ProcessesID);

                    WorkflowNodeExcutionRecordInfo ProcessesEndRecord = workflowNodeExcutionRecords.FirstOrDefault(e => e.NodeName == NodeType.End.ToString());

                    ProcessInfo process = new ProcessInfo();
                    process.ProcessID = ProcessesID;
                    process.Status = ProcessesEndRecord != null ? ProcessesEndRecord.Status == ExcutionRecordStatus.Success ? ProcessStatus.Success : ProcessStatus.Running : ProcessStatus.Running;
                    process.Results = ProcessesEndRecord != null ? ProcessesEndRecord.Outputs : "";
                    process.ExcutionRecordInfos = workflowNodeExcutionRecords;

                    return JsonMsg<ProcessInfo>.OK(process, SessionID = SessionID);
                }
                else
                {
                    return JsonMsg<ProcessInfo>.Error(null, ErrorCode.DataFormatError);
                }

            }
            else
            {
                return JsonMsg<ProcessInfo>.Error(null, ErrorCode.DataFormatError);
            }
        }
    }
}

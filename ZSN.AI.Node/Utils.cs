using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using ZSN.Utils.Core.Extensions;
using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.AI.Entity.Workflow;

namespace ZSN.AI.Node
{
    public class Utils
    {
        /// <summary>
        /// 初始化新工作流，组织初始化节点
        /// </summary>
        /// <param name="MainID"></param>
        /// <param name="MainType"></param>
        /// <returns></returns>
        public static WorkFlow initWorkFlow(string MainID, int MainType)
        {

            WorkFlow workflow = new WorkFlow();
            workflow.Info = new WorkflowInfo();
            workflow.Info.MainID = MainID;
            workflow.Info.MainType = MainType;
            workflow.Info.WorkflowName = MainType == 1 ? "应用 工作流" : "助理 工作流";

            workflow.Nodes = new List<WorkflowNodeInfo>();
            Inputs inputs = new Inputs();
            Output output = new Output();

            if (MainType == 1)
            {
                
                AppInfo appInfo = AppInfoBussiness.GetModel(MainID);

                #region MainAI
                WorkflowNodeInfo MainAINode = new WorkflowNodeInfo();
                MainAINode.NodeID = Guid.NewGuid().ToString();
                MainAINode.WorkflowID = workflow.Info.WorkflowID;
                MainAINode.NodeType = NodeType.MainAI;
                MainAINode.NodeName = "主控AI";

                NodeConfig MainAINodeConfig = new NodeConfig();
                MainAINodeConfig.id = MainAINode.NodeID;
                MainAINodeConfig.mainid = MainID;
                MainAINodeConfig.workflowid = workflow.Info.WorkflowID;
                MainAINodeConfig.type = NodeType.MainAI;
                MainAINodeConfig.position.x = 16;
                MainAINodeConfig.position.y = 80;

                MainAIData mainAIData = new MainAIData();

                mainAIData.prompt = appInfo.Prompt;
                mainAIData.topp = appInfo.TopPCoefficient;
                mainAIData.temperature = appInfo.TemperatureCoefficient;

                LargeModelInfo largeModelInfo = new LargeModelInfo();
                largeModelInfo.LargeModelID = appInfo.SessionModelID;

                inputs = new Inputs();
                inputs.varname = "input";
                mainAIData.inputs.Add(inputs);

                output = new Output();
                output.varname = "results";
                mainAIData.output.Add(output);

                mainAIData.model = largeModelInfo;

                MainAINodeConfig.data = mainAIData;

                MainAINode.Config = MainAINodeConfig;
                workflow.Nodes.Add(MainAINode);
                #endregion

                #region TimeTrigger
                WorkflowNodeInfo TimeTriggerNode = new WorkflowNodeInfo();
                TimeTriggerNode.NodeID = Guid.NewGuid().ToString();
                TimeTriggerNode.WorkflowID = workflow.Info.WorkflowID;
                TimeTriggerNode.NodeType = NodeType.TimeTrigger;
                TimeTriggerNode.NodeName = "时间触发器";

                NodeConfig TimeTriggerNodeConfig = new NodeConfig();
                TimeTriggerNodeConfig.id = TimeTriggerNode.NodeID;
                TimeTriggerNodeConfig.mainid = MainID;
                TimeTriggerNodeConfig.workflowid = workflow.Info.WorkflowID;
                TimeTriggerNodeConfig.type = NodeType.TimeTrigger;
                TimeTriggerNodeConfig.position.x = 688;
                TimeTriggerNodeConfig.position.y = 80;

                TimeTriggerData timeTriggerData = new TimeTriggerData();
                TimeTrigger timeTrigger = new TimeTrigger();

                timeTriggerData.timeTrigger = timeTrigger;
                timeTriggerData.output = new List<Output>();
                timeTriggerData.output.Add(new Output() { varname= "currentTime", value= "{{currentTime}}",type="DateTime",txt="当前时间" });

                TimeTriggerNodeConfig.data = timeTriggerData;

                TimeTriggerNode.Config = TimeTriggerNodeConfig;
                workflow.Nodes.Add(TimeTriggerNode);
                #endregion

                #region Agent
                WorkflowNodeInfo AgentNode = new WorkflowNodeInfo();
                AgentNode.NodeID = Guid.NewGuid().ToString();
                AgentNode.WorkflowID = workflow.Info.WorkflowID;
                AgentNode.NodeType = NodeType.Agent;
                AgentNode.NodeName = "Agent";

                NodeConfig AgentNodeConfig = new NodeConfig();
                AgentNodeConfig.id = AgentNode.NodeID;
                AgentNodeConfig.mainid = MainID;
                AgentNodeConfig.workflowid = workflow.Info.WorkflowID;
                AgentNodeConfig.type = NodeType.Agent;
                AgentNodeConfig.position.x = 688;
                AgentNodeConfig.position.y = 505;

                AgentData AgentData = new AgentData();
                inputs = new Inputs();
                inputs.varname = "input";
                AgentData.inputs.Add(inputs);

                output = new Output();
                output.varname = "results";
                AgentData.output.Add(output);

                AgentData.agent = new AgentInfo();

                AgentNodeConfig.data = AgentData;

                AgentNode.Config = AgentNodeConfig;
                workflow.Nodes.Add(AgentNode);
                #endregion


                #region Reporter
                WorkflowNodeInfo ReporterNode = new WorkflowNodeInfo();
                ReporterNode.NodeID = Guid.NewGuid().ToString();
                ReporterNode.WorkflowID = workflow.Info.WorkflowID;
                ReporterNode.NodeType = NodeType.Reporter;
                ReporterNode.NodeName = "记录员";

                NodeConfig ReporterNodeConfig = new NodeConfig();
                ReporterNodeConfig.id = ReporterNode.NodeID;
                ReporterNodeConfig.mainid = MainID;
                ReporterNodeConfig.workflowid = workflow.Info.WorkflowID;
                ReporterNodeConfig.type = NodeType.Reporter;
                ReporterNodeConfig.position.x = -808;
                ReporterNodeConfig.position.y = 596;

                ReporterData reporterData = new ReporterData();

                reporterData.prompt = "你是一个谈话记录员,负责将对话内容进行整理,按角色分别提取关键点,并有条理得整理成Json格式，总结后的内容方便你再一次提取理解会话过程,有效避免会话上下文太长导致你的记忆丢失的问题。\r\n\n\r" +
                                "例子:\n" +
                                "{\n" +
                                "\"User\":\"User的内容的总结\",\n\"+" +
                                "\"Assistant\":\"Assistant的内容的总结\"\n" +
                                 "\"Tool\":\"Tool的内容的总结\",\n\"+" +
                                "}\n";
                reporterData.topp = appInfo.TopPCoefficient;
                reporterData.temperature = appInfo.TemperatureCoefficient;

                LargeModelInfo reporterLargeModelInfo = new LargeModelInfo();
                reporterLargeModelInfo.LargeModelID = appInfo.SessionModelID;

                reporterData.model = reporterLargeModelInfo;

                ReporterNodeConfig.data = reporterData;

                ReporterNode.Config = ReporterNodeConfig;
                workflow.Nodes.Add(ReporterNode);
                #endregion

                WorkflowNodeInfo StartNode = new WorkflowNodeInfo();
                StartNode.NodeID = Guid.NewGuid().ToString();
                StartNode.WorkflowID = workflow.Info.WorkflowID;
                StartNode.NodeType = NodeType.Start;
                StartNode.NodeName = "开始";

                NodeConfig StartNodeConfig = new NodeConfig();
                StartNodeConfig.id = StartNode.NodeID;
                StartNodeConfig.mainid = MainID;
                StartNodeConfig.workflowid = workflow.Info.WorkflowID;
                StartNodeConfig.type = NodeType.Start;
                StartNodeConfig.position.x = -808;
                StartNodeConfig.position.y = 80;

                StartData startData = new StartData();
                startData.inputs.Add(new Inputs());
                startData.output.Add(new Output() { varname = "prompt" });
                startData.output.Add(new Output { varname = "currentTime", value = "{{currentTime}}", type = "DateTime", txt = "当前时间" });

                StartNodeConfig.data = startData;

                StartNode.Config = StartNodeConfig;

                StartNode.CreateTime = DateTime.Now;
                StartNode.LastUpdateTime = DateTime.Now;
                workflow.Nodes.Add(StartNode);

                WorkflowNodeInfo EndNode = new WorkflowNodeInfo();
                EndNode.NodeID = Guid.NewGuid().ToString();
                EndNode.WorkflowID = workflow.Info.WorkflowID;
                EndNode.NodeType = NodeType.End;
                EndNode.NodeName = "结束";

                NodeConfig EndNodeConfig = new NodeConfig();
                EndNodeConfig.id = EndNode.NodeID;
                EndNodeConfig.mainid = MainID;
                EndNodeConfig.workflowid = workflow.Info.WorkflowID;
                EndNodeConfig.type = NodeType.End;
                EndNodeConfig.position.x = 1230;
                EndNodeConfig.position.y = 80;

                EndData endData = new EndData();
                inputs = new Inputs();
                inputs.varname = "input";
                endData.inputs.Add(inputs);

                EndNodeConfig.data = endData;

                EndNode.Config = EndNodeConfig;

                EndNode.CreateTime = DateTime.Now;
                EndNode.LastUpdateTime = DateTime.Now;
                workflow.Nodes.Add(EndNode);
            }
            else
            {
                WorkflowNodeInfo StartNode = new WorkflowNodeInfo();
                StartNode.NodeID = Guid.NewGuid().ToString();
                StartNode.WorkflowID = workflow.Info.WorkflowID;
                StartNode.NodeType = NodeType.AgentStart;
                StartNode.NodeName = "开始";

                NodeConfig StartNodeConfig = new NodeConfig();
                StartNodeConfig.id = StartNode.NodeID;
                StartNodeConfig.mainid = MainID;
                StartNodeConfig.workflowid = workflow.Info.WorkflowID;
                StartNodeConfig.type = NodeType.AgentStart;
                StartNodeConfig.position.x = -808;
                StartNodeConfig.position.y = 80;

                AgentStartData startData = new AgentStartData();
                startData.inputs.Add(new Inputs());
                startData.output.Add(new Output() { varname = "prompt" });
                startData.output.Add(new Output { varname = "currentTime", value = "{{currentTime}}", type = "DateTime", txt = "当前时间" });

                StartNodeConfig.data = startData;

                StartNode.Config = StartNodeConfig;

                StartNode.CreateTime = DateTime.Now;
                StartNode.LastUpdateTime = DateTime.Now;
                workflow.Nodes.Add(StartNode);

                WorkflowNodeInfo EndNode = new WorkflowNodeInfo();
                EndNode.NodeID = Guid.NewGuid().ToString();
                EndNode.WorkflowID = workflow.Info.WorkflowID;
                EndNode.NodeType = NodeType.AgentEnd;
                EndNode.NodeName = "结束";

                NodeConfig EndNodeConfig = new NodeConfig();
                EndNodeConfig.id = EndNode.NodeID;
                EndNodeConfig.mainid = MainID;
                EndNodeConfig.workflowid = workflow.Info.WorkflowID;
                EndNodeConfig.type = NodeType.AgentEnd;
                EndNodeConfig.position.x = 1230;
                EndNodeConfig.position.y = 80;

                AgentEndData endData = new AgentEndData();
                endData.inputs.Add(new Inputs());

                endData.output.Add(new Output() { varname = "results" });
                endData.output.Add(new Output { varname = "currentTime", value = "{{currentTime}}", type = "DateTime", txt = "当前时间" });
                endData.output.Add(new Output { varname = "agentName", value = "{{agentName}}", type = "String" });

                EndNodeConfig.data = endData;

                EndNode.Config = EndNodeConfig;

                EndNode.CreateTime = DateTime.Now;
                EndNode.LastUpdateTime = DateTime.Now;
                workflow.Nodes.Add(EndNode);
            }

            return workflow;
        }

        /// <summary>
        /// 初始化节点
        /// </summary>
        /// <param name="WorkflowID"></param>
        /// <param name="NodeType"></param>
        /// <param name="MainID"></param>
        /// <returns></returns>
        public static WorkflowNodeInfo newNode(string WorkflowID, NodeType nodeType, string MainID)
        {

            WorkflowNodeInfo nodeInfo = new WorkflowNodeInfo();
            nodeInfo.WorkflowID = WorkflowID;
            nodeInfo.NodeType = nodeType;
            nodeInfo.NodeID = Guid.NewGuid().ToString();

            NodeConfig nodeConfig = new NodeConfig();
            nodeConfig.id = nodeInfo.NodeID;
            nodeConfig.mainid = MainID;
            nodeConfig.workflowid = WorkflowID;
            nodeConfig.type = nodeType;

            switch (nodeType)
            {
                case NodeType.Start:
                case NodeType.AgentStart:

                    StartData startData = new StartData();
                    startData.inputs.Add(new Inputs { varname = "input" });

                    startData.output.Add(new Output { varname = "prompt" });
                    startData.output.Add(new Output { varname = "currentTime" , value = "{{currentTime}}", type = "DateTime", txt = "当前时间" });

                    nodeConfig.data = startData;
                    break;

                case NodeType.End:
                case NodeType.AgentEnd:

                    EndData endData = new EndData();
                    endData.inputs.Add(new Inputs { varname = "input" });

                    nodeConfig.data = endData;
                    break;

                case NodeType.LargeModel:

                    LargeModelData largeModelData = new LargeModelData();
                    largeModelData.inputs.Add(new Inputs { varname = "input" });

                    largeModelData.output.Add(new Output { varname = "results" });

                    nodeConfig.data = largeModelData;
                    break;
                case NodeType.MainAI:

                    MainAIData mainAIData = new MainAIData();
                    mainAIData.inputs.Add(new Inputs { varname = "input" });
                    mainAIData.output.Add(new Output { varname = "results" });
                    mainAIData.output.Add(new Output { varname = "complete_type" });

                    nodeConfig.data = mainAIData;
                    break;
                case NodeType.Reporter:

                    ReporterData reporterData = new ReporterData();
                    reporterData.inputs.Add(new Inputs { varname = "input" });

                    reporterData.output.Add(new Output { varname = "results" });

                    nodeConfig.data = reporterData;
                    break;
                case NodeType.KnowledgeBase:
                    KnowledgeBaseData knowledgeBaseData = new KnowledgeBaseData();
                    knowledgeBaseData.inputs.Add(new Inputs { varname = "input" });
                    knowledgeBaseData.output.Add(new Output { varname = "results" });

                    nodeConfig.data = knowledgeBaseData;
                    break;

                case NodeType.Plugins:
                    PluginsData pluginsData = new PluginsData();

                    pluginsData.inputs.Add(new Inputs { varname = "input" });

                    pluginsData.output.Add(new Output { varname = "results" });

                    nodeConfig.data = pluginsData;
                    break;

                case NodeType.Selector:
                    SelectorData selectorData = new SelectorData();
                    selectorData.inputs.Add(new Inputs { varname = "input" });

                    selectorData.output.Add(new Output { varname = "results" });

                    nodeConfig.data = selectorData;
                    break;
                case NodeType.TimeTrigger:

                    TimeTriggerData timeTriggerData = new TimeTriggerData();
                    TimeTrigger timeTrigger = new TimeTrigger();

                    timeTriggerData.timeTrigger = timeTrigger;
                    timeTriggerData.output.Add(new Output() { varname = "prompt", value = "{{prompt}}", type = "string", txt = "" });
                    timeTriggerData.output.Add(new Output() { varname = "currentTime", value = "{{currentTime}}", type = "DateTime", txt = "当前时间" });

                    nodeConfig.data = timeTriggerData;
                    break;
                case NodeType.Agent:

                    AgentData AgentData = new AgentData();

                    AgentData.inputs.Add(new Inputs { varname = "input" });

                    AgentData.output.Add(new Output { varname = "results" });
                    AgentData.output.Add(new Output { varname = "currentTime", type = "DateTime" });
                    AgentData.output.Add(new Output { varname = "agentName" });

                    AgentData.agent = new AgentInfo();

                    nodeConfig.data = AgentData;
                    break;

            }
            nodeInfo.Config = nodeConfig;

            return nodeInfo;
        }

        

        /// <summary>
        /// 添加节点执行记录
        /// </summary>
        /// <param name="SessionID"></param>
        /// <param name="CurrentNode"></param>
        /// <param name="NextNodeID"></param>
        /// <returns>RecordID</returns>
        public static string newExcutionRecord(string SessionID, NodeConfig CurrentNode,string ProcessesID, string NextNodeID="")
        {
            WorkflowNodeExcutionRecordInfo recordInfo = new WorkflowNodeExcutionRecordInfo();
            recordInfo.SessionID = SessionID;
            recordInfo.ProcessesID = ProcessesID;
            recordInfo.NextNodeID = NextNodeID;
            recordInfo.WorkflowID = CurrentNode.workflowid;
            recordInfo.NodeID = CurrentNode.id;
            recordInfo.StartTime = DateTime.Now;
            recordInfo.EndTime = DateTime.Now;
            recordInfo.Status = ExcutionRecordStatus.Running;
            recordInfo.Inputs = CurrentNode.data;
            recordInfo.Outputs = null;
            recordInfo.Logs = null;
            recordInfo.NodeName = CurrentNode.type.ToString();
            
            switch (CurrentNode.type)
            {
                case NodeType.End:
                    recordInfo.NextNodeID = NodeType.End.ToString();
                    break;
                case NodeType.AgentEnd:
                    recordInfo.NextNodeID = NodeType.AgentEnd.ToString();
                    break;
            }

            WorkflowNodeExcutionRecordInfoBussiness.Add(recordInfo);

            return recordInfo.RecordID;
        }

        /// <summary>
        /// 记录更新执行结果
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="Status"></param>
        /// <param name="Outputs"></param>
        /// <param name="Logs"></param>
        public static void updateExcutionRecord(string RecordID, ExcutionRecordStatus Status, object Outputs,object Logs) {

            WorkflowNodeExcutionRecordInfoBussiness.Update( RecordID,  Status,  Outputs,  Logs);
        }
    }
}

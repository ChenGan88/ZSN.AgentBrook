using Microsoft.SemanticKernel;
using System;
using System.ComponentModel;
using ZSN.Utils.Core.Extensions;
using ZSN.Utils.Core.Helpers;
using ZSN.AI.BLL;
using ZSN.AI.Entity;

using Lucene.Net.Util.Fst;
using Microsoft.Extensions.Logging;
using JiebaNet.Segmenter;
using Newtonsoft.Json;
using System.Reflection;

namespace ZSN.AI.Plugins
{
    [Description("基础能力插件")]
    public class BasePlugin()
    {
        [KernelFunction]
        [Description("ZSN.AI.Plugins:获取当前日期与时间")]
        [return: Description("日期与时间")]
        public string get_date_time()
        {
            return DateTime.Now.ToDateTimeString();
        }

        [KernelFunction]
        [Description("ZSN.AI.Plugins:将日期转换为农历")]
        [return: Description("农历信息")]
        public string date_to_chinese_traditional_calendar([Description("日期,格式必须是yyyy-MM-dd")] string Date)
        {
            Date = Date == "" ? DateTime.Now.ToDateTimeString() : Date;
            ChineseTraditionalCalendarHelper cCalendar = new ChineseTraditionalCalendarHelper();
            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = DateTime.Parse(Date);
            }
            catch
            {

            }
            return cCalendar.GetDateTidyInfo(dateTime).Fullinfo;
        }

        [KernelFunction]
        [Description("ZSN.AI.Plugins:执行Agent")]
        [return: Description("NewTaskID")]
        public string excution_agent(
            [Description("AppID,不能为空")] string AppID,
            [Description("TaskID,不能为空")] string TaskID,
            [Description("FromMainTaskID,不能为空")] string FromMainTaskID,
            [Description("SessionID,不能为空")] string SessionID,
            [Description("ProcessesID,不能为空")] string ProcessesID,
            [Description("AgentNodeID,不能为空")] string AgentNodeID,
            [Description("提出的问题的内容,不能为空")] string Inputs
        )
        {
            string NewTaskID = "";
            if (!Inputs.IsNullOrEmpty() && !AppID.IsNullOrEmpty() && !TaskID.IsNullOrEmpty() && !SessionID.IsNullOrEmpty() && !ProcessesID.IsNullOrEmpty() && !AgentNodeID.IsNullOrEmpty())
            {
                WorkflowNodeInfo workflowNode = WorkflowNodeInfoBussiness.GetModel(AgentNodeID);
                if (workflowNode != null)
                {
                    NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(workflowNode.Config.ToString());
                    if (nodeConfig != null)
                    {
                        AgentData agentNodeData = JsonConvert.DeserializeObject<AgentData>(nodeConfig.data.ToString());
                        if (agentNodeData != null)
                        {
                            string AgentID = agentNodeData.agent.AgentID;
                            WorkflowNodeInfo node = WorkflowNodeInfoBussiness.GetAgentStartNode(agentNodeData.agent.AgentID);
                            if (node != null)
                            {
                                List<Output> outputs = new List<Output>();
                                List<string> Logs = new List<string>();

                                NodeConfig targetNode = new NodeConfig() { id = node.NodeID, mainid = AgentID, workflowid = node.WorkflowID, type = node.NodeType, data = node.Config };

                                //需要传值给StartNode(由于下一节点的输入参数需要跟上一节点的输出参数匹配，StartNode作为下一节点，输入参数名称是input)，所以Output.varname=input
                                outputs.Add(new Output() { varname = "input", value = Inputs });


                                NewTaskID = TaskInfoBussiness.toTask(nodeConfig, outputs, targetNode, AppID, SessionID, ProcessesID, TaskID, FromMainTaskID, AgentNodeID);

                            }
                        }
                    }
                }
            }

            return NewTaskID;
        }

        [KernelFunction]
        [Description("ZSN.AI.Plugins:执行workflow")]
        [return: Description("TaskID")]
        public string excution_workflow(
            [Description("AppID,不能为空")] string AppID,
            [Description("SessionID,不能为空")] string SessionID,
            [Description("ProcessesID,不能为空")] string ProcessesID,
            [Description("WorkFlowID,不能为空")] string WorkFlowID,
            [Description("输入的参数或者问题,不能为空")] string inputs
        )
        {

            string TaskID = "";
            WorkflowNodeInfo workflowNode = WorkflowNodeInfoBussiness.GetWorkFlowStartNode(WorkFlowID);
            if (workflowNode != null)
            {
                NodeConfig nodeConfig = JsonConvert.DeserializeObject<NodeConfig>(workflowNode.Config.ToString());
                if (nodeConfig != null)
                {
                    AgentData agentNodeData = JsonConvert.DeserializeObject<AgentData>(nodeConfig.data.ToString());
                    if (agentNodeData != null)
                    {
                        string AgentID = agentNodeData.agent.AgentID;
                        WorkflowNodeInfo node = WorkflowNodeInfoBussiness.GetAgentStartNode(agentNodeData.agent.AgentID);
                        if (node == null)
                        {
                            NodeConfig config = JsonConvert.DeserializeObject<NodeConfig>(node.Config.ToString());
                            List<Output> outputs = new List<Output>();
                            List<string> Logs = new List<string>();
                            NodeConfig targetNode = new NodeConfig();

                            if (config != null)
                            {
                                targetNode = config;
                            }

                            //需要传值给StartNode(由于下一节点的输入参数需要跟上一节点的输出参数匹配，StartNode作为下一节点，输入参数名称是input)，所以这个理的Output.varname=input
                            outputs.Add(new Output() { varname = "input", value = inputs });

                            TaskID = TaskInfoBussiness.toTask(null, outputs, targetNode, AppID, SessionID, ProcessesID, AgentID);

                        }
                    }
                }
            }

            return TaskID;
        }
    }
}

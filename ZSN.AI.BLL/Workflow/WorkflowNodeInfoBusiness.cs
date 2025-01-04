using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity;
using ZSN.AI.DAL;
using JiebaNet.Segmenter;
using Senparc.CO2NET.Extensions;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
namespace ZSN.AI.BLL
{
    public partial class WorkflowNodeInfoBussiness
    {
	    #region 基础信息
        private const string ConnectionName = "WorkflowDb";
        #endregion
        #region tb_workflow_node_info

        public static void Save(WorkflowNodeInfo model) {
            if (GetModel(model.NodeID) != null)
            {
                Update(model);
            }
            else
            {
                Add(model);
            }
        }
		/// <summary>
        /// 增加一条数据
        /// </summary>
		public static string Add(WorkflowNodeInfo model)
		{
			return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_Add(model);
		}
		/// <summary>
        /// 更新一条数据
        /// </summary>
		public static bool Update(WorkflowNodeInfo model)
		{
			return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_Update(model);
		}
        /// <summary>
        /// 删除一条数据
        /// </summary>
		public static bool Delete(string nodeID)
		{
			return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_Delete(nodeID);
		}
        /// <summary>
        /// 批量删除数据
        /// </summary>
		public static bool DeleteList(string nodeIDlist)
		{
			return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_DeleteList(nodeIDlist);
		}
        public static bool DeleteAbsentList(string nodeIDlist, string WorkflowID)
        {
            return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_DeleteAbsentList(nodeIDlist, WorkflowID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
		public static ZSN.AI.Entity.WorkflowNodeInfo GetModel(string nodeID)
		{
			return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetModel(nodeID);
		}
        public static ZSN.AI.Entity.WorkflowNodeInfo GetAppMainAINode(string AppID)
        {
            WorkflowInfo appWorkflow = WorkflowInfoBussiness.GetModelByAppID(AppID);
            if (appWorkflow != null)
            {
                string strWhere = " NodeType = '" + NodeType.MainAI.ToString() + "' and WorkflowID='" + appWorkflow.WorkflowID + "'";
                List<WorkflowNodeInfo> workflowNodes = GetList(1, strWhere, "");
                if (workflowNodes != null)
                {
                    return workflowNodes[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static ZSN.AI.Entity.WorkflowNodeInfo GetAppStartNode(string AppID)
        {
            WorkflowInfo appWorkflow = WorkflowInfoBussiness.GetModelByAppID(AppID);
            if (appWorkflow != null)
            {
                string strWhere = " NodeType = '" + NodeType.Start.ToString() + "' and WorkflowID='" + appWorkflow.WorkflowID + "'";
                List<WorkflowNodeInfo> workflowNodes = GetList(1, strWhere, "");
                if (workflowNodes != null)
                {
                    return workflowNodes[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static ZSN.AI.Entity.WorkflowNodeInfo GetAgentStartNode(string AgentID)
        {
            WorkflowInfo appWorkflow = WorkflowInfoBussiness.GetModelByAgentID(AgentID);
            if (appWorkflow != null)
            {
                string strWhere = " NodeType = '" + NodeType.AgentStart.ToString() + "' and WorkflowID='" + appWorkflow.WorkflowID + "'";
                List<WorkflowNodeInfo> workflowNodes = GetList(1, strWhere, "");
                if (workflowNodes != null && workflowNodes.Count>0)
                {
                    return workflowNodes[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static ZSN.AI.Entity.WorkflowNodeInfo GetWorkFlowStartNode(string WorkFlowID)
        {
            return GetWorkFlowNode(WorkFlowID, NodeType.Start);
        }
        public static ZSN.AI.Entity.WorkflowNodeInfo GetWorkFlowAgentStartNode(string WorkFlowID)
        {
            return GetWorkFlowNode(WorkFlowID, NodeType.AgentStart);
        }
        public static ZSN.AI.Entity.WorkflowNodeInfo GetWorkFlowNode(string WorkFlowID, NodeType nodeType)
        {

            string strWhere = " NodeType = '" + nodeType.ToString() + "' and WorkflowID='" + WorkFlowID + "'";
            List<WorkflowNodeInfo> workflowNodes = GetList(1, strWhere, "");
            if (workflowNodes != null)
            {
                return workflowNodes[0];
            }
            else
            {
                return null;
            }

        }
        public static ZSN.AI.Entity.WorkflowNodeInfo GetAppReporterNode(string AppID)
        {
            WorkflowInfo appWorkflow = WorkflowInfoBussiness.GetModelByAppID(AppID);
            if (appWorkflow != null)
            {
                string strWhere = " NodeType = '" + NodeType.Reporter.ToString() + "' and WorkflowID='" + appWorkflow.WorkflowID + "'";
                List<WorkflowNodeInfo> workflowNodes = GetList(1, strWhere, "");
                if (workflowNodes != null)
                {
                    return workflowNodes[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static List<WorkflowNodeInfo> GetListByNodeID(string NodeID = "")
        {
            string strWhere = " NodeID in("+ NodeID + ")";
            return WorkflowNodeInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetList(strWhere).Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public static List<WorkflowNodeInfo> GetList(string strWhere = "")
        {
            return WorkflowNodeInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetList(strWhere).Tables[0]);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
		public static List<WorkflowNodeInfo> GetList(int top, string strWhere, string filedOrder)
        {
            return WorkflowNodeInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetList(top, strWhere, filedOrder).Tables[0]);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
		public static int GetRecordCount(string strWhere = "")
        {
            return DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
		public static List<WorkflowNodeInfo> GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex)
        {
            return WorkflowNodeInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetListByPage(strWhere, orderBy, startIndex, endIndex).Tables[0]);
        }
		/// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页标</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pagetotal">总页数</param>
        /// <param name="total">总数</param>
        /// <param name="orderType">排序规则， 默认降序，1降序，0升序</param>
        /// <param name="showName">显示字段，默认全部</param>
        /// <param name="orderKey">排序key，默认主键</param>
        /// <returns></returns>
		public static List<WorkflowNodeInfo> GetListByPage(int pageSize, int pageIndex, string strWhere, out int pagetotal, out int total, int orderType = 1, string showName = "*", string orderKey = "NodeID")
		{
            return WorkflowNodeInfoDataSet_ToList(DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_GetListByPage(pageSize, pageIndex, strWhere, out pagetotal, out total, orderType, showName, orderKey));
        }
		private static List<WorkflowNodeInfo> WorkflowNodeInfoDataSet_ToList(DataTable dt)
		{
			var rows = dt.Rows;
            var list = new List<WorkflowNodeInfo>();
            foreach (DataRow r in rows)
            {
                list.Add(DatabaseProvider.GetWorkflowNodeInfo(ConnectionName).WorkflowNodeInfo_DataRowToModel(r));
            }
            return list;
		}
        #endregion

        /// <summary>
        /// 下一节点任务
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="SessionID"></param>
        /// <param name="SourceNode"></param>
        /// <param name="outputs"></param>
        /// <param name="Logs"></param>
        /// <param name="AgentID">为Agent工作流时启用</param>
        public static string NextNode(string AppID, string SessionID, string ProcessesID,string FromTaskID,string FromMainTaskID, string AgentNodeID, NodeConfig SourceNode, List<Output> outputs, List<string> Logs)
        {

            //更新上节点状态
            TaskInfoBussiness.updateTask(FromTaskID, TaskState.Completed, new Results()
            {
                Data = outputs
            });

            string newTaskID = "";
            List<WorkflowEdgeInfo> edgeList = WorkflowEdgeInfoBussiness.GetListBySourceNodeId(SourceNode.id);
            if (edgeList != null && edgeList.Count > 0)
            {
                List<WorkflowNodeInfo> targetNodeList = WorkflowNodeInfoBussiness.GetListByNodeID(string.Join(",", edgeList.Select(x => $"'{x.TargetNodeId}'")));
                if (targetNodeList != null)
                {
                    foreach (WorkflowNodeInfo node in targetNodeList)
                    {
                        //Agent任务只能通过主AI自主调用
                        if (!AgentNodeID.IsNullOrEmpty() || node.NodeType != NodeType.Agent)
                        {
                            NodeConfig targetNode = new NodeConfig();
                            targetNode.id = node.NodeID;
                            targetNode.mainid = SourceNode.mainid;
                            targetNode.workflowid = node.WorkflowID;
                            targetNode.type = node.NodeType;
                            targetNode.data = node.Config;
                            targetNode.name = node.NodeType.ToString();// node.NodeName;
                            targetNode.fromNodeType = SourceNode.type;

                            newTaskID = TaskInfoBussiness.toTask(SourceNode, outputs, targetNode, AppID, SessionID, ProcessesID, FromTaskID, FromMainTaskID, AgentNodeID);

                            Logs.Add($"{newTaskID}");
                        }
                    }
                }
            }
            return newTaskID;
        }

        /// <summary>
        /// Agent流程结束后的处理,回调AgentNode
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="SessionID"></param>
        /// <param name="ProcessesID"></param>
        /// <param name="FromTaskID"></param>
        /// <param name="FromMainTaskID"></param>
        /// <param name="AgentNodeID"></param>
        /// <param name="SourceNode"></param>
        /// <param name="outputs"></param>
        /// <param name="Logs"></param>
        /// <returns></returns>
        public static string AgentEndToNextNode(string AppID, string SessionID, string ProcessesID, string FromTaskID, string FromMainTaskID, string AgentNodeID, NodeConfig SourceNode, List<Output> outputs, List<string> Logs)
        {

            //更新上节点状态
            TaskInfoBussiness.updateTask(FromTaskID, TaskState.Completed, new Results()
            {
                Data = outputs
            });

            string newTaskID = Guid.NewGuid().ToString();

            NodeConfig targetNode = new NodeConfig();
            targetNode.id = AgentNodeID;
            targetNode.mainid = SourceNode.mainid;
            targetNode.workflowid = SourceNode.workflowid;
            targetNode.type = SourceNode.type;
            targetNode.data = SourceNode.data;
            targetNode.name = SourceNode.type.ToString();
            targetNode.fromNodeType = NodeType.AgentEnd;

            List<Inputs> inputs = outputs.Select(output => new Inputs
            {
                id = Guid.NewGuid().ToString(), // 如果需要生成唯一 id
                sourceId = output.id,
                varname = output.varname, // 如果想保留原 varname，可以赋值，否则用默认值
                value = output.value,
                type = output.type,
                txt = output.txt
            }).ToList();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskID = newTaskID;
            taskInfo.TaskType = SourceNode.type;
            taskInfo.TaskConfig = new TaskConfig();
            taskInfo.TaskConfig.NodeConfig = targetNode;
            taskInfo.TaskConfig.Data = new TaskData() { AppID = AppID, SessionID = SessionID, TaskID = newTaskID, FromMainTaskID = FromMainTaskID, ProcessesID = ProcessesID, AgentNodeID = AgentNodeID, Inputs = inputs };
            taskInfo.LoopType = LoopType.NOLoop;
            taskInfo.RepeatValue = 1;
            taskInfo.RedoCount = 0;
            taskInfo.CreateTime = DateTime.Now;
            taskInfo.UpdateTime = DateTime.Now;
            taskInfo.FromTaskID = FromTaskID;
            taskInfo.FromMainTaskID = FromMainTaskID;

            TaskInfoBussiness.Add(taskInfo);

            Logs.Add($"{newTaskID}");


            return newTaskID;
        }
    }
}

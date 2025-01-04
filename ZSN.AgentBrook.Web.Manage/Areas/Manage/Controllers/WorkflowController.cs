using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.Utils.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

using ZSN.Utils.Core.Helpers;
using ZSN.AgentBrook.Web.Manage.Attributes;
using Newtonsoft.Json;
using ZSN.AI.Service.Controllers;
using ZSN.AI.Entity.Workflow;
using log4net.Plugin;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using DocumentFormat.OpenXml.Drawing.Charts;
using Node = ZSN.AI.Node.Utils;


namespace ZSN.AgentBrook.Web.Manage.Areas.Manage.Controllers
{
    
    public class WorkflowController : AdminBaseController
    {
        [AdminAttributes]
        public IActionResult index(int type = 1, string mid = "", int index = 1, int size = 10)
        {
            string _where = " MainType="+type+ " and MainID='"+mid+"'";

            var lst = WorkflowInfoBussiness.GetListByPage(size, index, _where, out int pagetotal, out int total);
            ViewBag.Index = index;
            ViewBag.Size = size;
            ViewBag.Total = total;
            ViewBag.AppList = lst;
            ViewBag.MainType = type;
            ViewBag.MainID = mid;
            return View();
        }

        
        [HttpGet]
        [AdminAttributes(CheckLogin = false, CheckUrl = false)]
        public JsonMsg<BaseConfig> getBaseConfig()
        {
            BaseConfig baseConfig = new BaseConfig();
            baseConfig.largeModelList = LargeModelInfoBussiness.GetList(" SystemStatus=0 ");
            baseConfig.knowledgeBaseList = KnowledgeBaseInfoBussiness.GetList(" SystemStatus=0 ");
            baseConfig.pluginsList = PluginsInfoBussiness.GetList(" SystemStatus=0 ");
            baseConfig.agentList = AgentInfoBussiness.GetList(" SystemStatus=1");

            return JsonMsg<BaseConfig>.OK(baseConfig);
        }

        
        [HttpPost]
        [AdminAttributes(CheckLogin = false, CheckUrl = false)]
        public JsonMsg<WorkFlow> getWorkFlow(string WorkflowID,string MainID,int MainType=2)
        {
            WorkFlow workflow = new WorkFlow();
            workflow.Info = WorkflowInfoBussiness.GetModel(WorkflowID);
            if (workflow.Info != null)
            {
                workflow.WorkflowID = workflow.Info.WorkflowID;
                workflow.MainID = workflow.Info.MainID;
                workflow.MainType = workflow.Info.MainType;

                if (workflow.Info.SystemStatus != -1)
                {
                    workflow.Nodes = WorkflowNodeInfoBussiness.GetList(" WorkflowID='" + WorkflowID + "' ");
                    workflow.Edges = WorkflowEdgeInfoBussiness.GetList(" WorkflowID='" + WorkflowID + "' ");

                    return JsonMsg<WorkFlow>.OK(workflow);
                }
                else
                {
                    return JsonMsg<WorkFlow>.Error(null, ErrorCode.Locked);
                }
            }
            else
            {
                if (!MainID.IsNullOrEmpty())
                {
                    workflow = Node.initWorkFlow(MainID, MainType);

                    return JsonMsg<WorkFlow>.OK(workflow);
                }
                else
                {
                    return JsonMsg<WorkFlow>.Error(null, ErrorCode.DataEmpty);
                }
                
            }
        }

        [HttpPost]
        [AdminAttributes(CheckLogin = false, CheckUrl = false)]
        public JsonMsg<WorkflowNodeInfo> addNode(string WorkflowID,string NodeType,string MainID) {

            WorkflowNodeInfo nodeInfo = Node.newNode(WorkflowID, (NodeType)Enum.Parse(typeof(NodeType), NodeType), MainID);

            return JsonMsg<WorkflowNodeInfo>.OK(nodeInfo);
        }

        public IActionResult Edit(string id = "",string MainID="",int MainType=0)
        {
            AppInfo appinfo = null;
            AgentInfo agentInfo = null;

            string MainName = "";
            WorkflowInfo Workflow = new WorkflowInfo();
            
            if(MainType == 1)
            {
                appinfo = AppInfoBussiness.GetModel(MainID);
                MainName = appinfo.Name;
                id = appinfo.WorkFlowID;
            }
            else
            {
                agentInfo = AgentInfoBussiness.GetModel(MainID);
                MainName = agentInfo.Name;
            }

            if (!id.IsNullOrEmpty())
            {
                Workflow = WorkflowInfoBussiness.GetModel(id);
            }
            else
            {
                id = Workflow.WorkflowID;
            }

            ViewBag.Workflow = Workflow;
            ViewBag.MainName = MainName;
            ViewBag.MainID = MainID;
            ViewBag.MainType = MainType;

            ViewBag.PreviewHost = ConfigHelper.GetString("previewHost");

            return View();
        }

        [HttpPost]
        public JsonMsg<string> Save([FromBody] WorkFlow workFlow)
        {
            if (workFlow != null)
            {
                string WorkFlowID = WorkflowInfoBussiness.Save(workFlow);

                return JsonMsg<string>.OK(WorkFlowID);
            }
            else
            {
                return JsonMsg<string>.Error(null,ErrorCode.DataEmpty);
            }
            
        }

        [HttpPost]
        public JsonMsg<string> Status(string mid, bool status)
        {
            var Workflow = WorkflowInfoBussiness.GetModel(mid);
            Workflow.SystemStatus = status ? 0 : 1;

            WorkflowInfoBussiness.Update(Workflow);
            return JsonMsg<string>.OK("更新成功");
        }

        [HttpPost]
        public JsonMsg<ChatLog> getChatLog(string ChatSessionID)
        {
            ChatLog chatLog = new ChatLog();
            chatLog.Log = AppChatLogInfoBussiness.GetList(" ChatSessionID='"+ ChatSessionID + "'");

            return JsonMsg<ChatLog>.OK(chatLog);
        }
        public JsonMsg<string> Del(string mid)
        {
            WorkflowInfoBussiness.DeleteList(mid);

            return JsonMsg<string>.OK("删除成功");
        }
    }
}

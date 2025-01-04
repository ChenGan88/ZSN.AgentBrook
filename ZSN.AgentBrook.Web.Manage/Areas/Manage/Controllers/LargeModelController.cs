using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.Utils.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

using ZSN.Utils.Core.Helpers;
using ZSN.AgentBrook.Web.Manage.Attributes;
using ZSN.AI.Service.Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using ZSN.AI.Core.Interface;
using ZSN.AI.Core.Service;
using System.Text;
using Elastic.Clients.Elasticsearch;
using Markdig;
using ZSN.AI.Core.Repositories;
using ZSN.AI.Core.Utils;
using ZSN.AI.Entity.Model.Enum;

namespace ZSN.AgentBrook.Web.Manage.Areas.Manage.Controllers
{
    [AdminAttributes]
    public class LargeModelController: AdminBaseController
    {

        private readonly IChatService _chatService;
        private readonly IKernelService _kernelService;
        private readonly IKMService _kMService;

        public LargeModelController(IChatService chatService, IKernelService kernelService, IKMService kMService) {
            _chatService = chatService;
            _kernelService = kernelService;
            _kMService = kMService;
        }

        public IActionResult index(int index = 1, int size = 10)
        {
            var lst = LargeModelInfoBussiness.GetListByPage(size, index, "", out int pagetotal, out int total);
            ViewBag.Index = index;
            ViewBag.Size = size;
            ViewBag.Total = total;
            ViewBag.LargeModelList = lst;
            return View();
        }

        [HttpPost]
        public JsonMsg<string> LargeModelStatus(int mid, bool status)
        {
            var LargeModel = LargeModelInfoBussiness.GetModel(mid);
            LargeModel.SystemStatus = status ? 0 : 1;

            LargeModelInfoBussiness.Update(LargeModel);
            return JsonMsg<string>.OK("更新成功");
        }

        public IActionResult Edit(int mid = 0)
        {
            var LargeModel = mid == 0 ? new LargeModelInfo() : LargeModelInfoBussiness.GetModel(mid);
            ViewBag.LargeModel = LargeModel;

            ViewBag.ModelTypeList = (new AIModelTypeList()).List();// BaseDictionaryInfoBussiness.GetChildList("模型类型");
            ViewBag.ModelOrganizationList = (new AIOrganizationList()).List();
            
            ViewBag.SemanticFunction = PluginsInfoBussiness.GetList(" PluginsType = 1 and SystemStatus=0");
            ViewBag.NativeFunction = PluginsInfoBussiness.GetList(" PluginsType = 2 and SystemStatus=0");
            ViewBag.PreviewHost = ConfigHelper.GetString("previewHost");

            return View();
        }
        [HttpPost]
        public JsonMsg<string> LargeModelSave(LargeModelInfo LargeModel)
        {
            if (LargeModel.LargeModelID<=0)
            {
                LargeModel.CreateTime = DateTime.Now;
                LargeModelInfoBussiness.Add(LargeModel);
            }
            else
            {

                LargeModelInfoBussiness.Update(LargeModel);
            }
            return JsonMsg<string>.OK("保存成功");
        }

        public JsonMsg<string> LargeModelDel(string mid)
        {
            LargeModelInfoBussiness.DeleteList(mid);

            return JsonMsg<string>.OK("删除成功");
        }

        [HttpPost]
        public async Task<JsonMsg<List<Chats>>> Test(LargeModelInfo LargeModel)
        {
            LargeModelUnit ModelUnit = new LargeModelUnit();
            ChatHistory history = new ChatHistory();
            List<Chats> MessageList = [];
            Chats info = null;

            string _testStr = "这是一个测试，来确定大模型接口是否联通！";

            history.AddUserMessage(_testStr);

            ModelUnit = ModelUnit.ModelMap(LargeModel.TypeCode, LargeModel);
            IAsyncEnumerable<string> chatResult = null;
            string _res = "";
            LargeModelConfig modelConfig = new LargeModelConfig();

            modelConfig.Model = LargeModel;

            StringBuilder rawContent = new StringBuilder();

            switch (LargeModel.TypeCode)
            {
                case AIModelType.Chat:
                    
                    chatResult = _chatService.SendChatAsync(modelConfig, history);
                    await foreach (var content in chatResult)
                    {
                        if (info == null)
                        {
                            rawContent.Append(content.ConvertToString());
                            info = new Chats();
                            info.Id = Guid.NewGuid().ToString();
                            info.UserName = "_userName";
                            info.AppId = "Test";
                            info.Context = content.ConvertToString();
                            info.CreateTime = DateTime.Now;

                            MessageList.Add(info);
                        }
                        else
                        {
                            rawContent.Append(content.ConvertToString());
                        }
                        info.Context = Markdown.ToHtml(rawContent.ToString());
                    }

                    break;
                case AIModelType.Embedding:
                    chatResult = _kMService.TestEmbeddingAsync(modelConfig, _testStr);

                    await foreach (var content in chatResult)
                    {
                        if (info == null)
                        {
                            rawContent.Append(content.ConvertToString());
                            info = new Chats();
                            info.Id = Guid.NewGuid().ToString();
                            info.UserName = "_userName";
                            info.AppId = "Test";
                            info.Context = content.ConvertToString();
                            info.CreateTime = DateTime.Now;

                            MessageList.Add(info);
                        }
                        else
                        {
                            rawContent.Append(content.ConvertToString());
                        }
                        info.Context = Markdown.ToHtml(rawContent.ToString());
                    }

                    break;
                case AIModelType.Rerank:
                    //ModelUnit.RerankModel.Model = LargeModel;
                    break;
            }
            
            
            

            return JsonMsg<List<Chats>>.OK(MessageList);
        }
    }
}

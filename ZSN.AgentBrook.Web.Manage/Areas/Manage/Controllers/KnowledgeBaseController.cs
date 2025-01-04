using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.Utils.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

using ZSN.Utils.Core.Helpers;
using ZSN.AgentBrook.Web.Manage.Attributes;
using ZSN.AI.Service.Controllers;
using ZSN.AI.Entity.Model.Enum;
using Elastic.Clients.Elasticsearch.Cluster;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI;
using System.IO;
using MongoDB.Bson.IO;
using System.Text.Json.Serialization;
using ZSN.AI.Entity.Model;

namespace ZSN.AgentBrook.Web.Manage.Areas.Manage.Controllers
{
    [AdminAttributes]
    public class KnowledgeBaseController: AdminBaseController
    {
        public IActionResult index(int index = 1, int size = 10)
        {
            var lst = KnowledgeBaseInfoBussiness.GetListByPage(size, index, "", out int pagetotal, out int total);
            ViewBag.Index = index;
            ViewBag.Size = size;
            ViewBag.Total = total;
            ViewBag.KnowledgeBaseList = lst;
            return View();
        }

        public IActionResult filelist(string KnowledgeBaseID,int index = 1, int size = 10)
        {
            string where = " KnowledgeBaseID='"+ KnowledgeBaseID.SecureSQL()+"'";
            var lst = KnowledgeBaseFileInfoBussiness.GetListByPage(size, index, where, out int pagetotal, out int total);
            ViewBag.Index = index;
            ViewBag.Size = size;
            ViewBag.Total = total;
            ViewBag.KnowledgeBaseID = KnowledgeBaseID;
            ViewBag.KnowledgeBaseFileList = lst;
            return View();
        }

        [HttpPost]
        public JsonMsg<string> KnowledgeBaseStatus(string mid, bool status)
        {
            var KnowledgeBase = KnowledgeBaseInfoBussiness.GetModel(mid);
            KnowledgeBase.SystemStatus = status ? 0 : 1;

            KnowledgeBaseInfoBussiness.Update(KnowledgeBase);
            return JsonMsg<string>.OK("更新成功");
        }

        public IActionResult Edit(string mid = "")
        {
            var KnowledgeBase = mid == "" ? new KnowledgeBaseInfo() : KnowledgeBaseInfoBussiness.GetModel(mid);
            var ModelList = LargeModelInfoBussiness.GetList(" SystemStatus = 0 ");
            ViewBag.KnowledgeBase = KnowledgeBase;

            ViewBag.KnowledgeBaseTypeList = BaseDictionaryInfoBussiness.GetChildList("知识库类型");
            ViewBag.PreprocessModeList = ModelList.FindAll(x=>x.TypeCode == AIModelType.Chat);
            ViewBag.VectorModelList = ModelList.FindAll(x => x.TypeCode == AIModelType.Embedding);

            ViewBag.PreviewHost = ConfigHelper.GetString("previewHost");
            return View();
        }
        [HttpPost]
        public JsonMsg<string> KnowledgeBaseSave(KnowledgeBaseInfo KnowledgeBase)
        {
            KnowledgeBase.MemberID = KnowledgeBase.MemberID.IsNullOrEmpty() ? KnowledgeBase.MemberID : "";
            if (KnowledgeBase.KnowledgeBaseID.IsNullOrEmpty())
            {
                KnowledgeBase.KnowledgeBaseID = hashEncrypt.MD5System(Guid.NewGuid().ToString());
                KnowledgeBase.CreateTime = DateTime.Now;
                KnowledgeBaseInfoBussiness.Add(KnowledgeBase);
            }
            else
            {

                KnowledgeBaseInfoBussiness.Update(KnowledgeBase);
            }
            return JsonMsg<string>.OK("保存成功");
        }

        public JsonMsg<string> KnowledgeBaseDel(string mid)
        {
            KnowledgeBaseInfoBussiness.DeleteList(mid);

            return JsonMsg<string>.OK("删除成功");
        }

        public IActionResult EditFile(string KnowledgeBaseID,string FileID = "", int index = 1, int size = 10) {
            FileID = FileID.SecureSQL();
            if (!FileID.IsNullOrEmpty())
            {
                string where = " id like 'd="+ FileID+"%'";
                var KnowledgeBaseFileChunkList = KnowledgeBaseFileChunkInfoBussiness.GetListByPage(KnowledgeBaseID,size, index, where, out int pagetotal, out int total);

                ViewBag.Index = index;
                ViewBag.Size = size;
                ViewBag.Total = total;
                ViewBag.FileID = FileID;
                ViewBag.KnowledgeBaseID = KnowledgeBaseID;
                ViewBag.KnowledgeBaseFileList = KnowledgeBaseFileChunkList;
                return View();
            }
            else
            {
                ViewBag.FileID = FileID;
                ViewBag.KnowledgeBaseID = KnowledgeBaseID;
                return View();
            }
        }
        public JsonMsg<string> AddFile(string fileCode,string fileName,string KnowledgeBaseID) {
            if (!fileCode.IsNullOrEmpty() && !fileName.IsNullOrEmpty() && !KnowledgeBaseID.IsNullOrEmpty())
            {
                KnowledgeBaseFileInfo knowledgeBaseFileInfo = KnowledgeBaseFileInfoBussiness.GetModel(fileCode);
                FilesInfo fileInfo = FilesInfoBussiness.GetModel(fileCode);
                if (fileInfo != null && knowledgeBaseFileInfo==null)
                {
                    KnowledgeBaseFileInfo knowledgeBaseFile = new KnowledgeBaseFileInfo();
                    knowledgeBaseFile.FileID = fileCode;
                    knowledgeBaseFile.KnowledgeBaseID = KnowledgeBaseID;
                    knowledgeBaseFile.FileName = fileInfo.FOriginName;
                    knowledgeBaseFile.FilePath = fileInfo.FFilePath+ fileInfo.FName;
                    knowledgeBaseFile.Type = fileInfo.FType;
                    knowledgeBaseFile.ParserConfig =  "{}";
                    knowledgeBaseFile.CreateTime = DateTime.Now;
                    knowledgeBaseFile.SystemStatus = 0;

                    KnowledgeBaseFileInfoBussiness.Add(knowledgeBaseFile);
                }
                return JsonMsg<string>.OK("成功");
            }
            else {
                return JsonMsg<string>.Error("参数错误",ErrorCode.DataEmpty);
            }
        }
        public JsonMsg<string> KnowledgeBaseFileDel(string mid, string KnowledgeBaseID)
        {
            KnowledgeBaseFileInfoBussiness.DeleteList(mid);
            //删除原分块数据
            KnowledgeBaseFileChunkInfoBussiness.Delete(mid, KnowledgeBaseID);

            return JsonMsg<string>.OK("删除成功");
        }
        public JsonMsg<string> KnowledgeBaseFileToJob(string KnowledgeBaseID, string FileID)
        {
            if (!KnowledgeBaseID.IsNullOrEmpty() && !FileID.IsNullOrEmpty())
            {
                //删除原分块数据
                KnowledgeBaseFileChunkInfoBussiness.Delete(FileID, KnowledgeBaseID);

                KnowledgeBaseFileInfo knowledgeBaseFile = KnowledgeBaseFileInfoBussiness.GetModel(FileID);

                ImportKMSTaskReq importKMSTask = new ImportKMSTaskReq();
                importKMSTask.KmsId = KnowledgeBaseID;
                importKMSTask.IsQA = false;
                importKMSTask.ImportType = ImportType.File;
                importKMSTask.FileName = knowledgeBaseFile.FileName;
                importKMSTask.FilePath = knowledgeBaseFile.FilePath;
                importKMSTask.KnowledgeBaseFile = knowledgeBaseFile;

                TaskInfo taskInfo = new TaskInfo();
                taskInfo.TaskType = NodeType.NotNode_FileChunk;
                taskInfo.TaskConfig = new TaskConfig();
                taskInfo.TaskConfig.NotNodeConfig = new FileChunkConfig() { KnowledgeBaseID = KnowledgeBaseID, FileID = FileID, ImportKMSTask = importKMSTask };
                taskInfo.TaskConfig.Data = new TaskData() { };
                taskInfo.LoopType = LoopType.NOLoop;
                taskInfo.RepeatValue = 1;
                taskInfo.RedoCount = 0;
                taskInfo.CreateTime = DateTime.Now;
                taskInfo.UpdateTime = DateTime.Now;

                TaskInfoBussiness.Add(taskInfo);

                return JsonMsg<string>.OK("分块任务添加成功");
            }
            else
            {
                return JsonMsg<string>.Error("参数错误", ErrorCode.DataEmpty);
            }
        }
    }
}


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

namespace ZSN.AI.LLMServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AppController : ApiBaseController
    {
        public AppController() { 
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
        public JsonMsg<List<AppInfo>> GetList([FromBody] PostData paramValue)
        {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                List<AppInfo> _list = AppInfoBussiness.GetList();

                return JsonMsg<List<AppInfo>>.OK(_list);
            }
            else
            {
                return JsonMsg<List<AppInfo>>.Error(null, ErrorCode.DataFormatError);
            }
        }
        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("application/json")]
        [APIChecker(Token = false)]
        public JsonMsg<AppInfo> Get([FromBody] PostData paramValue) {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                string AppID = this.setting.AppID;
                AppInfo _app = AppInfoBussiness.GetModel(AppID);

                if (_app.AICON.IsNullOrEmpty() || _app.AICON == "#")
                {
                    _app.AICONList.Add(ConfigHelper.GetString("previewHost"));
                }
                else
                {
                    _app.AICONList = FormatFileCode(_app.AICON);
                }
                return JsonMsg<AppInfo>.OK(_app);
            }
            else
            {
                return JsonMsg<AppInfo>.Error(null, ErrorCode.DataFormatError);
            }
        }
    }
}

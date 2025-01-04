
using ZSN.AI.LLMServer.Controllers;
using ZSN.Utils.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZSN.AI.BLL;
using ZSN.AI.Entity;
using System.Net;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;
using Microsoft.SemanticKernel.TextGeneration;

using ZSN.Utils.Core.Extensions;
using ZSN.AI.Service.Attributes;
using ZSN.AI.LLMServer.Attributes;
using ZSN.AI.Service.Controllers;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ZSN.AI.LLMServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LargeModelController: ApiBaseController
    {
        public LargeModelController() { 
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
        public JsonMsg<List<LargeModelInfo>> GetLargeModelList([FromBody] PostData paramValue)
        {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                List<LargeModelInfo> _list = LargeModelInfoBussiness.GetList();

                return JsonMsg<List<LargeModelInfo>>.OK(_list);
            }
            else
            {
                return JsonMsg<List<LargeModelInfo>>.Error(null, ErrorCode.DataFormatError);
            }
        }
    }
}


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
    public class AgentController : ApiBaseController
    {
        public AgentController() { 
        }

        [HiddenApi]
        [HttpGet]
        public IActionResult Index()
        {
            return BuildSuccessResult(new { msg = "success" });
        }

        [ApiExplorerSettings(GroupName = "V1-Public")]
        [HttpPost]
        [Consumes("Agentlication/json")]
        [APIChecker(Token = false)]
        public JsonMsg<List<AgentInfo>> GetList([FromBody] PostData paramValue)
        {
            JObject jObject = this.JsonObj;
            if (jObject.JsonGetValue<int>("status") != -1)
            {
                List<AgentInfo> _list = AgentInfoBussiness.GetList();

                return JsonMsg<List<AgentInfo>>.OK(_list);
            }
            else
            {
                return JsonMsg<List<AgentInfo>>.Error(null, ErrorCode.DataFormatError);
            }
        }

    }
}

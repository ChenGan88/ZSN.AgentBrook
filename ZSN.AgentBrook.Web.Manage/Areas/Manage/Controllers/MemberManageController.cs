
using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.Utils.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

using ZSN.Utils.Core.Helpers;
using ZSN.AgentBrook.Web.Manage.Attributes;
using ZSN.AI.Service.Controllers;

namespace ZSN.AgentBrook.Web.Manage.Areas.Manage.Controllers
{
    [AdminAttributes]
    public class MemberManageController: AdminBaseController
    {
        public IActionResult index(int index = 1, int size = 10)
        {
            var lst = MemberInfoBussiness.GetListByPage(size, index, "", out int pagetotal, out int total);
            ViewBag.Index = index;
            ViewBag.Size = size;
            ViewBag.Total = total;
            ViewBag.MemberList = lst;
            return View();
        }

        [HttpPost]
        public JsonMsg<string> MemberStatus(string mid, bool status)
        {
            var member = MemberInfoBussiness.GetModel(mid);
            member.MState = status ? 0 : 1;

            MemberInfoBussiness.Update(member);
            return JsonMsg<string>.OK("更新成功");
        }

        public IActionResult MemberEdit(string mid = "")
        {
            var member = mid == "" ? new MemberInfo() : MemberInfoBussiness.GetModel(mid);
            ViewBag.Member = member;
            ViewBag.PreviewHost = ConfigHelper.GetString("previewHost");
            return View();
        }
        [HttpPost]
        public JsonMsg<string> MemberSave(MemberInfo member)
        {
            if (member.MemberID.IsNullOrEmpty())
            {
                member.MemberID = hashEncrypt.MD5System(Guid.NewGuid().ToString());
                member.MAppendTime = DateTime.Now;
                member.MPWD = hashEncrypt.MD5System(hashEncrypt.MD5System("12345678"));
                MemberInfoBussiness.Add(member);
            }
            else
            {
                //MemberInfo _member = MemberInfoBussiness.GetModel(member.MemberID);

                //member.MPWD = _member.MPWD;

                MemberInfoBussiness.Update(member);
            }
            return JsonMsg<string>.OK("保存成功");
        }

        public JsonMsg<string> MemberDel(string mid)
        {
            MemberInfoBussiness.DeleteList(mid);

            return JsonMsg<string>.OK("删除成功");
        }
    }
}

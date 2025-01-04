using System;
using System.Linq;
using ZSN.AI.BLL;
using ZSN.AI.Entity;
using ZSN.Utils.Core.Extensions;
using ZSN.Utils.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ZSN.AI.LLMServer.Helpers
{
    public class SettingsService
    {
        public static ApisettingsInfo Current => GetSetting();
        public static ApisettingsInfo GetSetting()
        {
            ApisettingsInfo setting = GetSessionSetting();// HttpContextHelper.Current.Session.Get<APISettings>("CurrentSetting");
            if (setting != null)
                return setting;
            
            var pms = HttpContextHelper.Session.GetString("BodyParams");

            if (!pms.IsNullOrEmpty())
            {
                try
                {
                    var obj = (JObject)JsonConvert.DeserializeObject(pms);
                    if (obj.ContainsKey("AppID"))
                    {
                        var appID = obj["AppID"].ToString();
                        setting = ApisettingsInfoBussiness.GetModelByAppID(appID);
                        
                        if (setting != null)
                        {
                            //ConsoleLogHelper.WriteLine(setting.SettingName);
                            SetSetting(setting);
                            return setting;
                        }
                        
                    }
                }
                catch (Exception ex)
                {

                }

            }
            
            return setting;
        }

        public static MemberSettings GetMemberSetting(string MemberID,int MemberOtherAuthID, string AccessToken,bool cache = true)
        {
            if (!MemberID.IsNullOrEmpty())
            {
                MemberSettings setting = GetSessionMemberSetting();// HttpContextHelper.Current.Session.Get<MemberSettings>("MemberSetting");
                if (setting != null && cache)
                    return setting;

                try
                {
                    setting = new MemberSettings();
                    setting.FullMember = new FullMemberInfo();
                    setting.FullMember.Member = MemberInfoBussiness.GetModel(MemberID);
                    setting.MemberOtherAuth = MemberOtherAuthInfoBussiness.GetModel(MemberOtherAuthID);
                    setting.MemberAuth = MemberAuthInfoBussiness.GetModelByAccessToken(AccessToken);

                    if (setting.FullMember.Member != null && setting.MemberAuth != null)
                    {
                        if (setting.FullMember.Member.MIcon.IsNullOrEmpty() == false)
                        {
                            setting.FullMember.Member.MIcon = string.Format(ConfigHelper.GetString("previewHost"), setting.FullMember.Member.MIcon);
                        }

                        SetMemberSetting(setting);
                        return setting;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {

                }


                return setting;
            }
            else
            {
                return null;
            }
        }

        public static void SetSetting(ApisettingsInfo setting)
        {
            HttpContextHelper.Current.Session.Set<ApisettingsInfo>("CurrentSetting", setting);
        }

        public static ApisettingsInfo GetSessionSetting() {
            return HttpContextHelper.Current.Session.Get<ApisettingsInfo>("CurrentSetting");
        }

        public static void SetMemberSetting(MemberSettings setting)
        {
            HttpContextHelper.Current.Session.Set<MemberSettings>("MemberSetting", setting);
        }

        public static MemberSettings GetSessionMemberSetting()
        {
            return HttpContextHelper.Current.Session.Get<MemberSettings>("MemberSetting");
        }

        public static void ClearSetting()
        {
            HttpContextHelper.Current.Session.Remove("CurrentSetting");
            //HttpContextHelper.Current.Session.Remove("BodyParams");
        }

        public static void ClearMemberSetting()
        {
            HttpContextHelper.Current.Session.Remove("MemberSetting");
        }


    }
}

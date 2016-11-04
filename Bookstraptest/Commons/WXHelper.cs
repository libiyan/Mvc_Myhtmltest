using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ucsmy.Framework.Common;

namespace Bookstraptest.Commons
{
    public static class WXHelper
    {
        //private static string applyid = ConfigurationSettings.AppSettings["WX_ApplyId"];
        //private static string secret = ConfigurationSettings.AppSettings["WX_AppSecret"];
        //private static string apiTokenUrl = ConfigurationSettings.AppSettings["WX_ApiTokenUrl"];
        //private static string getticketUrl = ConfigurationSettings["WX_ApiTicketUrl"];
        private static string applyid = "WX_ApplyId".ValueOfAppSettings();
        private static string secret = "WX_AppSecret".ValueOfAppSettings();
        private static string apiTokenUrl = "WX_ApiTokenUrl".ValueOfAppSettings();
        private static string getticketUrl = "WX_ApiTicketUrl".ValueOfAppSettings();
        private static Access_Token GetAccess_Token()
        {
            var data = "grant_type=client_credential&appid=" + applyid + "&secret=" + secret;
            var response = HtmlHelper.SendGet(data, apiTokenUrl);
            return JsonHelper.ToObject<Access_Token>(response);
        }

        private static Jsapi_Ticket GetJsapi_Ticket()
        {
            var data = "access_token=" + GetAccess_Token().access_token + "&&type=jsapi";
            var response = HtmlHelper.SendGet(data, getticketUrl);
            return JsonHelper.ToObject<Jsapi_Ticket>(response);
        }
        public static string GetSignature(string noncestr, string timestamp, string url)
        {
            try
            {
                var ticket = string.Empty;

                //因为获取最大次数限制--顾写缓存
                //var ticketCache = MemCacheClientProxyLib.MemCacheProxy.GetInstance().GetValue("WX_Jsapi_Ticket");

                //if (ticketCache != null)
                //    ticket = ticketCache.ToString();
                //else
                    ticket = GetJsapi_Ticket().ticket;

                var string1 = "jsapi_ticket=" + ticket +
                    "&noncestr=" + noncestr +
                    "&timestamp=" + timestamp +
                    "&url=" + url;

                return FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
            }
            catch { return string.Empty; }
        }

        public class Access_Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

        public class Jsapi_Ticket
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public int expires_in { get; set; }
        }
    }
}
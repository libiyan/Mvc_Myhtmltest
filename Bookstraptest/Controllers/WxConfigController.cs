using Bookstraptest.Commons;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Ucsmy.Framework;
using Ucsmy.Framework.Common;

namespace Bookstraptest.Controllers
{
    public class WxConfigController : Controller
    {
        //2015.5.13 微信分享config
        // GET: /WxConfig/   
        public ActionResult AjaxGetWxConfig(string url)
        {
            var result = new ServiceResult("获取微信config信息");
            string noncestr = Guid.NewGuid().ToString().ToLower();
            string timestamp = DateTime.Now.Ticks.ToString().LenthShow(10, "");
            string signdata = WXHelper.GetSignature(noncestr, timestamp, url);
            var jsConfig = new WxJsConfig()
            {
                Noncestr = noncestr,
                Timestamp = timestamp,
                Signdata = signdata.ToLower(),
                Url = url.ToLower()
            };
            result.SetData("appId", "WX_ApplyId".ValueOfAppSettings());
            result.SetData("jsConfig", jsConfig);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public class WxJsConfig
        {
            /// <summary>
            /// 随机串
            /// </summary>
            public string Noncestr { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string Timestamp { get; set; }
            /// <summary>
            /// 签名数据
            /// </summary>
            public string Signdata { get; set; }

            public string Url { get; set; }
        }

        /// <summary>
        /// https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421135319&token=&lang=zh_CN
        /// </summary> 
        public string Index()
        {
            string echostr = Request["echostr"];
            if (CheckSignature())
            {
                if (!string.IsNullOrWhiteSpace(echostr))
                {
                    return echostr;
                }
            }
            return "";
        }
        private bool CheckSignature()
        {
            const string Token = "LazyBone";
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);　　 //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

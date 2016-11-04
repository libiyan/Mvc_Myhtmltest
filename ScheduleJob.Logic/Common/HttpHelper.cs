using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using UCS.Platform.Common.Utils;
using UCS.Platform.Common.DataSign;
namespace ScheduleJob.Logic.Common
{
    public class HttpHelper
    {
        public static HttpHelper Instance
        {
            get
            {
                return new HttpHelper();
            }
        }

        public static bool Post(string url, string data, out string msg)
        {
            try
            {
                TxtLoger.SavaLogToTXT("url为：" + url + ",  data为：" + data, "a");
                WebRequest request = WebRequest.Create(url);
                WebResponse response;
                request.Method = "POST";
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);

                TxtLoger.SavaLogToTXT("byteArray的长度为：" + byteArray.Length, "a");
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
                string responseForm = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                msg = responseForm;
                TxtLoger.SavaLogToTXT("成功发送 ，msg 为：" + msg, "a");
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message; 
                TxtLoger.SavaLogToTXT("post 数据发生异常 详细信息为：" +ex.Message, "a");
                return false;
            }
        }

        //#region 报文数据
        //public static string Notice(string sourceID, object sendData)
        //{
        //    string data = JsonUtils.Serialize(sendData);
        //    var signData = new SignData(sourceID);
        //    string signStr = signData.Sign(data);
        //    var obj = new
        //    {
        //        data = Base64Utils.EncodeBase64String(data),
        //        signdata = Base64Utils.EncodeBase64String(signStr),
        //    };
        //    return JsonUtils.Serialize(obj);
        //}
        //#endregion
    }
}

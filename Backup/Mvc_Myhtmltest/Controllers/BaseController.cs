using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCS.Platform.Common.Utils;
using UCS.Platform.Common;
using System.Net;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using log4net;
using UCS.Platform.Common.DataSign;

namespace Mvc_Myhtmltest.Controllers
{
    /// <summary>
    /// 第三方接口
    /// </summary>
    public class BaseController : Controller
    {
        //日志处理对象
        private ILog loger = LogManager.GetLogger("BaseController");

        #region HTTP
        /// <summary>
        /// 读取Request内容
        /// </summary>
        /// <returns></returns>
        protected string ReadRequestXml()
        {
            var reqStream = Request.InputStream;
            var buffer = new byte[(int)reqStream.Length];
            reqStream.Read(buffer, 0, (int)reqStream.Length);
            var requestXml = System.Text.Encoding.Default.GetString(buffer);
            loger.Debug(string.Format("已获取访问请求数据（base64编码 + 签名）：{0}", requestXml));
            return requestXml;
        }

        /// <summary>
        /// 在response中写数据
        /// </summary>
        /// <param name="response"></param>
        protected void WriteResponse(string response)
        {
            var myRequestStream = Response.OutputStream;
            var myStreamWriter = new StreamWriter(myRequestStream);
            myStreamWriter.Write(response);
            myStreamWriter.Close();
            //loger.Debug(string.Format("处理完返回数据（base64编码 + 签名）：{0}", response));
            TxtLoger.SavaLogToTXT(string.Format("处理完返回数据（base64编码 + 签名）：{0}", response), "a");
        }

        /// <summary>
        /// 向指定url中传送数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        protected void WriteRequest(string url, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);
            var myRequestStream = request.GetRequestStream();
            var myStreamWriter = new StreamWriter(myRequestStream);
            myStreamWriter.Write(postData);
            myStreamWriter.Close();
            loger.Debug(string.Format("请求[{0}]，数据为：{1}", url, postData));
        }

        /// <summary>
        /// 向指定url中传送数据，并获取回复
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        protected string WriteAndReadRequest(string url, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);
            request.Timeout = 300000;
            request.ReadWriteTimeout = 300000;
            var myRequestStream = request.GetRequestStream();
            var myStreamWriter = new StreamWriter(myRequestStream);
            myStreamWriter.Write(postData);
            myStreamWriter.Close();
            loger.Debug(string.Format("请求[{0}]，数据为：{1}", url, postData));
            var res = "";
            var response = (HttpWebResponse)request.GetResponse();
            var myResponseStream = response.GetResponseStream();
            if (myResponseStream != null)
            {
                var myStreamReader = new StreamReader(myResponseStream);
                res = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                loger.Debug(string.Format("请求[{0}]后返回数据为：{1}", url, postData));
            }
            return res;
        }
        #endregion

        #region 处理前的验证

        /// <summary>
        /// 验证并返回请求对象
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <param name="data"></param>
        /// <param name="signdata"></param>
        /// <returns></returns>
        protected VerifyResult Verify(out BaseRequest baseRequest, string data = null, string signdata = null)
        {
            var verifyResult = new VerifyResult { IsPass = true };
            HttpMessage httpMessage;
            //报文验证
            if (!Base64Verify(out httpMessage, out baseRequest, data, signdata))
            {
                return VerifyResult.CreateVerifyResult(false, EnumPlatform.EResponseCode.解析报文错误);
            }
            //未传入SourceID，忽略此请求
            if (string.IsNullOrEmpty(baseRequest.SourceID))
            {
                return VerifyResult.CreateVerifyResult(false, EnumPlatform.EResponseCode.不存在此机构);
            }
            //机构号验证
            if (!SourceIdVerify(baseRequest))
            {
                return VerifyResult.CreateVerifyResult(false, EnumPlatform.EResponseCode.不存在此机构);
            }
            //验签
            if (!SignVerify(httpMessage, baseRequest.SourceID))
            {
                return VerifyResult.CreateVerifyResult(false, EnumPlatform.EResponseCode.验证签名失败);
            }

            return GetRequest(httpMessage.data, baseRequest.BizCode, out baseRequest);
        }

        /// <summary>
        /// Base64编码验证
        /// </summary>
        /// <param name="httpMessage"></param>
        /// <param name="baseRequest"></param>
        /// <param name="data"></param>
        /// <param name="signdata"></param>
        /// <returns></returns>
        private bool Base64Verify(out HttpMessage httpMessage, out BaseRequest baseRequest, string data = null, string signdata = null)
        {
            httpMessage = new HttpMessage();
            baseRequest = new BaseRequest();
            if (data == null || signdata == null)
            {
                var base64 = ReadRequestXml();
                if (string.IsNullOrEmpty(base64))
                {
                    return false;
                }
                //httpMessage = JsonUtils.Deserialize<HttpMessage>(base64);
                httpMessage = GetHttpMessage(base64);
            }
            else
            {
                httpMessage = new HttpMessage()
                {
                    data = data,
                    signdata = signdata
                };
            }

            if (httpMessage == null)
            {
                return false;
            }
            httpMessage.data = Base64Utils.DecodeBase64String(httpMessage.data);
            httpMessage.signdata = Base64Utils.DecodeBase64String(httpMessage.signdata);

            baseRequest = JsonUtils.Deserialize<BaseRequest>(httpMessage.data);
            loger.Debug(string.Format("Base64解码后的请求数据为：{0}", httpMessage.data));
            loger.Debug(string.Format("Base64解码后的较验码为：{0}", httpMessage.signdata));
            return true;
        }

        private HttpMessage GetHttpMessage(string request)
        {
            if (string.IsNullOrEmpty(request))
                return null;
            string[] datas = request.Split('&');
            if (datas.Length == 2)
            {
                string signdata = "";
                string data = "";
                foreach (var d in datas)
                {
                    string s = d.Substring(d.IndexOf('=') + 1);
                    if (d.StartsWith("signdata"))
                    {
                        signdata = s;
                    }
                    else if (d.StartsWith("data"))
                    {
                        data = s;
                    }
                }
                if (!string.IsNullOrEmpty(signdata) && !string.IsNullOrEmpty(data))
                    return new HttpMessage
                    {
                        signdata = signdata,
                        data = data
                    };
            }
            return null;
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="httpMessage"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        private bool SignVerify(HttpMessage httpMessage, string sourceId)
        {
            try
            {
                var verifySignData = new VerifySignData(sourceId);
                var isverify = verifySignData.VerifySign(httpMessage.data, httpMessage.signdata);
                return isverify;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 代销机构编码验证
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        private bool SourceIdVerify(BaseRequest baseRequest)
        {
            var query = new QuerySaleInstitution();
            return query.IsExist(baseRequest.SourceID);
        }

        /// <summary>
        /// 验证RequestData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        protected VerifyResult ModelVerify(BaseRequest t)
        {
            var verifyResult = new VerifyResult();
            var context = new ValidationContext(t, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(t, context, results);
            var errorMsg = new StringBuilder();
            if (!isValid)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    ValidationResult r = results[i];
                    errorMsg.Append(String.Format("{0}、{1}。\r\n", i + 1, r.ErrorMessage));
                }
                return VerifyResult.CreateVerifyResult(false, EnumPlatform.EResponseCode.参数不正确, errorMsg.ToString());
            }
            return VerifyResult.CreateVerifyResult(true, EnumPlatform.EResponseCode.成功受理了请求);
        }

        /// <summary>
        /// 获取request对象
        /// </summary>
        /// <param name="requestStr"></param>
        /// <param name="bizCode"></param>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        protected VerifyResult GetRequest(string requestStr, string bizCode, out BaseRequest baseRequest)
        {
            baseRequest = null;
            switch (bizCode)
            {
               
                case "1006":
                    baseRequest = JsonUtils.Deserialize<BaseRequest>(requestStr);
                    break;
         
                default:
                    return VerifyResult.CreateVerifyResult(false, EnumPlatform.EResponseCode.无效交易类型);
            }
            return ModelVerify(baseRequest);
        }

        #endregion
    }
}

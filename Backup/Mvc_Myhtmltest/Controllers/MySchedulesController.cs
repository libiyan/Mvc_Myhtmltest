using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using UCS.Platform.Common.Utils;
using UCS.Platform.Common.DataSign;
using UCS.Platform.Common;
using ScheduleJob.Logic;


namespace Mvc_Myhtmltest.Controllers
{
    public class MySchedulesController : BaseController
    {
        //
        // GET: /MySchedules.cs/
        //public ActionResult Index(string data, string signdata)
        //{

        //    ViewBag.aa = data;
        //    ViewBag.bb = signdata;
        //    return View();
        //}
        //{"data":"eyJTb3VyY2VJRCI6IjEwMDAxIiwiUGF5bWVudE5vIjoiMjdjY2U3NDc5ZjQ3NDZjNDljNzVhMTYzZDE3ZTZjMGYiLCJBbW91bnQiOjIwMDAwLjAwLCJTdGF0dXMiOjEsIlBheVRpbWUiOiIyMDE0MTAwOTE1MTYzMCIsIkJhbmRDT0RGbGFnIjowfQ==","signdata":"ZTZyTEZaZmFIcTdQRW92elYvVlF3N0NwVDdOYUJsa1F3UUFKV2dYNW43YnRIRU1QQmNrc2RTZmYzUzMyNWl1d000R2dFR1RrYzVpTno1WUZLbzB0dEhQeXlWYnI0eWtqb21vVkNVaVVKakRwWjRPQ3lQQUZKamxZUXpqRElTbUROM3NxYVNIRWl2anYyRXprcnhkYmZkTXhwWVlRK3RQeERSNGJwMm1OTUVFPQ=="}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            BaseRequest baseRequest;
            var verifyResult = Verify(out baseRequest);
            TxtLoger.SavaLogToTXT(string.Format("请求数据验证结果：编号：{0};信息：{1}", (int)verifyResult.Code, verifyResult.ErrorMessage), "a");
            //loger.Info(string.Format("请求数据验证结果：编号：{0};信息：{1}", (int)verifyResult.Code, verifyResult.ErrorMessage));
            if (verifyResult.IsPass)
            {
                var response = Excute(baseRequest);
                Notice(response, baseRequest.SourceID);
            }
            else
            {
               
                if (verifyResult.Code != EnumPlatform.EResponseCode.不存在此机构)
                {
                    var response = BaseResponse.CreateBaseResponse(verifyResult.Code, verifyResult.ErrorMessage, null);
                    Notice(response, baseRequest.SourceID);

                    TxtLoger.SavaLogToTXT("response 数据为：" +JsonUtils.Serialize(response), "a");
                }
                else {
                    TxtLoger.SavaLogToTXT("不存在此机构,verifyResult.Code =" + verifyResult.Code, "a");
                };
            }
            return View();
        }
        //public ActionResult Index()
        //{
        //    BaseRequest baseRequest;
        //    var verifyResult = Verify(out baseRequest);
        //    //loger.Info(string.Format("请求数据验证结果：编号：{0};信息：{1}", (int)verifyResult.Code, verifyResult.ErrorMessage));
        //    if (verifyResult.IsPass)
        //    {
        //        var response = Excute(baseRequest);
        //        Notice(response, baseRequest.SourceID);
        //    }
        //    else
        //    {
        //        if (verifyResult.Code != EnumPlatform.EResponseCode.不存在此机构)
        //        {
        //            var response = BaseResponse.CreateBaseResponse(verifyResult.Code, verifyResult.ErrorMessage, null);
        //            Notice(response, baseRequest.SourceID);
        //        };
        //    }
        //    ViewBag.aa = baseRequest.SourceID;
        //    ViewBag.bb = baseRequest.BizCode;
        //    //return Json(JsonUtils.Serialize(verifyResult));
        //    //return View(JsonUtils.Serialize(verifyResult));
        //    return View();
        //} 
        /// <summary>
        /// 返回数据给第三方
        /// </summary>
        /// <param name="response"></param>
        /// <param name="sourceId"></param>
        private void Notice(BaseResponse response, string sourceId)
        {
            if (sourceId == null)
            {
                sourceId = "10001";
            }
            var responseData = JsonUtils.Serialize(response);
            var signData = new SignData(sourceId);
            var signstr = signData.Sign(responseData);
            //loger.Info(string.Format("处理结果数据（base64编码前）：{0}", responseData));
            //loger.Info(string.Format("处理结果较验码（base64编码前）：{0}", signstr));
            var httpMessage = new HttpMessage
            {
                data = Base64Utils.EncodeBase64String(responseData),
                signdata = Base64Utils.EncodeBase64String(signstr)
            };
            TxtLoger.SavaLogToTXT("data=" + Base64Utils.EncodeBase64String(responseData) + " ,signdata=" + Base64Utils.EncodeBase64String(signstr), "a");
            //var responseStr = JsonUtils.Serialize(httpMessage);
            //WriteResponse(httpMessage.ToString());
            WriteResponse(JsonUtils.Serialize(httpMessage));
        }

        /// <summary>
        /// 根据bizCode选择处理方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private BaseResponse Excute(BaseRequest request)
        {

            switch (request.BizCode)
            {
               
                case "1006":
                    return Excute1006(request);
            }
            return BaseResponse.CreateBaseResponse(EnumPlatform.EResponseCode.无效交易类型, null);
        }

        private BaseResponse Excute1006(BaseRequest requestData)
        {
            //var res = GeneralFunction.CreateOrder(requestData as NoticeRequest1006);
            var res = requestData;
            if (requestData.SourceID == "10001")
            {
                //var data = res.DicData["OrderNo"];
                var data = res;
                var resCode = EnumPlatform.EResponseCode.成功受理了请求;
                return new BaseResponse
                {
                    Code = resCode,
                    Message = resCode.ToString(),
                    Data = data,
                };
                TxtLoger.SavaLogToTXT("成功受理了请求,code" , "a");

            }
            else
            {
                var resCode = (EnumPlatform.EResponseCode)Int16.Parse(res.BizCode);
                return new BaseResponse
                {
                    Code = resCode,
                    Message = resCode.ToString(),
                    Data = null,
                };
            }
        }

 

    }
}

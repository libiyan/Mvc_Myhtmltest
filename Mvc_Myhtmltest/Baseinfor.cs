using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCS.Platform.Common;
using System.ComponentModel.DataAnnotations;
using UCS.Platform.Common.Utils;

namespace Mvc_Myhtmltest
{
    public class Baseinfor
    {
    }
    #region class BaseRequest
    public class BaseRequest
    {
        /// <summary>
        /// 接入机构编码
        /// </summary>
        [Required(ErrorMessage = "接入机构编码不能为空")]
        public string SourceID { get; set; }

        /// <summary>
        /// 交易编码=1001/1002
        /// </summary>
        [Required(ErrorMessage = "交易编码不能为空")]
        public string BizCode { get; set; }
    }
    #endregion

    #region class BaseResponse
    public class BaseResponse
    {
        /// <summary>
        /// 返回结果编号
        /// </summary>
        public EnumPlatform.EResponseCode Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回结果数据
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Code={0}&Message={1}&Data={2}", Code, Message, JsonUtils.Serialize(Data));
        }

        /// <summary>
        /// 创建返回结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BaseResponse CreateBaseResponse(EnumPlatform.EResponseCode code, Object data)
        {
            return new BaseResponse()
            {
                Code = code,
                Message = code.ToString(),
                Data = data
            };
        }


        /// <summary>
        /// 创建返回结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BaseResponse CreateBaseResponse(EnumPlatform.EResponseCode code, string message, Object data)
        {
            return new BaseResponse()
            {
                Code = code,
                Message = message,
                Data = data
            };
        }
    }
    #endregion

    #region class VerifyResult
    public class VerifyResult
    {
        public EnumPlatform.EResponseCode Code { get; set; }

        public bool IsPass { get; set; }

        public string ErrorMessage { get; set; }

        public static VerifyResult CreateVerifyResult(bool isPass, EnumPlatform.EResponseCode code, string errorMessage = "")
        {
            return new VerifyResult
            {
                IsPass = isPass,
                Code = code,
                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? code.ToString() : errorMessage
            };
        }
    }
    #endregion

    #region class HttpMessage
    /// <summary>
    /// 报文结构
    /// </summary>
    public class HttpMessage
    {
        /// <summary>
        /// 请求内容
        /// </summary>
        [Required]
        public string data { get; set; }

        /// <summary>
        /// 签名内容
        /// </summary>
        [Required]
        public string signdata { get; set; }

        /// <summary>
        /// 转换成传输字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("data={0}&signdata={1}", data, signdata);
        }
    }
    #endregion

    #region 第三方机构查询
    /// <summary>
    /// 第三方机构查询
    /// </summary>
    public class QuerySaleInstitution
    {
        /// <summary>
        /// 判断机构号是否存在
        /// </summary>
        /// <param name="SourceId">机构号</param>
        /// <returns></returns>
        public bool IsExist(string SourceId)
        {
            //var list = QueryService.Platform.FindList<SaleInstitution>(
            //        t => t.SourceID.Equals(SourceId) && t.DelFlag != 1
            //            && t.FailureTime == null
            //    );
            //return list != null && list.Count > 0;

            if (SourceId == "10001")
            {
                return true;
            }
            return false;
        }

    }
    #endregion
}
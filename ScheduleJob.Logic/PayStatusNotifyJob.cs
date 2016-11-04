using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using My.DataAccess;
using My.DataAccess.PlatformModel;
using Ucsmy.Framework.Common;
using Common.Logging;
using Ucsmy.Framework;
using UCS.Platform.Common;
using UCS.Platform.Common.Utils;
using UCS.Platform.Common.DataSign;

namespace ScheduleJob.Logic.Common
{
    /*1006支付状态通知任务*/
    public class PayStatusNotifyJob : IJob
    {
        //日志处理对象
        private ILog loger = LogManager.GetLogger("PayStatusNotifyJob");
        public void Execute(IJobExecutionContext context)
        {
            TxtLoger.SavaLogToTXT("开始执行", "a");
            var res = new ServiceResult("PayStatusNotifyJob异常!");
            try
            {
                //通知次数<8 
                var PayNotifyList = QueryService.Platform.FindList<PayStatusNotifyTask>(t => t.TaskStatus == false && t.TaskNum < 8 && t.DelFlag == false);
                if (null != PayNotifyList)
                    TxtLoger.SavaLogToTXT("中行数为：" + PayNotifyList.Count, "a");
                    foreach (PayStatusNotifyTask paynotify in PayNotifyList)
                    {
                        if (paynotify.Status == 1)
                        {
                            NoticeRequest1006 sendCmd = new NoticeRequest1006
                            {
                                SourceID = paynotify.SourceID,
                                PaymentNo = paynotify.PaymentNo,
                                Amount = paynotify.Amount,
                                Status = paynotify.Status,
                                PayTime = string.Format("{0:yyyyMMddHHmmss}", paynotify.PayTime),
                                BandCODFlag = paynotify.BandCODFlag
                            };
                            string msg = "向代销机构推送数据";

                            //获取代销机构回调地址
                            string url = ConfigManager.AppSetting("CallBack_" + paynotify.SourceID);
                            //发送前签名data
                            //string notice = HttpHelper.Notice(paynotify.SourceID, sendCmd);
                            string notice = Notice(paynotify.SourceID, sendCmd);
                            TxtLoger.SavaLogToTXT("签名，后notice为：" + notice, "a");

                            //bool IsPost = HttpHelper.Post(url, notice, out msg);
                            //if (IsPost)
                            //{
                            //    TxtLoger.SavaLogToTXT("推送，后msg为：" + msg, "a");
                            //    //发送后，解析响应码
                            //    if (VerifyCode(msg, paynotify.SourceID))
                            //    {
                            //        paynotify.TaskStatus = true;
                            //    }
                            //    if (paynotify.TaskNum < 8)
                            //    {
                            //        paynotify.TaskNum = paynotify.TaskNum + 1;
                            //    }
                            //    if (UpdateTaskModel(paynotify) <= 0)
                            //    {
                            //        loger.Debug("更新支付状态通知任务表失败");
                            //    }
                            //}
                            //else
                            //{
                            //    loger.Debug("向代销机构推送数据失败，异常详情为：" + msg);
                            //}
                            paynotify.TaskStatus = true;
                            if (paynotify.TaskNum < 8)
                            {
                                paynotify.TaskNum = paynotify.TaskNum + 1;
                            }
                            if (UpdateTaskModel(paynotify) <= 0)
                            {
                                loger.Debug("更新支付状态通知任务表失败");
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                res.IsError(ex);
                TxtLoger.SavaLogToTXT("发生异常：" + ex.Message, "a");
            }
            TxtLoger.SavaLogToTXT("结束执行：", "a");
        }
        #region 报文数据
        public string Notice(string sourceID, NoticeRequest1006 sendData)
        {
            string data = JsonUtils.Serialize(sendData);

            TxtLoger.SavaLogToTXT("推送，前data为：" + data, "a");
            var signData = new SignData(sourceID);
            string signStr = signData.Sign(data);
            var obj = new
            {
                data = Base64Utils.EncodeBase64String(data),
                signdata = Base64Utils.EncodeBase64String(signStr),
            };

            TxtLoger.SavaLogToTXT("obj是 数据是：" + obj.ToJson(), "a");
            return JsonUtils.Serialize(obj);
        }
        #endregion

        #region 判断响应码--是否推送成功
        /// <param name="httpMessage">out msg</param>
        /// <param name="sourceId">机构号</param>
        private bool VerifyCode(string httpMessage, string sourceId)
        {
            var res = new ServiceResult();
            try
            {
                NoticeResponse1006 respose = JsonUtils.Deserialize<NoticeResponse1006>(httpMessage);
                if (respose.Code == "100")
                {
                    TxtLoger.SavaLogToTXT("推送成功！机构号为：" + sourceId + ",响应状态为：" + Enum.GetName(typeof(EnumPlatform.EResponseCode), respose.Code), "a");
                    return true;
                }
                else
                {

                    TxtLoger.SavaLogToTXT("推送失败！机构号为：" + sourceId + ",响应状态为：" + Enum.GetName(typeof(EnumPlatform.EResponseCode), respose.Code), "a");
                    res.IsFailure("推送失败！机构号为：" + sourceId + ",响应状态为：" + Enum.GetName(typeof(EnumPlatform.EResponseCode), respose.Code));
                    return false;
                }
            }
            catch (Exception ex)
            {
                TxtLoger.SavaLogToTXT("推送支付通知接受响应码异常：机构号为：" + sourceId + ",响应异常详情：" + ex.Message, "a");
                res.IsError(ex, "推送支付通知接受响应码异常：机构号为：" + sourceId + ",响应异常详情：" + ex.Message);
                return false;
            }
        }
        #endregion

        #region 更新通知次数，任务状态
        private int UpdateTaskModel(PayStatusNotifyTask paynotify)
        {
            var res = new ServiceResult("更新支付通知任务表失败！");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update PayStatusNotifyTask set ");
                if (paynotify.TaskStatus != null)
                {
                    sb.Append(" TaskStatus=@TaskStatus ");
                }
                sb.Append(",TaskNum=@TaskNum");
                sb.Append(" where TaskID=@TaskID");
                System.Data.SqlClient.SqlParameter[] parm = { new System.Data.SqlClient.SqlParameter("@TaskStatus",paynotify.TaskStatus),
                                                              new System.Data.SqlClient.SqlParameter("@TaskNum",paynotify.TaskNum),
                                                              new System.Data.SqlClient.SqlParameter("@TaskID",paynotify.TaskID)
                                                                    };
                return QueryService.Platform.ExecuteSqlCommand(sb.ToString(), parm);
            }
            catch (Exception ex)
            {
                TxtLoger.SavaLogToTXT("更新支付通知任务表失败：机构号为：" + paynotify.SourceID + ",update异常详情：" + ex.Message, "a");
                res.IsError(ex);
            }
            return 0;
        }
        #endregion

            //14-10-9
        //    try
        //    {
        //        //通知次数<8 
        //        var PayNotifyList = QueryService.Platform.FindList<PayStatusNotifyTask>(t => t.TaskStatus == false && t.TaskNum < 8 && t.DelFlag == false);
        //        if (null != PayNotifyList)
        //            TxtLoger.SavaLogToTXT("中行数为：" + PayNotifyList.Count, "a");
        //            foreach (PayStatusNotifyTask paynotify in PayNotifyList)
        //            {
        //                var sendCmd = new
        //                {
        //                    SourceID = paynotify.SourceID,
        //                    PaymentNo = paynotify.PaymentNo,
        //                    Amount = paynotify.Amount,
        //                    Status = paynotify.Status,
        //                    PayTime = string.Format("{0:yyyyMMddHHmmss}", paynotify.PayTime)
        //                };
        //                string msg = "12121";
        //                string data = sendCmd.ToJson();
        //                //获取代销机构回调地址
        //                //string url = ConfigManager.AppSetting("CallBack_" + paynotify.SourceID);
        //                //bool IsPost = UCS.ScheduleJob.Logic.Common.HttpHelper.Post(url, data, out msg);
        //                //if (IsPost)
        //                //{
        //                //    paynotify.TaskStatus = true;
        //                //}
        //                paynotify.TaskStatus = true;
        //                TxtLoger.SavaLogToTXT("时间是：" + sendCmd.PayTime + ",时间paytime是：" + paynotify.PayTime, "b");
        //                if (UpdateTaskModel(paynotify) <= 0)
        //                {
        //                    TxtLoger.SavaLogToTXT("更新支付状态通知任务表失败", "a");
        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        TxtLoger.SavaLogToTXT("PayStatusNotifyJob异常,信息为：" + ex.Message, "a");
        //    }
        //}

        //#region 更新通知次数，任务状态
        //private int UpdateTaskModel(PayStatusNotifyTask paynotify)
        //{
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("update PayStatusNotifyTask set ");
        //        if (paynotify.TaskStatus != null)
        //        {
        //            sb.Append(" TaskStatus=@TaskStatus ");
        //        }
        //        sb.Append(",TaskNum=@TaskNum");
        //        sb.Append(" where TaskID=@TaskID");
        //        if (paynotify.TaskNum < 8)
        //        {
        //            paynotify.TaskNum = paynotify.TaskNum + 1;
        //        }
        //        System.Data.SqlClient.SqlParameter[] parm = { new System.Data.SqlClient.SqlParameter("@TaskStatus",paynotify.TaskStatus),
        //                                                      new System.Data.SqlClient.SqlParameter("@TaskNum",paynotify.TaskNum),
        //                                                      new System.Data.SqlClient.SqlParameter("@TaskID",paynotify.TaskID)
        //                                                            };
        //        return QueryService.Platform.ExecuteSqlCommand(sb.ToString(), parm);
        //    }
        //    catch (Exception ex)
        //    {
        //        TxtLoger.SavaLogToTXT("更新任务表异常,id为：" + paynotify.TaskID + "更新出错,错误详情为:" + ex.Message, "a");
        //    }
        //    return 0;
        //}
        //#endregion

        //----------*****下面的**************
 //       //private static ILog _log = LogManager.GetLogger(typeof(PayStatusNotifyJob));
 //       public void Execute(IJobExecutionContext context)
 //       {
 //           try
 //           {
 //               var PayNotifyList = QueryService.Platform.FindList<PayStatusNotifyTask>(t => t.TaskStatus == false && t.TaskNum < 8 && t.DelFlag == false);
 //               if (null != PayNotifyList)
 //                   TxtLoger.SavaLogToTXT("获取list总数是：" + PayNotifyList.Count, "a");
 //               //_log.Info("Executing job: PayStatusNotifyJob executing at " + DateTime.Now.ToString("r"));
 //               foreach (PayStatusNotifyTask paynotify in PayNotifyList)
 //               {
 //                   //通知次数
 //                   var sendCmd = new
 //                      {
 //                          SourceID = paynotify.SourceID,
 //                          PaymentNo = paynotify.PaymentNo,
 //                          Amount = paynotify.Amount,
 //                          Status = paynotify.Status,
 //                          PayTime = string.Format("{0:yyyyMMddHHmmss}", paynotify.PayTime)
 //                      };
 //                   string msg = "支付状态通知：";
 //                   string data = sendCmd.ToJson();
 //                   //获取代销机构回调地址
 //                   //string url = ConfigManager.AppSetting("CallBack_" + paynotify.SourceID);
 //                   //bool IsPost = Common.HttpHelper.Post(url, data, out msg);
 //                   //if (IsPost)
 //                   //{
 //                   //    paynotify.TaskStatus = true;
 //                   //}
 //                   //if (paynotify.TaskNum < 8) { paynotify.TaskNum += 1; }
 //                   TxtLoger.SavaLogToTXT("时间是：" + sendCmd.PayTime+",时间paytime是："+paynotify.PayTime, "b");
 //                   paynotify.TaskStatus = true;

 //                   if (UpdateTaskModel(paynotify) <= 0)
 //                   {
 //                       TxtLoger.SavaLogToTXT("更新支付状态通知任务表失败", "a");
 //                   }

 //                   TxtLoger.SavaLogToTXT(msg + "data是:" + data, "a");
 //               }
 //           }
 //           catch (Exception ex)
 //           {
 //               TxtLoger.SavaLogToTXT("PayStatusNotifyJob异常,信息为：" + ex.Message, "a");
 //               //_log.Info("Executing job: PayStatusNotifyJob 异常,信息为: " + ex.Message);
 //           }
 //       }
 //#region 更新通知次数，任务状态
 //       private int UpdateTaskModel(PayStatusNotifyTask paynotify)
 //       {

 //           try
 //           {
 //               StringBuilder sb = new StringBuilder();
 //               sb.Append("update PayStatusNotifyTask set ");
 //               if (paynotify.TaskStatus != null)
 //               {
 //                   sb.Append(" TaskStatus=@TaskStatus ");
 //               }
 //               sb.Append(",TaskNum=@TaskNum");
 //               if (paynotify.TaskNum < 8)
 //               {
 //                   paynotify.TaskNum = paynotify.TaskNum + 1;
 //               }
 //               System.Data.SqlClient.SqlParameter[] parm = { new System.Data.SqlClient.SqlParameter("@TaskStatus",paynotify.TaskStatus),
 //                                                              new System.Data.SqlClient.SqlParameter("@TaskNum",paynotify.TaskNum)
 //                                                                   };
 //               TxtLoger.SavaLogToTXT("更新任务表 id为："+paynotify.TaskID+",sourceid为："+paynotify.SourceID , "a");
 //               sb.Append(" where TaskID='"+ paynotify.TaskID+"'");
 //               return QueryService.Platform.ExecuteSqlCommand(sb.ToString(), parm);
 //           }
 //           catch (Exception ex)
 //           {
 //              TxtLoger.SavaLogToTXT("更新任务表出错：" + ex.Message, "a");
 //           }
 //           return 0;
 //           //return QueryService.Platform.ExecuteSqlCommand("update PayStatusNotifyTask set TaskStatus=" + paynotify.TaskStatus + ",TaskNum=" + paynotify.TaskNum);
 //       }
 //       #endregion

    }
}

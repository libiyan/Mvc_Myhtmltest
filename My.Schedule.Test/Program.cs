using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using My.DataAccess.PlatformModel;
using My.DataAccess;

namespace My.Schedule.Test
{
  public class Program
    {
        static void Main(string[] args)
        {
                 PayStatusNotifyTask paynotify = new PayStatusNotifyTask{SourceID = "1005", PaymentNo = "445443443", Amount = 1000, BandCODFlag = 0};
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into PayStatusNotifyTask(SourceID,PaymentNo,Amount,BandCODFlag)values(@SourceID,@PaymentNo,@Amount,@BandCODFlag)");
               
           
                System.Data.SqlClient.SqlParameter[] parm = { 
                                                              new System.Data.SqlClient.SqlParameter("@SourceID",paynotify.SourceID),
                                                              new System.Data.SqlClient.SqlParameter("@PaymentNo", paynotify.PaymentNo),
                                                              new System.Data.SqlClient.SqlParameter("@Amount",paynotify.Amount),
                                                              new System.Data.SqlClient.SqlParameter("@BandCODFlag",paynotify.BandCODFlag)
                                                                    };

                //int res= QueryService.Platform.ExecuteSqlCommand(sb.ToString(), parm);
                int res= QueryService.Platform.ExecuteSqlCommand(sb.ToString(), parm);
                Console.WriteLine("执行完成,"+res);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                //TxtLoger.SavaLogToTXT("更新任务表异常,id为：" + paynotify.TaskID + "更新出错,错误详情为:" + ex.Message, "a");
                throw ex;
            }
        }
    }
}

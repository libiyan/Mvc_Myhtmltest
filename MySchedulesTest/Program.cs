using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using My.DataAccess;
using My.DataAccess.PlatformModel;
using Ucsmy.Framework.Common;

namespace MySchedulesTest
{
    class Program
    {
        static void Main(string[] args)
        {


        }
        #region 更新通知次数，任务状态
        private int UpdateTaskModel(PayStatusNotifyTask paynotify)
        {
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
                if (paynotify.TaskNum < 8)
                {
                    paynotify.TaskNum = paynotify.TaskNum + 1;
                }
                System.Data.SqlClient.SqlParameter[] parm = { new System.Data.SqlClient.SqlParameter("@TaskStatus",paynotify.TaskStatus),
                                                              new System.Data.SqlClient.SqlParameter("@TaskNum",paynotify.TaskNum),
                                                              new System.Data.SqlClient.SqlParameter("@TaskID",paynotify.TaskID)
                                                                    };
                return QueryService.Platform.ExecuteSqlCommand(sb.ToString(), parm);
            }
            catch (Exception ex)
            {
                //TxtLoger.SavaLogToTXT("更新任务表异常,id为：" + paynotify.TaskID + "更新出错,错误详情为:" + ex.Message, "a");
            }
            return 0;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Common.Logging;
using System.IO;

namespace ScheduleJob.Logic
{
   public class JobSchedule:IJob
    {
       private static int n = 0;
        public void Execute(IJobExecutionContext context)
        {
            
       ILog log = LogManager.GetLogger(typeof(JobSchedule));  
            
          StreamWriter w = null;  
          try  
          {  
              n++;  
              w = new StreamWriter("D:\\2.txt", true, System.Text.Encoding.UTF8);  
              w.WriteLine("------------------------------------");  
              w.WriteLine(n+" JobExecute_1正执行:时间:" + DateTime.Now);  
              w.WriteLine("------------------------------------");  
                
          }  
          finally  
          {  
              if (w != null)  
              {  
                  w.Close();  
                  w.Dispose();  
              }  
          }  

        }
    }
}

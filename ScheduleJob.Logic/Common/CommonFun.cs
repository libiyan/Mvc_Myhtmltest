using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Spi;
using Quartz.Impl.Triggers;
using System.IO;
using System.Diagnostics;

namespace ScheduleJob.Logic.Common
{
    public class CommonFun
    {
        public static CommonFun Instance
        {
            get { return new CommonFun(); }
        }

        public void AddSchedule<T>(ref IScheduler sched, string croname, string trigroup, string trigGroupName) where T : IJob
        {
            JobKey jobKey = new JobKey(croname + "key", trigroup);
            IJobDetail jobDetail = JobBuilder.Create<T>().WithIdentity(jobKey).Build();
            IOperableTrigger jobTrig = new CronTriggerImpl(croname + "Trig", trigGroupName, ConfigManager.SectionString("Crons", croname));
            sched.ScheduleJob(jobDetail, jobTrig);
        }
    }
}

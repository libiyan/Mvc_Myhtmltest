using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Quartz;
using ScheduleJob.Logic.Common;
using ScheduleJob.Logic;

namespace WinSchedule.Services
{
    partial class MainService : ServiceBase
    {
        IScheduler sched = null;
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            Quartz.ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory();
            sched = sf.GetScheduler();

            string MyGroupName = "PlatformJobGroup";
            string TrigGroupName = "PlatformTrigerGroup";
            //CommonFun.Instance.AddSchedule<JobSchedule>(ref sched, "DoImport", TrigGroupName, MyGroupName);
            CommonFun.Instance.AddSchedule<PayStatusNotifyJob>(ref sched, "DoImport", TrigGroupName, MyGroupName);
            sched.Start();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            sched.Shutdown();
        }
    }
}

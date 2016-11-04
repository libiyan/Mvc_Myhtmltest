using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;

namespace WindowsService_Txt
{
    partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            
            //StartHello();
        }
        bool Flag = true;
        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            try
            {
                //ThreadHello.Abort();

               Flag = false;

                System.Diagnostics.Trace.Write("线程停止");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);
            }
        }
        //private Thread ThreadHello;

        //private void StartHello()
        //{
        //    try
        //    {
        //        // 标准形式

        //        //ThreadStart NewThreadStart = new ThreadStart(VoidName);
        //        //Thread NewThead = new Thread(NewThreadStart);
        //        //NewThead.Start();

        //        ThreadHello = new Thread(new ThreadStart(Hello));
        //        ThreadHello.Start();
        //        System.Diagnostics.Trace.Write("线程任务开始");
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Trace.Write(ex.Message);
        //        throw ex;
        //    }
        //}

        //private void Hello()
        //{
        //    while (Flag)
        //    {
        //        //localhost.AdvService la = new MainService.localhost.AdvService();
        //        try
        //        {
        //            string helloname = "aaaaaaaaa,";
        //            //string helloname = la.Hello("Andy"); // 调用远程WebService中的方法
        //            writeInLog(helloname, false);   // 把调用返回的数据写入到文件中
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Trace.Write(ex.Message);
        //            throw ex;
        //        }

        //        Thread.Sleep(5000);
        //    }
        //}

        ///// <summary>
        ///// 写入文件操作
        ///// </summary>
        ///// <param name="msg">写入内容</param>
        ///// <param name="IsAutoDelete">是否删除</param>
        //private void writeInLog(string msg, bool IsAutoDelete)
        //{
        //    try
        //    {
        //        string logFileName = @"E:\txt\log.txt"; // 文件路径
        //        FileInfo fileinfo = new FileInfo(logFileName);
        //        if (IsAutoDelete)
        //        {
        //            if (fileinfo.Exists && fileinfo.Length >= 1024)
        //            {
        //                fileinfo.Delete();
        //            }
        //        }
        //        using (FileStream fs = fileinfo.OpenWrite())
        //        {
        //            StreamWriter sw = new StreamWriter(fs);
        //            sw.BaseStream.Seek(0, SeekOrigin.End);
        //            sw.WriteLine("=====================================");
        //            sw.Write("添加日期为:" + DateTime.Now.ToString() + "\r\n");
        //            sw.Write("日志内容为:" + msg + "\r\n");
        //            sw.WriteLine("=====================================");
        //            sw.Flush();
        //            sw.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
    }
}

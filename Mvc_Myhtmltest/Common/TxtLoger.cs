using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace Mvc_Myhtmltest.Common
{
    /// <summary>
    /// 文本文件日志
    /// </summary>
    public static class TxtLoger
    {
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="logCon"></param>
        public static void SavaLogToTXT(string logCon, string folder, Exception ex = null)
        {
            lock ((typeof(TxtLoger)))
            {
                try
                {
                    string path = ConfigurationManager.AppSettings["WinServer_Log"];
                    string pathDir = path + @"\" + folder;

                    if (!Directory.Exists(pathDir))
                    {
                        Directory.CreateDirectory(pathDir);
                    }
                    string logtxt = pathDir + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    if (File.Exists(logtxt))
                    {
                        using (FileStream Files = new FileStream(@logtxt, FileMode.Append))
                        {
                            StreamWriter sw = new StreamWriter(Files);
                            sw.WriteLine(string.Format("[日志]: {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--->", logCon));
                            if (ex != null)
                            {
                                sw.WriteLine(ex.ToString());
                            }
                            sw.Dispose();
                            sw.Close();
                        }
                    }
                    else
                    {
                        using (FileStream Filess = new FileStream(@logtxt, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(Filess);
                            sw.WriteLine(string.Format("[日志]: {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--->", logCon));
                            if (ex != null)
                            {
                                sw.WriteLine(ex.ToString());
                            }
                            sw.Dispose();
                            sw.Close();
                        }
                    }
                }
                catch (Exception logEx)
                {
                    throw logEx;
                }
            }
        }
    }
}
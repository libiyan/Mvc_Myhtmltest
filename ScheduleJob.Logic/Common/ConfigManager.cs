using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;

namespace ScheduleJob.Logic.Common
{
    public class ConfigManager
    {
        /// <summary>
        /// 读取AppSetting节点信息
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string AppSetting(string Key)
        {
            return ConfigurationManager.AppSettings[Key] == null ? "" : ConfigurationManager.AppSettings[Key].ToString();
        }

        /// <summary>
        /// 读取connectionStrings节点信息
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ConnectString(string Key)
        {
            return ConfigurationManager.ConnectionStrings[Key] == null ? "" : ConfigurationManager.ConnectionStrings[Key].ConnectionString;
        }

        /// <summary>
        /// 读取其它节点信息
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string SectionString(string SectionName, string Key)
        {
            IDictionary Dicts = (IDictionary)ConfigurationManager.GetSection(SectionName);
            if (Dicts != null)
            {
                return Dicts[Key] == null ? "" : Dicts[Key].ToString();
            }
            else { return ""; }
        }
    }
}

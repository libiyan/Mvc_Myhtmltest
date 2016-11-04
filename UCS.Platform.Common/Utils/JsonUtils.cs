using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace UCS.Platform.Common.Utils
{
    public class JsonUtils
    {

        public static int GetIntProperty(string key, string json)
        {
            var code = 0;
            var regex = new Regex(String.Format("\"{0}\":(?<value>-?\\d+)", key), RegexOptions.IgnoreCase);

            var match = regex.Match(json);
            if (match.Success)
            {
                var codeValue = match.Groups["value"].Value;

                int.TryParse(codeValue, out code);

                return code;
            }

            return 0;
        }

        public static string GetStringProperty(string key, string json)
        {

            var regex = new Regex(String.Format("\"{0}\":\"(?<value>.+?)\"", key), RegexOptions.IgnoreCase);

            var match = regex.Match(json);
            if (match.Success)
            {
                return match.Groups["value"].Value;
            }

            return String.Empty;
        }

        public static string Serialize(object obj)
        {
            var jser = new JavaScriptSerializer();
            var json = jser.Serialize(obj);

            json = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });

            return json;
        }

        public static T Deserialize<T>(string json)
        {
            var jser = new JavaScriptSerializer();
            return jser.Deserialize<T>(json);
        }
    }
}

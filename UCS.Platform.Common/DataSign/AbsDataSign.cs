using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Configuration;

namespace UCS.Platform.Common.DataSign
{
    public abstract class AbsDataSign
    {
        protected DataSignModel dataSignModel;
        private string dataSignFilePath;
        private string configFileName = "config.ini";

        protected AbsDataSign(string sourceId)
        {
            this.dataSignModel = new DataSignModel { SourceID = sourceId };
            this.dataSignFilePath = ConfigurationManager.AppSettings["DataSignFilePath"] + "\\" + dataSignModel.SourceID;
            Init();
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        private void Init()
        {
            string configFilePath = dataSignFilePath + "\\" + configFileName;
            var file = new FileInfo(configFilePath);
            if (!file.Exists)
            {
                dataSignModel.Message = "验证签名失败";
                return;
            }

            //读取加签文件加密密码
            dataSignModel.SignPassword = ReadString("CONFIG", "SignPassword", "", configFilePath);
            //读取加签文件路径
            dataSignModel.PfxPath = ReadString("CONFIG", "PfxPath", "", configFilePath);
            //读取验签文件路径
            dataSignModel.CerPath = ReadString("CONFIG", "CerPath", "", configFilePath);
            //读取验签文件类型
            dataSignModel.VerifySignFileType = GetVerifySignFileType(configFilePath);

        }

        private VerifySignFileType GetVerifySignFileType(string FileName)
        {
            var type = ReadString("CONFIG", "VerifySignFileType", "", FileName);
            if (type.ToLower().Equals(VerifySignFileType.Base64.ToString().ToLower()))
            {
                return VerifySignFileType.Base64;
            }
            else
            {
                return VerifySignFileType.Pem;
            }

        }

        public string ReadString(string Section, string Ident, string Default, string FileName)
        {
            var Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }
        //{"data":"eyJTb3VyY2VJRCI6IjEwMDAxIiwiUGF5bWVudE5vIjoiM2JkODE3MWE4YTg4NDA3Y2I0YzczNmExMDM5YWM3OTgiLCJBbW91bnQiOjUwMDAwLjAwLCJTdGF0dXMiOjEsIlBheVRpbWUiOiIyMDE0MTAwOTE4MzMxMCIsIkJhbmRDT0RGbGFnIjowfQ==",
        //"signdata":"a3kxV1hPZ0duTGtQbkJGMWxxZy84QUFjSEI2MlVKaDY4NStjL0VUTEZGR2Fza0Q1dXd4L1IvUzl1dGk2dVN2WXI4eExSM09BZHZCb0JSd1IydU9DQjM3eWlpS0pma001eUZsWnNjeUJYaGQrRkJlRTYwQ3EybjQrSXlPM0VOQWlUSFVUSUdaL2taMnBieGdBcSt0a1RkUDBKS2tDWUZWdW12aGwvWWlINFZRPQ=="}
    }
}

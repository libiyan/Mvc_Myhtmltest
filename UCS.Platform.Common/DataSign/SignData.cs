using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace UCS.Platform.Common.DataSign
{
    public class SignData : AbsDataSign
    {
        public SignData(string sourceId)
            : base(sourceId)
        {
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="strSrcData"></param>
        /// <returns></returns>
        public string Sign(string strSrcData)
        {
            try
            {
                //取HashCode
                string strHashCode = strSrcData;
                //转成数组
                byte[] arrData = Encoding.UTF8.GetBytes(strHashCode);
                //签名
                var objx5092 = new X509Certificate2(dataSignModel.PfxPath, dataSignModel.SignPassword, X509KeyStorageFlags.MachineKeySet);
                var rsa = objx5092.PrivateKey as RSACryptoServiceProvider;
                byte[] arrSignData = rsa.SignData(arrData, new SHA1CryptoServiceProvider());
                return System.Convert.ToBase64String(arrSignData);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        ///// <summary>
        ///// 数据签名
        ///// </summary>
        ///// <param name="strSrcData"></param>
        ///// <returns></returns>
        //public string Sign(string strSrcData)
        //{
        //    try
        //    {
        //        //取HashCode
        //        string strHashCode = strSrcData;
        //        //转成数组
        //        //byte[] arrData = Encoding.UTF8.GetBytes(strHashCode);

        //        byte[] arrData = Encoding.UTF8.GetBytes(strSrcData);
        //        //签名
        //        TxtLoger.SavaLogToTXT("转成数组 arrData后为：" + arrData.Length, "a");
        //        var objx5092 = new X509Certificate2(dataSignModel.PfxPath, dataSignModel.SignPassword, X509KeyStorageFlags.MachineKeySet);
        //        var rsa = objx5092.PrivateKey as RSACryptoServiceProvider;
        //        byte[] arrSignData = rsa.SignData(arrData, new SHA1CryptoServiceProvider());

        //        TxtLoger.SavaLogToTXT("根据hash 转成数组arrSignData 后为：" + arrSignData.Length, "a");
        //        string aa = System.Convert.ToBase64String(arrSignData);

        //        TxtLoger.SavaLogToTXT("签名后的数据 是：" + aa, "a");
        //        return aa;
        //        //return System.Convert.ToBase64String(arrSignData);
        //    }
        //    catch (Exception ex)
        //    {
        //        TxtLoger.SavaLogToTXT("发生异常：" + ex.Message, "a");

        //        return "";
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace UCS.Platform.Common.DataSign
{
    public class VerifySignData : AbsDataSign
    {
        public VerifySignData(string sourceId)
            : base(sourceId)
        {

        }

        /// <summary>
        /// 数据验签
        /// </summary>
        /// <param name="strSrcData"></param>
        /// <param name="strSignData"></param>
        /// <returns></returns>
        public bool VerifySign(string strSrcData, string strSignData)
        {
            switch (dataSignModel.VerifySignFileType)
            {
                case VerifySignFileType.Pem:
                    return VerifySignPem(strSrcData, strSignData);
                case VerifySignFileType.Base64:
                    return VerifyDataBase64(strSrcData, strSignData);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Pem数据验签
        /// </summary>
        /// <param name="strSrcData"></param>
        /// <param name="strSignData"></param>
        /// <returns></returns>
        private bool VerifySignPem(string strSrcData, string strSignData)
        {
            byte[] Data = Encoding.UTF8.GetBytes(strSrcData);
            byte[] SignData = Convert.FromBase64String(strSignData);
            string publicKey = LoadCerContent(dataSignModel.CerPath);
            if (string.IsNullOrEmpty(publicKey)) return false;
            SHA1 sh = new SHA1CryptoServiceProvider();
            var paraPub = ConvertFromPemPublicKey(publicKey);
            var rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            var result = rsaPub.VerifyData(Data, sh, SignData);
            return result;
        }

        /// <summary>
        /// Base64数据验签
        /// </summary>
        /// <param name="source"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifyDataBase64(string source, string signature)
        {
            try
            {
                byte[] hash = Utils.CommonHelper.Hash(source);
                var x509Certificate = new X509Certificate2(dataSignModel.CerPath);
                string xmlString = x509Certificate.PublicKey.Key.ToXmlString(false);
                var key = new RSACryptoServiceProvider();
                key.FromXmlString(xmlString);
                var signatureDeformatter = new RSAPKCS1SignatureDeformatter(key);
                signatureDeformatter.SetHashAlgorithm("SHA1");
                return signatureDeformatter.VerifySignature(hash, Convert.FromBase64String(signature));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 将pem格式公钥(1024 or 2048)转换为RSAParameters
        /// </summary>
        /// <param name="pemFileConent">pem公钥内容</param>
        /// <returns>转换得到的RSAParamenters</returns>
        private static RSAParameters ConvertFromPemPublicKey(string pemFileConent)
        {
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            pemFileConent = pemFileConent.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            bool keySize1024 = (keyData.Length == 162);
            bool keySize2048 = (keyData.Length == 294);
            if (!(keySize1024 || keySize2048))
            {
                throw new ArgumentException("pem file content is incorrect, Only support the key size is 1024 or 2048");
            }
            byte[] pemModulus = (keySize1024 ? new byte[128] : new byte[256]);
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, (keySize1024 ? 29 : 33), pemModulus, 0, (keySize1024 ? 128 : 256));
            Array.Copy(keyData, (keySize1024 ? 159 : 291), pemPublicExponent, 0, 3);
            var para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        /// <summary>
        /// 加载证书内容
        /// </summary>
        /// <param name="strCerFilePath"></param>
        /// <returns></returns>
        private string LoadCerContent(string strCerFilePath)
        {
            try
            {
                string CerFileContent = System.IO.File.ReadAllText(strCerFilePath);
                return CerFileContent;
            }
            catch { return null; }
        }
    }
}

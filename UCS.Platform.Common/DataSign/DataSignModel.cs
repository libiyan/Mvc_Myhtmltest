using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UCS.Platform.Common.DataSign
{
    public class DataSignModel
    {
        /// <summary>
        /// 代销机构号
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 代销机构证书类型
        /// </summary>
        public VerifySignFileType VerifySignFileType { get; set; }

        /// <summary>
        /// 签名证书路径
        /// </summary>
        public string PfxPath { get; set; }

        /// <summary>
        /// 验签证书路径
        /// </summary>
        public string CerPath { get; set; }

        /// <summary>
        /// 签名证书密码
        /// </summary>
        public string SignPassword { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string Message { get; set; }

    }
}

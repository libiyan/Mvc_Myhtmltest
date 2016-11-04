using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleJob.Logic
{/// <summary>
    /// 1006-支付状态变更通知 （众金平台发起）
    /// </summary>
    public class NoticeRequest1006
    {
        /// <summary>
        /// 接入机构编码
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 支付交易流水号
        /// </summary>
        public string PaymentNo { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 支付状态 0=未支付 1=已支付
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 完成支付的时间 yyyyMMddHHmmss（此时间为银行通知的时间）
        /// </summary>
        public string PayTime { get; set; }
        /// <summary>
        /// 绑卡状态 0=绑卡失败 1=绑卡成功 2=绑卡处理中 用户第一次绑卡成功后，会带这个值；后面再次支付，可无需这个值。也即是：如果不带此值，默认就是绑卡成功的。
        /// </summary>
        public int? BandCODFlag { get; set; }
    }
}

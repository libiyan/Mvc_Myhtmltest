using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleJob.Logic
{
    /// <summary>
    /// 1006-支付状态变更通知  （众金平台发起）
    /// </summary>
    public class NoticeResponse1006
    {
        /// <summary>
        /// 参见附录-『响应码』
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

    }
}

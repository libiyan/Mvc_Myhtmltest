using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UCS.Platform.Common
{
    public class EnumPlatform
    {
        public enum EResponseCode
        {
            成功受理了请求 = 100,
            系统内部错误 = 101,
            验证签名失败 = 102,
            解析报文错误 = 103,
            报文格式错误 = 104,
            无效交易类型 = 105,
            无效日期 = 106,
            参数不正确 = 107,
            交易证书过期 = 108,
            不存在此机构 = 1000,
            不存在此订单 = 1001,
            不存在此支付交易 = 1002,
            订单号长度不正确 = 1003,
            支付交易流水号长度不正确 = 1004,
            支付交易流水号重复 = 1005,
            账户类型错误 = 1006,
            金额格式不对 = 1007,
            备注过长 = 1008,
            金额必须大于0 = 1009,
            银行账户信息不完整 = 1010,
            账户名称与账户号码不匹配 = 1011,
            交易流水号为空 = 1012,
            不存在此项目 = 1013
        }

        /// <summary>
        /// 投融资平台
        /// </summary>
        public enum FCode
        {
            招商银行 = 101046,
            兰州银行 = 101174,
            青岛银行 = 101175
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public enum ProjectType
        {
            普通项目 = 0,
            专属项目 = 1,
        }

        /// <summary>
        /// 项目承兑类型
        /// </summary>
        public enum FPrjType
        {
            信用证 = 0,
            票据 = 1
        }

        /// <summary>
        /// 项目支付渠道
        /// </summary>
        public enum FPayType
        {
            中金 = 0,
            银联 = 1
        }

        /// <summary>
        /// 订单来源
        /// </summary>
        public enum OrderFrom
        {
            投融资平台 = 0,
            众金平台 = 1,
        }

        /// <summary>
        /// 订单支付状态
        /// </summary>
        public enum OrderStatus
        {
            未支付 = 0,
            支付成功 = 1,
            支付失败 = 3,
            已作废 = 4,
            已退款 = 5,
        }

        /// <summary>
        /// 订单银联支付状态
        /// </summary>
        public enum OrderUnionpayStatus
        {
            支付确认中 = 10,
            支付成功 = 20,
            支付失败 = 30
        }

        /// <summary>
        /// 推送后台订单类型
        /// </summary>
        public enum PushOrderType
        {
            本金 = 1,
            融资款 = 2,
            平台服务费 = 3,
            利息 = 4,
            投资退款 = 5,
            承兑余款 = 6,
            信息技术费 = 7,
            支付代扣 = 10,
            保险费 = 12,
            票据托管费 = 13,
            支行手续费 = 14,
        }

        /// <summary>
        /// 指令请求回调状态
        /// </summary>
        public enum RequestExcStatus
        {
            放款中 = 0,
            结算成功 = 1,
            结算失败 = 2,
            结算异常 = 3
        }

        /// <summary>
        /// 银行卡类型
        /// </summary>
        public enum AccountType
        {
            个人 = 11,
            企业 = 12,
        }

        /// <summary>
        /// 回款账户类型
        /// </summary>
        public enum BankAccountType
        {
            融资者用 = 0,
            投资者用 = 1,
        }

        /// <summary>
        /// 项目查询状态编码
        /// </summary>
        public enum ProjectStatus
        {
            已完成项目 = 0,
            未完成项目 = 1,
            不限 = 2
        }
        /// <summary>
        /// 项目查询模式编码
        /// </summary>
        public enum QueryMode
        {
            发布时间 = 0,
            利率大小 = 1,
            金额大小 = 2
        }

        /// <summary>
        /// 项目查询排序模式
        /// </summary>
        public enum SortMode
        {
            降序 = 0,
            升序 = 1
        }

        /// <summary>
        /// 资产状态
        /// </summary>
        public enum OrderAssetsStatus
        {
            回收中 = 1,
            已投票待起息 = 2,
            回收完成 = 3,
            已逾期 = 4,
            投标失败 = 5
        }
    }
}

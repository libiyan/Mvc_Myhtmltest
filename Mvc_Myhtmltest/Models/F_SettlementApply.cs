//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Mvc_Myhtmltest.Models
{
    public partial class F_SettlementApply
    {
        public string SettlementApplyID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public string BankInfo { get; set; }
        public int PaymentCount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string SettlementProductID { get; set; }
        public System.DateTime ApplyDate { get; set; }
        public decimal DepositBalance { get; set; }
        public int IsPersonal { get; set; }
        public int IsInterbank { get; set; }
        public int FeedbackState { get; set; }
    
        public virtual F_SettlementProduct F_SettlementProduct { get; set; }
    }
    
}

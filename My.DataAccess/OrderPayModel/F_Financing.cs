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

namespace My.DataAccess.OrderPayModel
{
    public partial class F_Financing
    {
        public F_Financing()
        {
            this.F_Guarantee = new HashSet<F_Guarantee>();
            this.F_Investment_Order = new HashSet<F_Investment_Order>();
            this.F_Investment_WhiteList = new HashSet<F_Investment_WhiteList>();
            this.F_Project_Content = new HashSet<F_Project_Content>();
        }
    
        public string FinancingId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public decimal Amount { get; set; }
        public int PartsCount { get; set; }
        public int MinInvestPartsCount { get; set; }
        public decimal Progress { get; set; }
        public decimal BankInterest { get; set; }
        public decimal InvestmentInterest { get; set; }
        public decimal YMInterest { get; set; }
        public int Duration { get; set; }
        public int RepayType { get; set; }
        public int RepaySourceType { get; set; }
        public System.DateTime ValueBegin { get; set; }
        public System.DateTime RepayBegin { get; set; }
        public System.DateTime ProjectBeginTime { get; set; }
        public System.DateTime ReadyBeginTime { get; set; }
        public System.DateTime JMBeginTime { get; set; }
        public System.DateTime ToFinancingTime { get; set; }
        public Nullable<System.DateTime> ActualToFinancingTime { get; set; }
        public System.DateTime ToInvestmentTime { get; set; }
        public Nullable<System.DateTime> ActualToInvestmentTime { get; set; }
        public int ProjectStatus { get; set; }
        public int CreditLevel { get; set; }
        public string BindUserId { get; set; }
        public string BindCompanyId { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string CreateCompanyId { get; set; }
        public string CreateCompanyName { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateUserId { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public string BindUserName { get; set; }
        public string BindCompanyName { get; set; }
        public string AreaCode { get; set; }
        public string Year { get; set; }
        public int YearIndex { get; set; }
        public int ProjectType { get; set; }
        public int SettlementType { get; set; }
        public bool IsShow { get; set; }
        public bool IsExclusivePublic { get; set; }
        public string ExclusiveCode { get; set; }
        public Nullable<bool> IsVIP { get; set; }
    
        public virtual ICollection<F_Guarantee> F_Guarantee { get; set; }
        public virtual ICollection<F_Investment_Order> F_Investment_Order { get; set; }
        public virtual ICollection<F_Investment_WhiteList> F_Investment_WhiteList { get; set; }
        public virtual ICollection<F_Project_Content> F_Project_Content { get; set; }
    }
    
}

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
    public partial class F_UserActivity
    {
        public string UserActivityId { get; set; }
        public string LoginName { get; set; }
        public string UserId { get; set; }
        public string ActivityName { get; set; }
        public string PrizeName { get; set; }
        public string PrizeCode { get; set; }
        public string PrizePassword { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UseTimeBegin { get; set; }
        public Nullable<System.DateTime> UseTimeEnd { get; set; }
        public Nullable<System.DateTime> ReceiveTime { get; set; }
    }
    
}
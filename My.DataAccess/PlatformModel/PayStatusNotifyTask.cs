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

namespace My.DataAccess.PlatformModel
{
    public partial class PayStatusNotifyTask
    {
        public System.Guid TaskID { get; set; }
        public string SourceID { get; set; }
        public string PaymentNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public Nullable<bool> TaskStatus { get; set; }
        public Nullable<int> TaskNum { get; set; }
        public Nullable<bool> DelFlag { get; set; }
        public Nullable<int> BandCODFlag { get; set; }
    }
    
}
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
    public partial class F_Project_Content
    {
        public string Project_ContentId { get; set; }
        public string FinancingId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    
        public virtual F_Financing F_Financing { get; set; }
    }
    
}
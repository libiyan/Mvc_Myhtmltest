﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace My.DataAccess.OrderPayModel
{
    public partial class OrderPayEntities : DbContext
    {
        public OrderPayEntities()
            : base("name=OrderPayEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<F_BankAccount> F_BankAccount { get; set; }
        public DbSet<F_Financing> F_Financing { get; set; }
        public DbSet<F_Guarantee> F_Guarantee { get; set; }
        public DbSet<F_Income> F_Income { get; set; }
        public DbSet<F_Investment_Order> F_Investment_Order { get; set; }
        public DbSet<F_Investment_Order_Push> F_Investment_Order_Push { get; set; }
        public DbSet<F_Investment_WhiteList> F_Investment_WhiteList { get; set; }
        public DbSet<F_Order_CmdRequest> F_Order_CmdRequest { get; set; }
        public DbSet<F_Order_RunningNO> F_Order_RunningNO { get; set; }
        public DbSet<F_Order_ServiceLog> F_Order_ServiceLog { get; set; }
        public DbSet<F_Project_Content> F_Project_Content { get; set; }
        public DbSet<F_Repay_OrderDetail> F_Repay_OrderDetail { get; set; }
        public DbSet<V_F_Investment_Order_Push> V_F_Investment_Order_Push { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardChequeModule.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OCCEVENTLOG
    {
        public long ID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string Remarks { get; set; }
        public Nullable<long> UserID { get; set; }
    
        public virtual OCCUSER OCCUSER { get; set; }
    }
}

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
    
    public partial class OCCENUMERATION
    {
        public OCCENUMERATION()
        {
            this.CARDCHEREUISITION = new HashSet<CARDCHEREUISITION>();
            this.CARDCHLEAF = new HashSet<CARDCHLEAF>();
            this.CARDCHTRAN = new HashSet<CARDCHTRAN>();
            this.OCCUSER = new HashSet<OCCUSER>();
        }
    
        public long ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ICollection<CARDCHEREUISITION> CARDCHEREUISITION { get; set; }
        public virtual ICollection<CARDCHLEAF> CARDCHLEAF { get; set; }
        public virtual ICollection<CARDCHTRAN> CARDCHTRAN { get; set; }
        public virtual ICollection<OCCUSER> OCCUSER { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using CardChequeModule.Models.MetaData;

namespace CardChequeModule.Models
{
    using System;
    using System.Collections.Generic;

    [MetadataType(typeof(BranchMeta))]
    public partial class BRANCHINFO
    {
        public BRANCHINFO()
        {
            this.CARDCHEREUISITION = new HashSet<CARDCHEREUISITION>();
            this.CARDCHTRAN = new HashSet<CARDCHTRAN>();
            this.CARDCHTRAN1 = new HashSet<CARDCHTRAN>();
            this.DEPOSIT = new HashSet<DEPOSIT>();
            this.OCCUSER = new HashSet<OCCUSER>();
        }
    
        public long ID { get; set; }
        public string BRANCHCODE { get; set; }
        public string BRANCHNAME { get; set; }
    
        public virtual ICollection<CARDCHEREUISITION> CARDCHEREUISITION { get; set; }
        public virtual ICollection<CARDCHTRAN> CARDCHTRAN { get; set; }
        public virtual ICollection<CARDCHTRAN> CARDCHTRAN1 { get; set; }
        public virtual ICollection<DEPOSIT> DEPOSIT { get; set; }
        public virtual ICollection<OCCUSER> OCCUSER { get; set; }
    }
}

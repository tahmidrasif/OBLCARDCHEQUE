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
    
    public partial class BRANCHINFO
    {
        public BRANCHINFO()
        {
            this.CARDCHEREUISITION = new HashSet<CARDCHEREUISITION>();
            this.DEPOSIT = new HashSet<DEPOSIT>();
        }
    
        public long ID { get; set; }
        public Nullable<int> TOTALEMPLOYEE { get; set; }
        public string BRANCHCODE { get; set; }
        public string BRANCHNAME { get; set; }
        public string BRANCHMANAGER { get; set; }
        public string BRANCHCONTACT { get; set; }
        public string BRANCHMAIL { get; set; }
        public string BRANCHADDRESS { get; set; }
    
        public virtual ICollection<CARDCHEREUISITION> CARDCHEREUISITION { get; set; }
        public virtual ICollection<DEPOSIT> DEPOSIT { get; set; }
    }
}
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
    
    public partial class ImageTable
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string CardNumber { get; set; }
        public byte[] Signature { get; set; }
        public Nullable<decimal> Outstanding { get; set; }
    }
}

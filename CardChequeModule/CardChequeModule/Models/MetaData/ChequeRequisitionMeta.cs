using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardChequeModule.Models.MetaData
{
    public class ChequeRequisitionMeta
    {
        public long ID { get; set; }

        [Required]
        [DisplayName("Card Number")]
        public string CARDNO { get; set; }

        [Required(ErrorMessage = "Enter the branch")]
        [DisplayName("Branch")]
        public long BRANCHCODE { get; set; }

        [DisplayName("Status")]
        public long STATUS { get; set; }       
        public int LEAFNO { get; set; }

        [DisplayName("Leaf From")]
        public string LEAFFROM { get; set; }

        [DisplayName("Leaf To")]
        public string LEAFTO { get; set; }
        public Nullable<long> LEAFNEXT { get; set; }

        [DisplayName("Remarks")]
        public string REMARKS { get; set; }

        [DisplayName("Is Active?")]
        public bool ISACTIVE { get; set; }

        [DisplayName("User Name")]
        public long CREATEDBY { get; set; }

        [DisplayName("Apply Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required]
        public System.DateTime CREATEDON { get; set; }
        public string MODIFIEDBY { get; set; }

        [DisplayName("Modified Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> MODIFIEDON { get; set; }

        [DisplayName("Reference Number")]
        public string REFERENCENO { get; set; }

        [DisplayName("Authorized By")]
        public Nullable<long> AUTHORIZEDBY { get; set; }

        [DisplayName("Authorized Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> AUTHORIZEDON { get; set; }

        [DisplayName("Downloaded By")]
        public Nullable<long> ESTABLISHMENTBY { get; set; }

        [DisplayName("Download Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> ESTABLISHMENTON { get; set; }

        [DisplayName("Activaed By")]
        public Nullable<long> CALLCENTERBY { get; set; }

        [DisplayName("Activation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CALLCENTERON { get; set; }

        [DisplayName("Delivered By")]
        public Nullable<long> DELIVEREDBY { get; set; }

        [DisplayName("Delivered Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DELIVEREDON { get; set; }
    

    }
}
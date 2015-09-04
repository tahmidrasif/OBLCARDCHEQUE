using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardChequeModule.Models.MetaData
{

    
    public class CardChLeafMeta
    {
        [DisplayName("Cheque Id")]
        public long CHEQUEID { get; set; }

        [DisplayName("Leaf Number")]
        public string LEAFNO { get; set; }

        [DisplayName("Leaf Status")]
        public long LEAFSTATUS { get; set; }

        [DisplayName("Remarks")]
        public string REMARKS { get; set; }

        [DisplayName("Is Active?")]
        public bool ISACTIVE { get; set; }

        [DisplayName("Created By")]

        public long CREATEDBY { get; set; }

        [DisplayName("Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime CREATEDON { get; set; }

         [DisplayName("Modified By")]
        public Nullable<long> MODIFIEDBY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> MODIFIEDON { get; set; }
    }
}
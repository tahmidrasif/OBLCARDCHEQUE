using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardChequeModule.Models.MetaData
{
    public class CardChTranMeta
    {
        [DisplayName("Card Number")]
        public string CARDNO { get; set; }

         [DisplayName("Card Holder's Name")]
        public string CARDHOLDERNAME { get; set; }

        [DisplayName("Branch Code")]
        public Nullable<long> BRANCHCODE { get; set; }

        [DisplayName("Request Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime REQUESTDATE { get; set; }

        [DisplayName("Beneficiary Name")]
        public string BENEFICIARINFO { get; set; }

        [DisplayName("Amount")]
        public decimal AMOUNT { get; set; }

        [DisplayName("Is Signature Valid")]
        public bool ISSIGNATUREVERIFIED { get; set; }

        [DisplayName("Status")]
        public long STATUS { get; set; }

        [DisplayName("Approval Number")]
        public string APPROVALNO { get; set; }

        [DisplayName("Is Active?")]
        public bool ISACTIVE { get; set; }

        [DisplayName("Created By")]
        public Nullable<long> CREATEDBY { get; set; }

        [DisplayName("Created Date")]
        public System.DateTime CREATEDON { get; set; }

        [DisplayName("Modified By")]
        public Nullable<long> MODIFIEDBY { get; set; }

        [DisplayName("Modified Date")]
        public Nullable<System.DateTime> MODIFIEDON { get; set; }
    }
}
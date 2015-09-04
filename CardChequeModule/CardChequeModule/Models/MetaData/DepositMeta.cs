using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CardChequeModule.Models.MetaData
{
    
    public class DepositMeta
    {
        public long ID { get; set; }

        [Required]
        [DisplayName("Card Number")]
        public string CARDNUMBER { get; set; }

        [DisplayName("Card Holder's Name")]
        public string CARDHOLDERNAME { get; set; }

        [Required(ErrorMessage = "Enter Date")]
        [DisplayName("Deposit Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CREATEDON { get; set; }

        [Required(ErrorMessage = "Enter the branch")]
        [DisplayName("Branch")]
        public Nullable<long> BRANCH { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Number Only")]
     //   [Required(ErrorMessage = "Enter the amount in number")]
        [DisplayName("Amount")]
        public Nullable<decimal> AMOUNT { get; set; }

        [DisplayName("Reference Number")]
        public string REFERENCENUMBER { get; set; }
    }
}
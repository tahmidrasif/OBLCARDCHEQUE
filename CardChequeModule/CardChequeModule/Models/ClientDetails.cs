using System.ComponentModel;

namespace CardChequeModule.Models
{
    public class ClientDetails
    {
        public int Id { get; set; }
        [DisplayName("Client Name")]
        public string CLIENTNAME { get; set; }

        [DisplayName("Address")]
        public string ADDRESS { get; set; }
        //public string STADDRESS { get; set; }
        //public string REGION { get; set; }

        //[DisplayName("Country")]
        //public string COUNTRY { get; set; }
        //public string ZIP { get; set; }

        //[DisplayName("Billing Date")]
        //public string BILLINGDATE { get; set; }
        //public int CREATEDBY { get; set; }
        //public System.DateTime CRATEDON { get; set; }
        //public string STATEMENTDATE { get; set; }
        //public string PAYMENTDATE { get; set; }
        public string CARDNO { get; set; }
        //public string CURRENCY { get; set; }

        [DisplayName("Minimum Due")]
        public decimal MINAMNTDUE { get; set; }

        [DisplayName("Current Balance")]
        public decimal CURRBALANCE { get; set; }

        [DisplayName("Total Purchase")]
        public decimal TOTALPURCHASE { get; set; }

        [DisplayName("Credit Limit")]
        public decimal CREDITLIMIT { get; set; }
    }
}
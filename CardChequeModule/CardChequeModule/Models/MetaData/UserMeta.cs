using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardChequeModule.Models.MetaData
{
    public class UserMeta
    {
        [DisplayName("Employee Id")]
        public string EMPLOYEEID { get; set; }

        [DisplayName("User Name")]
        public string USERNAME { get; set; }

        public Nullable<long> BRANCH { get; set; }

        [DisplayName("Is Active?")]
        public Nullable<bool> ISACTIVE { get; set; }
    
        [DisplayName("Created By")]
        public Nullable<long> CREATEDBY { get; set; }

        [DisplayName("Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CREATEDON { get; set; }

        [DisplayName("Modfied By")]
        public Nullable<long> MODIFIEDBY { get; set; }

        [DisplayName("Modified On")]
        public Nullable<System.DateTime> MODIFIEDON { get; set; }

        [DisplayName("Role")]
        public Nullable<long> TYPE { get; set; }
    

    }
}
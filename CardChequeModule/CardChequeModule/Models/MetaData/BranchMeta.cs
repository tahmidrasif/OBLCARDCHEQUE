using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CardChequeModule.Models.MetaData
{
    public class BranchMeta
    {
        [DisplayName("Branch Code")]
        public string BRANCHCODE { get; set; }

        [DisplayName("Branch Name")]
        public string BRANCHNAME { get; set; }

    }
}
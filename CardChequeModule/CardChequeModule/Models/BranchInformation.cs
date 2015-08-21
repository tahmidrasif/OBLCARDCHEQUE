using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CardChequeModule.WebRef;

namespace CardChequeModule.Models
{
    public class BranchInformation
    {
        WebRef.OBLAPP obluser=new WebRef.OBLAPP();

        public void GetAllBranch()
        {
            var a=obluser.GetGlobalAllBranch();
        }
    }
}
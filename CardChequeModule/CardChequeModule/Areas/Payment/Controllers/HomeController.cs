using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using Microsoft.Ajax.Utilities;

namespace CardChequeModule.Areas.Payment.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class HomeController : Controller
    {
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        private OCCUSER user;
        //
        // GET: /Payment/Home/
        public ActionResult Index()
        {
            return View();
        }

	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;

namespace CardChequeModule.Areas.Payment.Controllers
{
    public class ListController : Controller
    {
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        private OCCUSER user;
        //
        // GET: /Payment/List/
        public ActionResult Index()
        {
            user = (OCCUSER) Session["User"];
            try
            {
                var list = db.DEPOSIT.Where(x => x.CREATEDBY == user.ID).ToList();
                return View(list);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new {Area = ""});
            }     
            
        }

    }
}
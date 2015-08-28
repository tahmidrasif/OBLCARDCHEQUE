using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Models;
using PagedList;

namespace CardChequeModule.Areas.Payment.Controllers
{
    [Authorize(Roles = "teller,admin")]
    public class ListController : Controller
    {
        OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        private OCCUSER user;
        //
        // GET: /Payment/List/
        public ActionResult Index(int?BRANCH,string CARDNO,DateTime? CREATEDON,int? page,int? serial)
        {
            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = (page ?? 1);

            user = (OCCUSER) Session["User"];
            try
            {
                var list = db.DEPOSIT.Where(x => x.CREATEDBY == user.ID).OrderByDescending(x=>x.ID).ToList();
                var branch = db.BRANCHINFO.Select(x => new {x.BRANCHNAME, x.ID}).OrderBy(x=>x.BRANCHNAME);
                ViewBag.BRANCH = new SelectList(branch, "ID", "BRANCHNAME");

                if (serial != null)
                {
                    @ViewBag.Sln = (pageNumber*pageSize)-9;
                }
                else
                {
                    @ViewBag.Sln = 1;
                }

                if (BRANCH != null)
                {
                    list = list.Where(x => x.BRANCH == BRANCH).ToList();
                    ViewBag.BRANCH = new SelectList(branch, "ID", "BRANCHNAME",BRANCH);
                    ViewBag.currentBranch = BRANCH;
                }
                if (!String.IsNullOrEmpty(CARDNO))
                {
                    CARDNO = CARDNO.Trim();
                    list = list.Where(x => x.CARDNUMBER == CARDNO).ToList();
                    ViewBag.CARDNO = CARDNO;
                }
                if (CREATEDON != null)
                {
                    list = list.Where(x => x.CREATEDON == CREATEDON).ToList();
                    ViewBag.CREATEDON = CREATEDON;
                }
                
                return View(list.ToPagedList(pageNumber, pageSize));
               // return View(list);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new {Area = ""});
            }     
            
        }

    }
}
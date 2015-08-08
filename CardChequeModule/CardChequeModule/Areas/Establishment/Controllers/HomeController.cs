using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.Establishment.Models;
using CardChequeModule.Models;
using CardChequeModule.WebReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardChequeModule.Areas.Establishment.Controllers
{
    public class HomeController : Controller
    {
        private JsonSerializer serializer = new JsonSerializer();
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        // GET: /Establishment/Home/
        public ActionResult Index()
        {
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            var cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x=>x.STATUS==5);
            return View(cardchereuisition.ToList());
        }

        [HttpPost]
        public ActionResult Post(IEnumerable<long> idList)
        {
            //DownloadFile();
            List<CARDCHEREUISITION> UpdatedList = new List<CARDCHEREUISITION>();

            OCCUSER user = (OCCUSER) Session["User"];
            try
            {

                foreach (var id in idList)
                {
                    var cheueReq = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheueReq.STATUS = 7;
                    cheueReq.MODIFIEDBY = user.ID;
                    cheueReq.MODIFIEDON = DateTime.Now.Date;
                    UpdatedList.Add(cheueReq);
                }

                foreach (var editedChqRq in UpdatedList)
                {
                    db.Entry(editedChqRq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                // var authorizedList = db.CARDCHEREUISITION.Where(x => x.STATUS == 4).ToList();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new {Area = ""});
            }
        }

        //private void DownloadFile()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Card Number");
        //    dt.Columns.Add("Card Holder Name");
        //    dt.Columns.Add("Branch Name");
        //    dt.Columns.Add("Leaves Number");
        //    dt.Columns.Add("RTN");
        //    dt.Columns.Add("TN");
        //    List<DownloadViewModel> models=new List<DownloadViewModel>();
        //    var List=db.CARDCHEREUISITION.Where(x => x.STATUS == 7);
        //    foreach (var cardchereuisition in List)
        //    {
        //        var row = dt.NewRow();
        //        DownloadViewModel model=new DownloadViewModel();
        //        WebReference.CCMService service=new CCMService();
        //        var details=service.GetClientDetails(cardchereuisition.CARDNO);

        //        if (details != "null")
        //        {
        //            JObject json = JObject.Parse(details);
        //            ClientDetails clientDetails = (ClientDetails)serializer.Deserialize(new JTokenReader(json), typeof(ClientDetails));
        //            row["Card Holder Name"] = clientDetails.CLIENTNAME;
        //        }
        //        row["Card Number"] = cardchereuisition.CARDNO;
        //        row["Branch Name"] = cardchereuisition.BRANCHINFO.BRANCHNAME;
        //        row["Leaves Number"] = "1/" + cardchereuisition.LEAFNO;
        //        row["RTN"] = "Default";
        //        row["TN"] = 10;

        //    }
        //}

        [HttpPost]
        public PartialViewResult SearchResult(string CARDNO, DateTime? CREATEDON, int? BRANCH)
        {
            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x=>x.STATUS==5).ToList();
            
            if (!String.IsNullOrEmpty(CARDNO))
            {
                List = List.Where(x => x.CARDNO == CARDNO).ToList();
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.MODIFIEDON == CREATEDON).ToList();
            }
            if (BRANCH != null)
            {
                List = List.Where(x => x.BRANCHCODE == BRANCH).ToList();
            }

            return PartialView("_AuthorizedList", List);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

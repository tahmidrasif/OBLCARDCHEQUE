using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CardChequeModule.Models;
//using CardChequeModule.WebReference;
using Newtonsoft.Json;
//using Microsoft.Office.Interop.Excel;
// InternalWorkbook
// HSSFWorkbook, HSSFSheet
using NPOI.XSSF.UserModel;
using PagedList;


namespace CardChequeModule.Areas.Establishment.Controllers
{
    [Authorize(Roles = "establishment,admin")]
    public class HomeController : Controller
    {
        private JsonSerializer serializer = new JsonSerializer();
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();

        //private Application _application;
        //private Workbook _workbook;
        //private Worksheet _worksheet;


        XSSFWorkbook wb;
        XSSFSheet sh;


        // GET: /Establishment/Home/

        public ActionResult Index(string CARDNO, DateTime? CREATEDON, int? BRANCH, int? page)
        {
            try
            {
                ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
                var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 5).OrderByDescending(x => x.ID).ToList();

                if (!String.IsNullOrEmpty(CARDNO))
                {
                    CARDNO = CARDNO.Trim();
                    List = List.Where(x => x.CARDNO.Contains(CARDNO)).ToList();
                    ViewBag.CARDNO = CARDNO;
                }
                if (CREATEDON != null)
                {
                    List = List.Where(x => x.AUTHORIZEDON.Value.Date == CREATEDON.Value.Date).ToList();
                    ViewBag.CREATEDON = CREATEDON;
                }
                if (BRANCH != null)
                {
                    List = List.Where(x => x.BRANCHCODE == BRANCH).ToList();
                    ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME", BRANCH);
                    ViewBag.currbranch = BRANCH;
                }
                int pageSize = ConstantConfig.PageSizes;
                int pageNumber = ((page ?? 1));
                return View(List.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        [HttpPost]
        public ActionResult Post(IEnumerable<long> idList)
        {

            OCCUSER user = (OCCUSER)Session["User"];
            var aList = idList.OrderBy(x => x);
            try
            {
                foreach (var id in aList)
                {

                    var cheque = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    var statusId =
                        db.OCCENUMERATION.Where(x => x.Type == "chequereq")
                            .Where(x => x.Name == "received")
                            .Select(x => x.ID)
                            .FirstOrDefault();
                    cheque.STATUS = statusId; 
                    if (IsLeafNoStartsWithZero())
                    {
                        cheque.LEAFFROM = "0000001";
                        cheque.LEAFTO = "0000010";
                        cheque.LEAFNEXT = 11;
                    }

                    else
                    {
                        var lastSavedchq = db.CARDCHEREUISITION.Where(x => x.ESTABLISHMENTBY!=null).OrderByDescending(x => x.LEAFNEXT).OrderByDescending(x=>x.ESTABLISHMENTON).FirstOrDefault();
                        var leafnext = (int)lastSavedchq.LEAFNEXT;
                        if (leafnext + 10 > 9999999)
                        {
                            cheque.LEAFFROM = "0000001";
                            cheque.LEAFTO = "0000010";
                            cheque.LEAFNEXT = 11;
                        }
                        else
                        {
                            string leafnum = leafnext.ToString(CultureInfo.InvariantCulture);

                            cheque.LEAFFROM = leafnum.PadLeft(7, '0');

                            leafnext += 9;
                            leafnum = leafnext.ToString(CultureInfo.InvariantCulture);

                            cheque.LEAFTO = leafnum.PadLeft(7, '0');

                            leafnext++;

                            cheque.LEAFNEXT = leafnext;
                        }
                    }
                    cheque.ESTABLISHMENTBY = user.ID;
                    cheque.ESTABLISHMENTON = DateTime.Now;
                    db.Entry(cheque).State = EntityState.Modified;
                    db.SaveChanges();

                }
                DownloadFile(idList);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }

        private bool IsLeafNoStartsWithZero()
        {
            var downloadlist = db.CARDCHEREUISITION.Where(x=>x.ESTABLISHMENTBY!=null).ToList();
            var savelistcount = downloadlist.Count;
            if (savelistcount == 0)
            {
                return true;
            }
            return false;
        }



        public ActionResult DownloadFile(IEnumerable<long> idList)
        {
            try
            {
                wb = new XSSFWorkbook();
                sh = (XSSFSheet)wb.CreateSheet("Sheet1");
                List<CARDCHEREUISITION> list = new List<CARDCHEREUISITION>();
                var row = sh.CreateRow(0);

                row.CreateCell(0).SetCellValue("Bank");

                row.CreateCell(1).SetCellValue("Card No");

                row.CreateCell(2).SetCellValue("No of Books/Leaves");

                row.CreateCell(3).SetCellValue("Delivery Brn/Channel");

                row.CreateCell(4).SetCellValue("RoutingNo?");

                row.CreateCell(5).SetCellValue("Transaction Code");
                foreach (var id in idList)
                {
                    var aItem = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID==id);
                    list.Add(aItem);
                }
               // var list = db.CARDCHEREUISITION.Where(x => x.STATUS == 7).ToList();

                foreach (var card in list.Select((value, index) => new { value, index }))
                {
                    row = sh.CreateRow(card.index +1);

                    row.CreateCell(0).SetCellValue("OBL");
                    row.CreateCell(1).SetCellValue(card.value.CARDNO);
                    row.CreateCell(2).SetCellValue(card.value.LEAFNO);
                    row.CreateCell(3).SetCellValue(card.value.BRANCHINFO.BRANCHNAME);
                    row.CreateCell(4).SetCellValue("Test Routing");
                    row.CreateCell(5).SetCellValue("Test Transaction");
                }
                //using (var fs = new FileStream(@"~/test.xls", FileMode.Create, FileAccess.Write))
                //{
                //    wb.Write(fs);
                //}
                using (var exportData = new MemoryStream())
                {

                    wb.Write(exportData);
                    exportData.Close();

                    var buffer = exportData.GetBuffer();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(buffer);

                    Response.End();

                    //string saveAsFileName = string.Format("Cheque_Requisition{0:d}.xls", DateTime.Now).Replace("/", "-");
                    //Response.Clear();
                    ////Response.ContentType = "application/octet-stream";
                    //Response.ContentType = "application/vnd.ms-excel";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    //Response.BinaryWrite(exportData.GetBuffer());
                    //Response.End();

                }

                return RedirectToAction("Index");


            }
            catch (Exception exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }

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

//wb.Write(exportData);

//                   string saveAsFileName = string.Format("Cheque_Requisition{0:d}.xls", DateTime.Now).Replace("/", "-");


//                   Response.Clear();
//                   //Response.ContentType = "application/octet-stream";
//                   Response.ContentType = "application/vnd.ms-excel";

//                   Response.Cache.SetCacheability(HttpCacheability.NoCache);


//                   Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));



//                   Response.BinaryWrite(exportData.GetBuffer());

//                   Response.End();

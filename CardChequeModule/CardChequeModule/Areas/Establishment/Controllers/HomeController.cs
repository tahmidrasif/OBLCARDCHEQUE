using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardChequeModule.Areas.Establishment.Models;
using CardChequeModule.Models;
using CardChequeModule.WebReference;
using ExcelLibrary.SpreadSheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Microsoft.Office.Interop.Excel;

using NPOI.HSSF.Model; // InternalWorkbook
using NPOI.HSSF.UserModel; // HSSFWorkbook, HSSFSheet
using NPOI.XSSF.UserModel;


namespace CardChequeModule.Areas.Establishment.Controllers
{
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
        public ActionResult Index()
        {
            ViewBag.BRANCH = new SelectList(db.BRANCHINFO.ToList(), "ID", "BRANCHNAME");
            var cardchereuisition = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Include(c => c.OCCUSER1).Where(x => x.STATUS == 5);
            return View(cardchereuisition.ToList());
        }

        [HttpPost]
        public ActionResult Post(IEnumerable<long> idList)
        {
            //DownloadFile();
            List<CARDCHEREUISITION> UpdatedList = new List<CARDCHEREUISITION>();
            OCCUSER user = (OCCUSER)Session["User"];

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

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }

        public ActionResult DownloadFile()
        {

            try
            {

                wb = new XSSFWorkbook();
                sh = (XSSFSheet)wb.CreateSheet("Sheet1");

                var row = sh.CreateRow(1);

                row.CreateCell(0).SetCellValue("Bank");

                row.CreateCell(1).SetCellValue("Card No");

                row.CreateCell(2).SetCellValue("No of Books/Leaves");

                row.CreateCell(3).SetCellValue("Delivery Brn/Channel");

                row.CreateCell(4).SetCellValue("RoutingNo?");

                row.CreateCell(5).SetCellValue("Transaction Code");

                var list = db.CARDCHEREUISITION.Where(x => x.STATUS == 7).ToList();

                foreach (var card in list.Select((value, index) => new { value, index }))
                {
                    row = sh.CreateRow(card.index + 2);
                    row.CreateCell(0).SetCellValue(card.value.CARDNO);
                    row.CreateCell(1).SetCellValue("Test Name");
                    row.CreateCell(2).SetCellValue(card.value.LEAFNO);
                    row.CreateCell(3).SetCellValue(card.value.BRANCHINFO.BRANCHNAME);
                    row.CreateCell(4).SetCellValue("Test Routing");
                    row.CreateCell(5).SetCellValue("Test Transaction");


                }
                //using (var fs = new FileStream(@"C:\test.xls", FileMode.Create, FileAccess.Write))
                //{
                //    wb.Write(fs);
                //}
                using (var exportData = new MemoryStream())
                {

                    wb.Write(exportData);

                    string saveAsFileName = string.Format("MembershipExport-{0:d}.xls", DateTime.Now).Replace("/", "-");


                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);


                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));

                    

                    Response.BinaryWrite(exportData.GetBuffer());

                    Response.End();

                }

                return RedirectToAction("Index");


            }
            catch (Exception exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }

        }

        [HttpPost]
        public PartialViewResult SearchResult(string CARDNO, DateTime? CREATEDON, int? BRANCH)
        {
            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 5).ToList();

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


//public ActionResult DownloadFile()
//       {

//           try
//           {
//               _application = new Application();
//               _workbook = _application.Workbooks.Add();
//               _worksheet = (Microsoft.Office.Interop.Excel.Worksheet)_workbook.Worksheets.get_Item(1);
//               Microsoft.Office.Interop.Excel.Range cell = (Range)_worksheet.Cells;
//               cell.EntireColumn.NumberFormat = "@";

//               _worksheet.Cells[1, 1] = "Bank";
//               _worksheet.Cells[1, 2] = "Card No";
//               _worksheet.Cells[1, 3] = "Name";
//               _worksheet.Cells[1, 4] = "No of Books/Leaves";
//               _worksheet.Cells[1, 5] = "Delivery Brn/Channel";
//               _worksheet.Cells[1, 6] = "RoutingNo";
//               _worksheet.Cells[1, 7] = "Transaction Code";

//               var list = db.CARDCHEREUISITION.Where(x => x.STATUS == 7).ToList();

//               foreach (var card in list.Select((value, index) => new { value, index }))
//               {
//                   _worksheet.Cells[card.index + 2, 1] = "OBL";
//                   _worksheet.Cells[card.index + 2, 2] = card.value.CARDNO;
//                   _worksheet.Cells[card.index + 2, 3] = "Test Name";
//                   _worksheet.Cells[card.index + 2, 4] = card.value.LEAFNO;
//                   _worksheet.Cells[card.index + 2, 5] = card.value.BRANCHINFO.BRANCHNAME;
//                   _worksheet.Cells[card.index + 2, 6] = "Test Routing";
//                   _worksheet.Cells[card.index + 2, 7] = "Test Transaction";

//               }


//               _application.DisplayAlerts = false;
//               //string folderPath = Path.GetDirectoryName(@"E:\Temp\Cheque_Requistion_"+DateTime.Now.Date+".xls");
//               string filename = @"E:\Temp\Cheque_Requistion_" + DateTime.Now.ToShortDateString() + ".xls";
//               string targetFolder = Server.MapPath("~/Downloads");
//               if (!Directory.Exists(targetFolder))
//               {
//                   Directory.CreateDirectory(targetFolder);
//               }
//               string targetPath = Path.Combine(targetFolder, filename);


//               _workbook.SaveAs(targetPath);

//               _application.Visible = true;
//               _workbook.Activate();

//               return RedirectToAction("Index");
//           }
//           catch (Exception exception)
//           {

//               return RedirectToAction("Error", "Home", new { Area = "" });
//           }

//       }
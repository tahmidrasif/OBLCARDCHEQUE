﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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

            OCCUSER user = (OCCUSER)Session["User"];
           
            try
            {
                foreach (var id in idList)
                {

                    var cheque = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheque.STATUS = 7;
                    if (IsLeafNoStartsWithZero())
                    {
                        cheque.LEAFFROM = "0000001";
                        cheque.LEAFTO = "0000010";
                        cheque.LEAFNEXT = 11;
                    }
                   
                    else
                    {
                        var lastSavedchq = db.CARDCHEREUISITION.Where(x => x.STATUS == 7).OrderByDescending(x => x.MODIFIEDON).FirstOrDefault();
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
                    cheque.MODIFIEDBY = user.ID;
                    cheque.MODIFIEDON = DateTime.Now;
                    db.Entry(cheque).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }

        private bool IsLeafNoStartsWithZero()
        {
            var downloadlist = db.CARDCHEREUISITION.Where(x => x.STATUS == 7).ToList();
            var savelistcount = downloadlist.Count;
            if (savelistcount == 0)
            {
                return true;
            }
            return false;
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
                using (var fs = new FileStream(@"C:\test.xls", FileMode.Create, FileAccess.Write))
                {
                    wb.Write(fs);
                }
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

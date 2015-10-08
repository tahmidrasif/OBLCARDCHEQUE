using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using CardChequeModule.Areas.Admin.Models;
using CardChequeModule.Areas.Authorizer.Models;
using CardChequeModule.Models;
using PagedList;
using DataTable = Microsoft.Office.Interop.Excel.DataTable;

namespace CardChequeModule.Areas.Authorizer.Controllers
{
    [Authorize(Roles = "authorizer,admin")]
    public class HomeController : Controller
    {
        private OBLCARDCHEQUEEntities db = new OBLCARDCHEQUEEntities();
        public ActionResult Index()
        {
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                AuthorizerDashboardVM aVm = new AuthorizerDashboardVM();

                WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                System.Data.DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                foreach (DataRow dataRow in dt.Rows)
                {
                    aVm.EmployeeInfoVm.BranchName = (string)dataRow[21];
                    aVm.EmployeeInfoVm.Email = (string)dataRow[9];
                    aVm.EmployeeInfoVm.EmployeeId = (string)dataRow[2];
                    aVm.EmployeeInfoVm.JobTitle = (string)dataRow[7];
                    aVm.EmployeeInfoVm.Name = (string)dataRow[3];
                    aVm.EmployeeInfoVm.PreDeptName = (string)dataRow[17];
                }

                var pendingReqList = db.CARDCHEREUISITION.Where(x => x.ISDELETE != true).Where(x => x.STATUS == 4).OrderByDescending(x => x.ID).ToList();
                var pendingReqListCount = pendingReqList.Count;
                aVm.PendingRequisitionCount = pendingReqListCount;
                aVm.PendingRequisitionList = pendingReqList.Take(5).ToList();


                var pendingChequeList = db.CARDCHTRAN.Where(x=>x.STATUS==13).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                var pendingChequeListCount = pendingChequeList.Count;
                aVm.PendingCardChequeAmount = pendingChequeList.Sum(x => x.AMOUNT);
                aVm.PendingCardChequeCount = pendingChequeListCount;
                aVm.PendingCardChequeList = pendingChequeList.Take(5).ToList();

                var authReqList = db.CARDCHEREUISITION.Where(x=>x.ISDELETE!=true).Where(x => x.STATUS ==5).OrderByDescending(x => x.ID).ToList();
                var authReqListCount = authReqList.Count;
                aVm.AuthRequisitionCount = authReqListCount;
                aVm.AuthorizedRequisitionList = authReqList.Take(5).ToList();
                    

                var authChequeList = db.CARDCHTRAN.Where(x => x.STATUS == 14).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                var authChequeListCount = authChequeList.Count;
                aVm.AuthorizedCardChequeAmount = authChequeList.Sum(x => x.AMOUNT);
                aVm.AuthCardCHequeCount = authChequeListCount;
                aVm.AuthorizedCardChequeList = authChequeList.Take(5).ToList();

                return View(aVm);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home", new { Area = "" });
            }
           
           // return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime? startDate, DateTime? endDate, string btnName)
        {
            try
            {
                if (startDate != null || endDate != null)
                {
                    OCCUSER user = (OCCUSER)Session["User"];
                    AuthorizerDashboardVM aVm = new AuthorizerDashboardVM();

                    WebRef.OBLAPP oblApp = new WebRef.OBLAPP();
                    System.Data.DataTable dt = oblApp.GetByUserID(user.EMPLOYEEID);
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        aVm.EmployeeInfoVm.BranchName = (string)dataRow[21];
                        aVm.EmployeeInfoVm.Email = (string)dataRow[9];
                        aVm.EmployeeInfoVm.EmployeeId = (string)dataRow[2];
                        aVm.EmployeeInfoVm.JobTitle = (string)dataRow[7];
                        aVm.EmployeeInfoVm.Name = (string)dataRow[3];
                        aVm.EmployeeInfoVm.PreDeptName = (string)dataRow[17];
                    }

                    var pendingReqList = db.CARDCHEREUISITION.Where(x => x.ISDELETE == false).Where(x => x.STATUS == 4).OrderByDescending(x => x.ID).ToList();
                    var pendingReqListCount = pendingReqList.Count;
                    aVm.PendingRequisitionCount = pendingReqListCount;
                    aVm.PendingRequisitionList = pendingReqList.Take(5).ToList();


                    var pendingChequeList = db.CARDCHTRAN.Where(x => x.STATUS == 13).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                    var pendingChequeListCount = pendingChequeList.Count;
                    aVm.PendingCardChequeAmount = pendingChequeList.Sum(x => x.AMOUNT);
                    aVm.PendingCardChequeCount = pendingChequeListCount;
                    aVm.PendingCardChequeList = pendingChequeList.Take(5).ToList();

                    var authReqList = db.CARDCHEREUISITION.Where(x => x.ISDELETE != true).Where(x => x.STATUS == 5).OrderByDescending(x => x.ID).ToList();
                    var authReqListCount = authReqList.Count;
                    aVm.AuthRequisitionCount = authReqListCount;
                    aVm.AuthorizedRequisitionList = authReqList.Take(5).ToList();


                    var authChequeList = db.CARDCHTRAN.Where(x => x.STATUS == 14).Where(x=>x.ISDELETE!=true).OrderByDescending(x => x.ID).ToList();
                    var authChequeListCount = authChequeList.Count;
                    aVm.AuthorizedCardChequeAmount = authChequeList.Sum(x => x.AMOUNT);
                    aVm.AuthCardCHequeCount = authChequeListCount;
                    aVm.AuthorizedCardChequeList = authChequeList.Take(5).ToList();


                    if (btnName == "pendingreq")
                    {
                        pendingReqList = db.CARDCHEREUISITION.Where(x => x.ISDELETE != true).Where(x => x.STATUS == 4).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).ToList();
                        pendingReqListCount = pendingReqList.Count;
                        aVm.PendingRequisitionCount = pendingReqListCount;
                        aVm.PendingRequisitionList = pendingReqList.ToList();
                        //aVm.Deposits = List;
                    }
                    else if (btnName == "pendingcc")
                    {
                        pendingChequeList = db.CARDCHTRAN.Where(x => x.STATUS == 13).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x=>x.ISDELETE!=true).ToList();
                        pendingChequeListCount = pendingChequeList.Count;
                        aVm.PendingCardChequeAmount = pendingChequeList.Sum(x => x.AMOUNT);
                        aVm.PendingCardChequeCount = pendingChequeListCount;
                        aVm.PendingCardChequeList = pendingChequeList.ToList();

                    }
                    else if (btnName == "authreq")
                    {
                        authReqList = db.CARDCHEREUISITION.Where(x => x.ISDELETE != true).Where(x => x.STATUS == 5).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).ToList();
                        authReqListCount = authReqList.Count;
                        aVm.AuthRequisitionCount = authReqListCount;
                        aVm.AuthorizedRequisitionList = authReqList.Take(5).ToList();

                    }

                    else if (btnName == "authcc")
                    {
                        authChequeList = db.CARDCHTRAN.Where(x => x.STATUS == 14).Where(x => x.CREATEDON >= startDate).Where(x => x.CREATEDON <= endDate).Where(x=>x.ISDELETE!=true).ToList();
                        authChequeListCount = authChequeList.Count;
                        aVm.AuthorizedCardChequeAmount = authChequeList.Sum(x => x.AMOUNT);
                        aVm.AuthCardCHequeCount = authChequeListCount;
                        aVm.AuthorizedCardChequeList = authChequeList.Take(5).ToList();

                    }
                    return View(aVm);
                }
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }
        }


        #region Cheque Reequistion Authorization

        public ActionResult RequisitionRequest(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
        {
            Dictionary<int, string> statusID;
            OCCUSER user = (OCCUSER) Session["User"];
            if (user.TYPE == 1)
            {
                statusID =  new Dictionary<int, string>(){ {4,"applied"},{5,"authorized"},{7,"received"},{8,"deny"}}; 
            }
            else
            {
                statusID = new Dictionary<int, string>(){{4,"applied"},{5,"authorized"} };
            }
           
            ViewBag.STATUS = new SelectList(statusID, "Key", "Value");


            var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.ISDELETE == false).OrderByDescending(x => x.ID).ToList();
            if (STATUS != null)
            {
                List = List.Where(x => x.STATUS == STATUS).ToList();
                ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
                ViewBag.currsts = STATUS;
                // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
            }
            else
            {
                List = List.Where(x => x.STATUS == 4).ToList();
            }
            if (!String.IsNullOrEmpty(CARDNO))
            {
                CARDNO = CARDNO.Trim();
                List = List.Where(x => x.CARDNO == CARDNO).ToList();
                ViewBag.CARDNO = CARDNO;
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.CREATEDON.Date == CREATEDON.Value.Date).ToList();
                ViewBag.CREATEDON = CREATEDON;
            }

            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = ((page ?? 1));
            return View(List.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult PostAuthList(IEnumerable<long> idList)
        {
            List<CARDCHEREUISITION> UpdatedList = new List<CARDCHEREUISITION>();
            long count = 1;
            
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];

                foreach (var id in idList)
                {
                    var cheueReq = db.CARDCHEREUISITION.FirstOrDefault(x => x.ID == id);
                    cheueReq.STATUS = db.OCCENUMERATION.Where(x => x.Type == "chequereq").Where(x => x.Name == "authorized").Select(x=>x.ID).FirstOrDefault();
                    cheueReq.AUTHORIZEDBY = user.ID;
                    cheueReq.AUTHORIZEDON = DateTime.Now;
                    UpdatedList.Add(cheueReq);
                }

                foreach (var editedChqRq in UpdatedList)
                {
                    db.Entry(editedChqRq).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // var authorizedList = db.CARDCHEREUISITION.Where(x => x.STATUS == 4).ToList();
                return RedirectToAction("RequisitionRequest");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        #endregion       

        #region Cheque CardCheque Authorization
        public ActionResult CardChequePendingRequest(string CARDNO, DateTime? CREATEDON, int? page, int? STATUS)
        {
            var statusID = db.OCCENUMERATION.Where(x => x.Type == "cardcheque");
            List<CARDCHTRAN> List=new List<CARDCHTRAN>();
            ViewBag.STATUS = new SelectList(statusID, "ID", "Name");
            OCCUSER user=(OCCUSER) Session["User"];
           // db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x=>x.)
            if (STATUS != null)
            {
               // List = List.Where(x => x.STATUS == STATUS).ToList();
                //if (STATUS == 13)
                //{

                //    List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 13).Where(x => x.ISDELETE != true).ToList();       
                //}
                //else
                //{
                //    List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == 14).Where(x => x.ISDELETE != true).ToList();        
                //}
                List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.STATUS == STATUS).Where(x => x.ISDELETE != true).OrderByDescending(x=>x.ID).ToList();       
 
                ViewBag.currentStatus = STATUS;
                ViewBag.STATUS = new SelectList(statusID, "ID", "Name", STATUS);
            }
            else 
            {
                List = db.CARDCHTRAN.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x=>x.ISDELETE!=true).OrderByDescending(x=>x.ID).ToList();       
            }
              

          
            if (!String.IsNullOrEmpty(CARDNO))
            {
                List = List.Where(x => x.CARDNO == CARDNO).Where(x => x.ISDELETE != true).ToList();
                ViewBag.currentCardNo = CARDNO;
            }
            if (CREATEDON != null)
            {
                List = List.Where(x => x.CREATEDON == CREATEDON).Where(x => x.ISDELETE != true).ToList();
                ViewBag.currentDate = CREATEDON;
            }

           // int pageSize = 3;
            int pageSize = ConstantConfig.PageSizes;
            int pageNumber = ((page ?? 1));
            //var chqSts = db.OCCENUMERATION.Where(x => x.Type == "cardcheque" && x.Name != "pending").ToList();
            //ViewBag.CHQSTATUS = new SelectList (chqSts, "Id", "Name");
            return View(List.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult CardChAuthPost(FormCollection formCollection)
        
        {
            List<long> ID=new List<long>();
            List<string> APPROVALNO=new List<string>();
            List<int> CHQSTATUS = new List<int>();
            List<string> REMARKSLIST = new List<string>();
            try
            {
                OCCUSER user = (OCCUSER)Session["User"];
                List<CardChequeAuthorizrVM> vm = new List<CardChequeAuthorizrVM>();

                foreach (var key in formCollection.AllKeys)
                {
                    if (key.Contains("ID"))
                    {
                        long id =  Convert.ToInt64(formCollection[key]);
                         ID.Add(id);
                    }
                    if (key.Contains("APPROVAL"))
                    {
                        string approval = formCollection[key];
                        APPROVALNO.Add(approval);                        
                    }
                    if (key.Contains("CHQSTATUS"))
                    {
                        int chqStatus = Convert.ToInt32(formCollection[key]);
                        CHQSTATUS.Add(chqStatus);
                    }
                    if (key.Contains("REMARKS"))
                    {
                        string remarks = formCollection[key];
                        REMARKSLIST.Add(remarks);
                    }
                    
                    var value = formCollection[key];
                }
                for (int i = 0; i < ID.Count; i++)
                {

                    var aVm = new CardChequeAuthorizrVM() { APPROVALNO = APPROVALNO[i], ID = ID[i], ChqStatus = CHQSTATUS[i], REMARKS = REMARKSLIST[i] };
                    vm.Add(aVm);
                }
               // var a = formCollection["CHQSTATUS"];
                foreach (var aViewModel in vm)
                {
                    var cardChq = db.CARDCHTRAN.FirstOrDefault(x => x.ID == aViewModel.ID);
                    cardChq.APPROVALNO = aViewModel.APPROVALNO;
                    cardChq.STATUS = aViewModel.ChqStatus;
                    cardChq.REMARKS = aViewModel.REMARKS;
                    cardChq.AUTHORIZEDBY = user.ID;
                    cardChq.AUTHORIZEDON = DateTime.Now;
                    db.Entry(cardChq).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("CardChequePendingRequest");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { Area = "" });
            }


        }

        #endregion

        #region Download Visa Xml

        public ActionResult VisaXmlDownload()
        {
            var List = db.DEPOSIT.Where(x => x.ISDOWNLOAD == false && x.ISDELETE == false).OrderByDescending(x=>x.ID).ToList();
            List<string> currencyList = new List<string>() { "USD", "BDT" };
            ViewBag.CURRENCY = new SelectList(currencyList);
            return View(List);
        }
        public ActionResult DownloadXml(string CURRENCY)
        {
            try
            {
               // int flag = 0;
                if (String.IsNullOrEmpty(CURRENCY))
                {
                    return RedirectToAction("VisaXmlDownload");
                }
                //Create Memory Stream to store XML Data
                MemoryStream ms = new MemoryStream();
                //Use a writer to greate the XML
                using (XmlWriter writer = XmlWriter.Create(ms))
                {
                    writer.WriteStartElement("Visa");
                    var list = db.DEPOSIT.Where(x => x.ISDOWNLOAD == false && x.ISDELETE == false);
                    if (CURRENCY=="USD")
                    {
                        list= list.Where(x => x.CURRENCY == "USD");
                      //  flag = 1;
                    }
                    else if (CURRENCY == "BDT")
                    {
                         list = list.Where(x => x.CURRENCY == "BDT");
                       // flag = 0;
                    }
                                     
                    foreach (var aList in list.ToList())
                    {
                        writer.WriteStartElement("Visa_Payment");
                        writer.WriteElementString("SBK_PAN", aList.CARDNUMBER);
                        writer.WriteElementString("MBR", "0");
                        writer.WriteElementString("SBK_SUM", aList.AMOUNT.ToString());
                        writer.WriteElementString("SBK_UDT",
                            aList.PAYMENTTYPE == "Cash"
                                ? "Payment Received Cash - Thank You."
                                : aList.PAYMENTTYPE == "Cheque"
                                    ? "Payment Received Cheque - Thank You."
                                    : "Payment Received Account Debit Instruction - Thank You.");
                        writer.WriteEndElement();
                        aList.ISDOWNLOAD= true;
                        db.Entry(aList).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    writer.WriteEndElement();
                    writer.Close();
                    //writer.Flush();

                    //Convert Memory Stream to Byte Array
                    byte[] data = ms.ToArray();

                    //The Proposed FileName that will show when the 
                    //user is prompted to save the file
                    string xmlFileName = "";
                    if (CURRENCY == "USD")
                    {
                        xmlFileName = "Visa_USD_" + DateTime.Today.Year.ToString() +
                                         DateTime.Today.Month.ToString("00") +
                                         DateTime.Today.Day.ToString("00");
                    }
                    else if(CURRENCY=="BDT")
                    {
                        xmlFileName = "Visa_BDT_" + DateTime.Today.Year.ToString() +
                                        DateTime.Today.Month.ToString("00") +
                                        DateTime.Today.Day.ToString("00");
                    }

                    //Creating the Context
                    Response.Clear();

                    //Heads up browser, here comes some XML
                    Response.ContentType = "text/xml";
                    Response.AddHeader("Content-Disposition:",
                        "attachment;filename=" + HttpUtility.UrlEncode(xmlFileName));


                    //Download the file and prompt the user to save
                    Response.BinaryWrite(data);
                    Response.End();
                    ms.Flush();
                    ms.Close();
                }
                return RedirectToAction("VisaXmlDownload");
            }
            catch (Exception exception)
            {
                var a = exception.Message;
                return RedirectToAction("VisaXmlDownload");
            }
        }


        #endregion

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


//#region Delivary Report



//public ActionResult DelivaryChequeBook(int? STATUS, string CARDNO, DateTime? CREATEDON, int? page)
//{
//    Dictionary<int, string> statusID;
//    OCCUSER user = (OCCUSER)Session["User"];
//    if (user.TYPE == 1)
//    {
//        statusID = new Dictionary<int, string>() { { 4, "applied" }, { 5, "authorized" }, { 7, "received" }, { 8, "deny" }, { 16, "delivered" } };
//    }
//    else
//    {
//        statusID = new Dictionary<int, string>() { { 4, "applied" }, { 5, "authorized" }, { 7, "received" }, { 16, "delivered" } };
//    }

//    ViewBag.STATUS = new SelectList(statusID, "Key", "Value");
//    var statusId =
//                db.OCCENUMERATION.Where(x => x.Type == "chequereq")
//                    .Where(x => x.Name == "received")
//                    .Select(x => x.ID)
//                    .FirstOrDefault();

//    var List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.ISDELETE != true).Where(x => x.STATUS == statusId).OrderByDescending(x => x.ID).ToList();
//    if (STATUS != null)
//    {
//        List = db.CARDCHEREUISITION.Include(c => c.BRANCHINFO).Include(c => c.OCCENUMERATION).Include(c => c.OCCUSER).Where(x => x.ISDELETE != true).Where(x => x.STATUS == STATUS).OrderByDescending(x => x.ID).ToList();
//        ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
//        ViewBag.currsts = STATUS;
//        // ViewBag.STATUS = new SelectList(statusID, "Key", "Value", statusID.Where(x => x.Key == STATUS));
//    }

//    if (!String.IsNullOrEmpty(CARDNO))
//    {
//        CARDNO = CARDNO.Trim();
//        List = List.Where(x => x.CARDNO.Contains(CARDNO)).Where(x => x.ISDELETE != true).ToList();
//        ViewBag.CARDNO = CARDNO;
//    }
//    if (CREATEDON != null)
//    {
//        List = List.Where(x => x.ESTABLISHMENTON.Value.Date == CREATEDON).Where(x => x.ISDELETE != true).ToList();
//        ViewBag.CREATEDON = CREATEDON;
//    }

//    int pageSize = ConstantConfig.PageSizes;
//    int pageNumber = ((page ?? 1));
//    return View(List.ToPagedList(pageNumber, pageSize));
//}


//public ActionResult DelivaryPost(IEnumerable<long> idList)
//{
//    OCCUSER user = (OCCUSER) Session["User"];

//    try
//    {
//        foreach (var id in idList)
//        {
//            var chequereq = db.CARDCHEREUISITION.Find(id);
//            var statusId= db.OCCENUMERATION.Where(x => x.Type == "chequereq")
//                    .Where(x => x.Name == "delivered")
//                    .Select(x => x.ID)
//                    .FirstOrDefault();
//            chequereq.DELIVEREDBY = user.ID;
//            chequereq.DELIVEREDON = DateTime.Now;
//            chequereq.STATUS = statusId;
//            db.Entry(chequereq).State = EntityState.Modified;
//            db.SaveChanges();
//        }
//        return RedirectToAction("DelivaryChequeBook");
//    }
//    catch (Exception exception)
//    {
//        return RedirectToAction("Error", "Home", new { Area = "" });
//    }
//}

//#endregion
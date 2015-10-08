using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CardChequeModule.Models;
using CardChequeModule.OraDBCardInfo;
using CardChequeModule.WebRef;
using TestTemplate.Models;

namespace CardChequeModule.Controllers
{
    public class HomeController : Controller
    {
        OBLCARDCHEQUEEntities db=new OBLCARDCHEQUEEntities();
        OraDBCardInfo.OradbaccessSoapClient cardApi = new OradbaccessSoapClient();
        private int flag;

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetCardDetails(string cardNo)
        {
            string name = "";
            string msg = "";
            DateTime dob = DateTime.Now;
            int flag = 0;

            try
            {
                DataTable dt = cardApi.GetCCardDetail(cardNo);

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dataRow in dt.Rows)
                    {
                        name = (string)dataRow[2];
                        dob = (DateTime)dataRow[3];
                    }
                    msg = "This Card Is Valid, User Name: " + name + " and Date of Birth: " + dob.Date.ToShortDateString();
                    return Json(new { msg, flag = 1 }, JsonRequestBehavior.AllowGet);
                }
                msg = "Invalid Card Number. Please Recheck";
                return Json(new { msg, flag }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                return Json(exception, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult LogIn()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(OCCUSER aUser, string PASSWORD)
        {
            WebRef.OBLAPP oblApp=new WebRef.OBLAPP();
            try
            {
              
                var isValid = oblApp.GetByUserIDCheck(aUser.EMPLOYEEID, PASSWORD);
                
                if (isValid == "Valid")
                {
                    ViewBag.flag = "";
                    if (IsUserInSystem(aUser.EMPLOYEEID))
                    {
                        var user = GetByEmpId(aUser.EMPLOYEEID);
                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(Convert.ToString(user.ID), false);
                            Session["User"] = user;
                            if (user.TYPE == 1)
                            {
                                return RedirectToAction("Index", "Home", new { Area = "Admin" });
                            }
                            if (user.TYPE == 3)
                            {
                                return RedirectToAction("Index", "Home", new { Area = "Authorizer" });
                            }
                            if (user.TYPE == 2)
                            {
                                return RedirectToAction("Index", "Home", new { Area = "Teller" });
                            }
                            if (user.TYPE == 9)
                            {
                                return RedirectToAction("Index", "Home",new {Area="Establishment"});
                            }
                            if (user.TYPE == 15)
                            {
                                return RedirectToAction("Index", "Home", new { Area = "CallCenter" });
                            }
                            if (user.TYPE == 17)
                            {
                                return RedirectToAction("Index", "Home", new { Area = "Dispatcher" });
                            }
                        }

                    }
                    else
                    {
                        ViewBag.flag = "User is not in the system";
                    }


                }
                else if (isValid == "Invalid")
                {
                    ViewBag.flag = "Invalid User";
                    return View();
                }
                else if (isValid == "NotFound")
                {
                    ViewBag.flag = "User Not Found";
                    return View();
                }
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.flag = "Something is wrong "+exception.Message;
                return View();
            }
            
        }

        public ActionResult Error()
        {           
            return View();
        }
        private bool IsUserInSystem(string employeeid)
        {
            var user = GetByEmpId(employeeid);
            if (user != null)
                return true;
            return false;
        }

        private OCCUSER GetByEmpId(string employeeid)
        {
            var user = db.OCCUSER.FirstOrDefault(x => x.EMPLOYEEID == employeeid);
            return user;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["User"] = null;
            return RedirectToAction("Index");

        }
    }
}
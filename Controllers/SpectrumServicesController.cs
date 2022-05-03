using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleTax;

namespace SimpleTax.Controllers
{
    public class SpectrumServicesController : Controller
    {
        // GET: SpectrumServices
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            //DatabaseHelpers.InitializeUserSpectrumServices();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogInControl userControl)
        {
            if (!ModelState.IsValid)
            {
                var userCtrl = new UserLogInControl();
                return View("LogIn", userCtrl);
            }
            User uuser = new User();
            uuser.Username = userControl.Username;
            uuser.Password = userControl.Password;
            userControl.user = uuser;
            User user = DatabaseHelpers.ifValidLoginSpectrumServices(userControl.user);

            //string str = SessionStatus.Admin.ToString();
            if (user != null)
            {
                if (user.IsAdmin && user.IsValidCredentials)
                {
                    Session[SessionStatus.Admin.ToString()] = SessionStatus.Admin.ToString();
                    Session["userId"] = user.Id;
                    return RedirectToAction("Overview", "SpectrumServices", new { userId = user.Id.ToString() });
                }
                else
                {
                    Session[SessionStatus.Customer.ToString()] = SessionStatus.Customer.ToString();
                    return RedirectToAction("Home", "SpectrumServices");
                }
            }
            else
            {
                var userCtrl = new UserLogInControl()
                {
                    IncorrectLogin = true,
                    LoginMessage = "Incorrect username and password.",
                };
                return View(userCtrl);
            }
        }


    
        public ActionResult Overview(string userId)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
                return RedirectToAction("Home", "SpectrumServices");
            return View();
        }


    
        public ActionResult FHCC_Overview()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
                return RedirectToAction("Home", "SpectrumServices");

            FHCCOverviewControl foc = new FHCCOverviewControl();

            List<DirectSalesDaily> ListDirectSales = DatabaseHelpers.GetDirectSalesDailyBoxesSold();
            List<DirectSalesDaily> ListDirectSalesMonthly = DatabaseHelpers.GetDirectSalesDailyBoxesSold(DateTime.Now.ToShortDateString());

            List<Products> ProductsForReference = DatabaseHelpers.LoadProductList();

            foreach (DirectSalesDaily dsd in ListDirectSales)
            {
                foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                {
                    foreach (Products pro in ProductsForReference)
                    {
                        if (prdDs.Products != null)
                            if (pro.Description.Equals(prdDs.Products.Description) && !dsd.isCancelled && !dsd.isDeleted)
                            {
                                if (!string.IsNullOrEmpty(prdDs.Qty))
                                    pro.Qty += Convert.ToDouble(prdDs.Qty);

                                foc.FhccBoxesSoldDaily = pro.Qty.ToString();
                            }
                    }
                }
            }


            return View(foc);

        }

        public ActionResult LogOut()
        {
            Session[SessionStatus.Admin.ToString()] = null;
            Session[SessionStatus.Customer.ToString()] = null;
            Session["userId"] = null;
            return RedirectToAction("Home", "SpectrumServices");
        }

    }
}
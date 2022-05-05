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


    
        public ActionResult FHCC_Overview(FHCCOverviewControl focc)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
                return RedirectToAction("Home", "SpectrumServices");

            FHCCOverviewControl foc = new FHCCOverviewControl();

            List<DirectSalesDaily> ListDirectSales = DatabaseHelpers.GetDirectSalesDailyBoxesSold();
            List<DirectSalesDaily> ListDirectSalesMonthly = DatabaseHelpers.GetDirectSalesDailyBoxesSold(DateTime.Now.ToShortDateString());

            List<Products> ProductsForReference = DatabaseHelpers.LoadProductList();

            //if (focc.search != null)
            //{
            //    foreach (DirectSalesDaily dsd in ListDirectSales)
            //    {
            //        foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
            //        {
            //            foreach (Products pro in ProductsForReference)
            //            {
            //                if (prdDs.Products != null)
            //                    if (pro.Description.Equals(prdDs.Products.Description) && !dsd.isCancelled && !dsd.isDeleted)
            //                    {
            //                        if (!string.IsNullOrEmpty(prdDs.Qty))
            //                        {
            //                            pro.Qty += Convert.ToDouble(prdDs.Qty);
            //                            focc.FhccBoxesSoldDaily = pro.Qty.ToString();
            //                        }
            //                    }
            //            }
            //        }
            //    }

            //    return View(focc);
            //}
            
            SearchDateViewModel search = new SearchDateViewModel();
            search.FhccBoxesSoldDaily = "0";
            search.FhccBoxesSoldMonthly = "0";
            foc.search = search;

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
                                {
                                    pro.Qty += Convert.ToDouble(prdDs.Qty);
                                    foc.FhccBoxesSoldDaily = pro.Qty.ToString();
                                }   
                            }
                    }
                }
            }


            return View(foc);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchSalesDate(FHCCOverviewControl overviewControl)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new FHCCOverviewControl
                {
                    FromDate = overviewControl.search.FromDate,
                    ToDate = overviewControl.search.ToDate,
                };

                return View("SpectrumServices", viewModel);
            }

            if(!string.IsNullOrEmpty(overviewControl.search.FromDate.ToShortDateString()) && !string.IsNullOrEmpty(overviewControl.search.ToDate.ToShortDateString())){

                List<DirectSalesDaily> DirectSalesSalesCustom = DatabaseHelpers.GetDirectSalesDailyBoxesSold(overviewControl.search.FromDate.ToShortDateString(), overviewControl.search.ToDate.ToShortDateString());

                FHCCOverviewControl foc = new FHCCOverviewControl();
                List<Products> ProductsForReference = DatabaseHelpers.LoadProductList();
                SearchDateViewModel svm = new SearchDateViewModel();

                double count = 0;
                foreach (DirectSalesDaily dsd in DirectSalesSalesCustom)
                {
                    foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                    {
                        foreach (Products pro in ProductsForReference)
                        {
                            if (prdDs.Products != null)
                                if (pro.Description.Equals(prdDs.Products.Description) && !dsd.isCancelled && !dsd.isDeleted)
                                {
                                    if (!string.IsNullOrEmpty(prdDs.Qty))
                                    {
                                        pro.Qty += Convert.ToDouble(prdDs.Qty);
                                        count = pro.Qty;
                                    }
                                       
                                }
                        }
                    }
                }

                List<DirectSalesDaily> ListDirectSales = DatabaseHelpers.GetDirectSalesDailyBoxesSold();
                foc.FhccBoxesSoldDaily = string.Empty;

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
                                    {
                                        pro.Qty += Convert.ToDouble(prdDs.Qty);
                                        foc.FhccBoxesSoldDaily = pro.Qty.ToString();
                                    }
                                }
                        }
                    }
                }

                svm.FhccBoxesSoldDaily = count.ToString();

                foc.search = svm; 


                return View("FHCC_Overview", foc);
             }

            return RedirectToAction("FHCC_Overview", "SpectrumServices");
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
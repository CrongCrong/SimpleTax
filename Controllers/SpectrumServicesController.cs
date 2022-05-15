﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
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
            
            SearchDateViewModel search = new SearchDateViewModel();
            search.FhccBoxesSoldDaily = "0";
            search.FhccBoxesSoldMonthly = "0";
            double count = 0;

            foc.search = search;
            ObjectId fhBox = ObjectId.Parse("5c5005be7973a5351c95c30f");
            ObjectId fhBoxMC = ObjectId.Parse("5ddcf84a7973a551bcff145d");

            foreach (DirectSalesDaily dsd in ListDirectSales)
            {
                foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                {
                    if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                    {
                        if (!string.IsNullOrEmpty(prdDs.Qty))
                        {
                            count += Convert.ToDouble(prdDs.Qty);
                            foc.FhccBoxesSoldDaily = count.ToString();
                        }
                    }

                    //foreach (Products pro in ProductsForReference)
                    //{
                    //    if (prdDs.Products != null)
                    //    {
                    //        if ((pro.Id.Equals(fhBoxMC) || pro.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                    //        {
                    //            if (!string.IsNullOrEmpty(prdDs.Qty))
                    //            {
                    //                pro.Qty += Convert.ToDouble(prdDs.Qty);
                    //                foc.FhccBoxesSoldDaily = pro.Qty.ToString();
                    //            }
                    //        }
                    //    }
                           
                    //}
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
                List<Products> ProductsForReference2 = ProductsForReference;
                SearchDateViewModel svm = new SearchDateViewModel();

                ObjectId fhBox = ObjectId.Parse("5c5005be7973a5351c95c30f");
                ObjectId fhBoxMC = ObjectId.Parse("5ddcf84a7973a551bcff145d");

                double count = 0;
                double count2 = 0;

                foreach (DirectSalesDaily dsd in DirectSalesSalesCustom)
                {
                    foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                    {
                        if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                        {
                            if (!string.IsNullOrEmpty(prdDs.Qty))
                            {
                                count += Convert.ToDouble(prdDs.Qty);
                                foc.FhccBoxesSoldDaily = count2.ToString();
                            }
                        }
                        //foreach (Products pro in ProductsForReference)
                        //{
                        //    if (prdDs.Products != null)
                        //        if (pro.Description.Equals(prdDs.Products.Description) && !dsd.isCancelled && !dsd.isDeleted)
                        //        {
                        //            if (!string.IsNullOrEmpty(prdDs.Qty))
                        //            {
                        //                pro.Qty += Convert.ToDouble(prdDs.Qty);
                        //                count = pro.Qty;
                        //            }

                        //        }
                        //}
                    }
                }

                List<DirectSalesDaily> ListDirectSales = DatabaseHelpers.GetDirectSalesDailyBoxesSold();
                foc.FhccBoxesSoldDaily = string.Empty;

                foreach (DirectSalesDaily dsd in ListDirectSales)
                {
                    foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                    {

                        if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                                {
                                    if (!string.IsNullOrEmpty(prdDs.Qty))
                                    {
                                       count2 += Convert.ToDouble(prdDs.Qty);
                                        foc.FhccBoxesSoldDaily = count2.ToString();
                                    }
                            }


                            //if (prdDs.Products != null)
                            //{
                            //    if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                            //    {
                            //        if (!string.IsNullOrEmpty(prdDs.Qty))
                            //        {
                            //            count2 += Convert.ToDouble(prdDs.Qty);
                            //            foc.FhccBoxesSoldDaily = count2.ToString();
                            //        }
                            //    }
                            //}

                    }
                }

                svm.FhccBoxesSoldDaily = count.ToString();

                foc.search = svm; 
                //test

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
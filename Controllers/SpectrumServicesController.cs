using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;


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
            List<DirectSalesDaily> ListDirectSalesMonthly = DatabaseHelpers.GetDirectSalesDailyBoxesSold("01/01/2021", "01/31/2021");

            List<Products> ProductsForReference = DatabaseHelpers.LoadProductList();

            SearchDateViewModel search = new SearchDateViewModel();
            search.FhccBoxesSoldDaily = "0";
            search.FhccBoxesSoldMonthly = "0";
            double count = 0;
            double count2 = 0;

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
                }
            }

            foreach (DirectSalesDaily dsd in ListDirectSalesMonthly)
            {
                foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                {
                    if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                    {
                        if (!string.IsNullOrEmpty(prdDs.Qty))
                        {
                            count2 += Convert.ToDouble(prdDs.Qty);
                            foc.FhccBoxesSoldMonthly = count2.ToString();
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

            if (!string.IsNullOrEmpty(overviewControl.search.FromDate.ToShortDateString()) && !string.IsNullOrEmpty(overviewControl.search.ToDate.ToShortDateString()))
            {

                List<DirectSalesDaily> DirectSalesSalesCustom = DatabaseHelpers.GetDirectSalesDailyBoxesSold(overviewControl.search.FromDate.ToShortDateString(), overviewControl.search.ToDate.ToShortDateString());

                FHCCOverviewControl foc = new FHCCOverviewControl();
                List<Products> ProductsForReference = DatabaseHelpers.LoadProductList();
                List<Products> ProductsForReference2 = ProductsForReference;
                SearchDateViewModel svm = new SearchDateViewModel();

                ObjectId fhBox = ObjectId.Parse("5c5005be7973a5351c95c30f");
                ObjectId fhBoxMC = ObjectId.Parse("5ddcf84a7973a551bcff145d");

                double count = 0;
                double count2 = 0;
                double count3 = 0;

                foreach (DirectSalesDaily dsd in DirectSalesSalesCustom)
                {
                    foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                    {
                        if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                        {
                            if (!string.IsNullOrEmpty(prdDs.Qty))
                            {
                                count += Convert.ToDouble(prdDs.Qty);
                                foc.FhccBoxesSoldMonthly = count2.ToString();
                            }
                        }

                    }
                }

                List<DirectSalesDaily> ListDirectSales = DatabaseHelpers.GetDirectSalesDailyBoxesSold();
                List<DirectSalesDaily> ListDirectSalesMonthly = DatabaseHelpers.GetDirectSalesDailyBoxesSold("01/01/2021", "01/31/2021");


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
                    }
                }

                foreach (DirectSalesDaily dsd in ListDirectSalesMonthly)
                {
                    foreach (ProductsOrderedDS prdDs in dsd.ProductsOrdered)
                    {
                        if ((prdDs.Products.Id.Equals(fhBoxMC) || prdDs.Products.Id.Equals(fhBox)) && !dsd.isCancelled && !dsd.isDeleted)
                        {
                            if (!string.IsNullOrEmpty(prdDs.Qty))
                            {
                                count3 += Convert.ToDouble(prdDs.Qty);
                                foc.FhccBoxesSoldMonthly = count3.ToString();
                            }
                        }
                    }
                }

                svm.FhccBoxesSoldMonthly = count.ToString();

                foc.search = svm;

                return View("FHCC_Overview", foc);
            }

            return RedirectToAction("FHCC_Overview", "SpectrumServices");
        }


        public ActionResult SPMProducts_Overview(SPMProductsOverviewControl overviewControl)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
                return RedirectToAction("Home", "SpectrumServices");

            SPMProductsOverviewControl spmOc = new SPMProductsOverviewControl();
            List<FHSkinProductSales> ListProductsToDisplay = new List<FHSkinProductSales>();
            List<FHSkinProductSales> ListProductsToDisplay2 = new List<FHSkinProductSales>();

            FHSkinProductSales ProductDisplay = new FHSkinProductSales();

            List<FHSkinProducts> ListSPMProducts = DatabaseHelpers.LoadSPMProductsList();
            List<FHSkinSales> ListSPMProductSales = DatabaseHelpers.LoadFHSkinSales();


            foreach (FHSkinSales sales in ListSPMProductSales)
            {
                if (sales.ProductsBought != null)
                {
                    foreach (FHSkinProducts skinProd in sales.ProductsBought)
                    {
                        foreach (FHSkinProducts DefaultProd in ListSPMProducts)
                        {
                            if (DefaultProd.Id == skinProd.Id)
                            {
                                DefaultProd.ProductQuantity = (Convert.ToDouble(DefaultProd.ProductQuantity) + Convert.ToDouble(skinProd.ProductQuantity)).ToString();
                            }
                        }
                    }

                    foreach (FHSkinProducts prod in ListSPMProducts)
                    {
                        ProductDisplay.Product = prod;
                        ProductDisplay.ProductQuantity = prod.ProductQuantity;
                        ListProductsToDisplay.Add(ProductDisplay);
                        ProductDisplay = new FHSkinProductSales();
                    }

                }
                else if (sales.ProductsSales != null)
                {
                    foreach (FHSkinProductSales skinProd in sales.ProductsSales)
                    {
                        foreach (FHSkinProducts DefaultProd in ListSPMProducts)
                        {
                            if (DefaultProd.Id == skinProd.Product.Id)
                            {
                                ProductDisplay.Product = DefaultProd;
                                ProductDisplay.ProductQuantity = (Convert.ToDouble(ProductDisplay.ProductQuantity) + Convert.ToDouble(skinProd.ProductQuantity)).ToString();
                                ListProductsToDisplay.Add(ProductDisplay);
                                ProductDisplay = new FHSkinProductSales();
                            }
                        }
                    }
                }
            }

            ListProductsToDisplay = ListProductsToDisplay.OrderByDescending(a => a.Product.ProductDescription).ToList();

            ListProductsToDisplay = ListProductsToDisplay.GroupBy(g => new { g.Product, g.Product.ProductDescription })
                .Select(s => new FHSkinProductSales
                {
                    Product = s.Key.Product,
                    ProductName = s.Key.ProductDescription,
                    ProductQuantity = s.Sum(q => Convert.ToDouble(q.ProductQuantity)).ToString(),
                }).ToList();

            spmOc.ListProductsSold = ListProductsToDisplay;

            return View(spmOc);
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
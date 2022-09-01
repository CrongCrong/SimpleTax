using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleTax.Controllers.Api
{
    public class SPMProductsController : ApiController
    {

        static ConnectionDB conDB = new ConnectionDB();


        //GET /api/spmproducts
        public IEnumerable<FHSkinProductSales> GetSpmProducts()
        {
            List<FHSkinProducts> ListSPMProducts = DatabaseHelpers.LoadSPMProductsList();
            List<FHSkinSales> ListSPMProductSales = DatabaseHelpers.LoadFHSkinSales();
            List<FHSkinProductSales> ListProductsToDisplay = new List<FHSkinProductSales>();
            FHSkinProductSales ProductDisplay = new FHSkinProductSales();

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

            return ListProductsToDisplay;
        }
    }
}

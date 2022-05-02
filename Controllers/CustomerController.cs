

using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SimpleTax.Controllers
{
    [AuthorizeUsers]
    public class CustomerController : Controller
    {
        // GET: Customer

        [CustomAuthorization]
        public ActionResult Index()
        {
            var listCustomers = DatabaseHelpers.LoadCustomers();
            var viewModel = new CustomerViewModel()
            {
                customer = new Customer(),
                ListCustomers = listCustomers,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Home()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
                return RedirectToAction("Index", "Home");

            string userId = Session["userId"].ToString();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Home");

            Customer customer = DatabaseHelpers.ifValidLogin(userId);

            var viewModel = new CustomerViewModel()
            {
                customer = customer,
            };
            return View("Home", viewModel);

        }

        public ActionResult Home(string userId)
        {

            //if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
            //    return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])) && string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Home");

            userId = (string.IsNullOrEmpty(userId)) ? Convert.ToString(Session["userID"]) : userId;

            Customer customer = DatabaseHelpers.ifValidLogin(userId);
            Session["userId"] = customer.Id;
            var viewModel = new CustomerViewModel()
            {
                customer = customer,
            };
            return View("Home", viewModel);

        }


        [CustomAuthorization]
        public ActionResult New()
        {
            var listCustomers = DatabaseHelpers.LoadCustomers();
            List<string> ListTaxPayerType = new List<string>()
            {
                "VAT",
                "NON-VAT",
            };
            var viewModel = new CustomerViewModel()
            {
                customer = new Customer(),
                ListCustomers = listCustomers,
                TaxPayerTypeList = ListTaxPayerType,
            };

            return View("CustomerInfo", viewModel);
        }

        public ActionResult Edit()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["userId"])))
                return RedirectToAction("Index", "Home");

            string userId = Convert.ToString(Session["userID"]);

            Customer customer = DatabaseHelpers.ifValidLogin(userId);
            List<string> ListTaxPayerType = new List<string>()
            {
                "VAT",
                "NON-VAT",
            };

            var viewModel = new CustomerViewModel()
            {
                CustomerId = customer.Id.ToString(),
                customer = customer,
                TaxPayerTypeList = ListTaxPayerType,
            };

            return View("CustomerInfo", viewModel);
        }

        public ActionResult EditProfile(string id)
        {

            Customer customer = DatabaseHelpers.ifValidLogin(id);
            List<string> ListTaxPayerType = new List<string>()
            {
                "VAT",
                "NON-VAT",
            };
            var viewModel = new CustomerViewModel()
            {
                CustomerId = customer.Id.ToString(),
                customer = customer,
                TaxPayerTypeList = ListTaxPayerType,
            };

            return View("CustomerInfo", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerViewModel
                {
                    customer = customer,
                };

                return View("CustomerInfo", viewModel);
            }

            var ObjId = new ObjectId();
            customer.TaxPayer = (customer.StrTaxpayerType.Equals("VAT")) ? TaxPayerType.VAT : TaxPayerType.NONVAT;
            if (ObjId == ObjectId.Empty)
            {
                DatabaseHelpers.SaveCustomer(customer);
            }
            else
            {
                customer.Id = new ObjectId(customer.StrCustomerId);
                DatabaseHelpers.UpdateCustomer(customer);

                return RedirectToAction("Home", "Customer");
            }

            return RedirectToAction("Index", "Customer");
        }
    }
}
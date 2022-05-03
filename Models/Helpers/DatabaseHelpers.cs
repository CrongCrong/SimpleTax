using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleTax
{
    public class DatabaseHelpers
    {
        static ConnectionDB conDB = new ConnectionDB();

        #region SIMPLE TAX METHODS
        public static List<Customer> LoadCustomers()
        {
            List<Customer> listCustomers = new List<Customer>();
            try
            {
                var db = conDB.client.GetDatabase("DBSIMPLETAX");
                var collection = db.GetCollection<Customer>("Customer");
                var filter = Builders<Customer>.Filter.And(
       Builders<Customer>.Filter.Where(p => p.IsDeleted == false),
       Builders<Customer>.Filter.Where(p => p.Credentials.IsAdmin == false));

                listCustomers = collection.Find(filter).ToList();   

                foreach (Customer ee in listCustomers)
                {
                    ee.StrCustomerId = ee.Id.ToString();
                    ee.StrFullName = ee.Lastname + ", " + ee.Firstname;
                }

                listCustomers = listCustomers.OrderBy(a => a.Lastname).ToList();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.StackTrace);
            }
            return listCustomers;
        }

        public static bool SaveCustomer(Customer customer)
        {
            bool ifOkay = false;
            try
            {
                var db = conDB.client.GetDatabase("DBSIMPLETAX");

                var collection = db.GetCollection<Customer>("Customer");

                customer.Credentials.Firstname = customer.Firstname;
                customer.Credentials.Lastname = customer.Lastname;
                PasswordHash ph = new PasswordHash(customer.Credentials.Password);
                customer.Credentials.bHash = ph.Hash;
                customer.Credentials.bSalt = ph.Salt;

                collection.InsertOne(customer);
                ifOkay = true;
            }
            catch (Exception ex)
            {
                ifOkay = false;
                Debug.Assert(false, ex.StackTrace);
            }
            return ifOkay;
        }

        public static bool UpdateCustomer(Customer customer)
        {
            bool ifOkay = false;
            try
            {
                var db = conDB.client.GetDatabase("DBSIMPLETAX");
                var collection = db.GetCollection<Customer>("Customer");

                var filter = Builders<Customer>.Filter.And(
                Builders<Customer>.Filter.Where(p => p.Id == customer.Id));

                var updte = Builders<Customer>.Update.Set("Address", customer.Address)
                     .Set("Birthday", customer.Birthday)
                     .Set("Businessname", customer.Businessname)
                     .Set("Firstname", customer.Firstname)
                     .Set("Lastname", customer.Lastname)
                     .Set("TinNumber", customer.TinNumber);

                collection.UpdateOne(filter, updte);
                ifOkay = true;
            }
            catch (Exception ex)
            {
                ifOkay = false;
                Debug.Assert(false, ex.StackTrace);
            }
            return ifOkay;
        }

        public static bool DeleteCustomer(string id)
        {
            bool ifOkay = false;
            try
            {
                var objId = new ObjectId(id);
                var db = conDB.client.GetDatabase("DBSIMPLETAX");

                var collection = db.GetCollection<Customer>("Customer");

                var filter = Builders<Customer>.Filter.And(
                Builders<Customer>.Filter.Where(p => p.Id == objId));

                var updte = Builders<Customer>.Update.Set("IsDeleted", true);

                collection.UpdateOne(filter, updte);
                ifOkay = true;
            }
            catch (Exception ex)
            {
                ifOkay = false;
                Debug.Assert(false, ex.StackTrace);
            }
            return ifOkay;
        }

        #endregion

        #region LOGIN METHODS

        public static Customer ifValidLogin(User user)
        {
            PasswordHash ph = new PasswordHash(user.Password);
            var db = conDB.client.GetDatabase("DBSIMPLETAX");
            var collection = db.GetCollection<Customer>("Customer");
            var filter = Builders<Customer>.Filter.And(
    Builders<Customer>.Filter.Where(p => p.Credentials.Username.Equals(user.Username)),
    Builders<Customer>.Filter.Where(p => p.IsDeleted == false));

            Customer userr = collection.Find(filter).ToList().SingleOrDefault(u => u.Credentials.Username.Equals(user.Username));


            if (userr != null)
            {
                ph = new PasswordHash(userr.Credentials.bSalt, userr.Credentials.bHash);
                userr.Credentials.IsValidCredentials = ph.Verify(user.Password);
            }

            return userr;
        }

        public static Customer ifValidLogin(string id)
        {
            var db = conDB.client.GetDatabase("DBSIMPLETAX");
            var collection = db.GetCollection<Customer>("Customer");
            var filter = Builders<Customer>.Filter.And(
    Builders<Customer>.Filter.Where(p => p.Id == new ObjectId(id)),
    Builders<Customer>.Filter.Where(p => p.IsDeleted == false));

            Customer uu = collection.Find(filter).ToList().Single();
            uu.StrTaxpayerType = "";

            if (uu.TaxPayer == TaxPayerType.VAT)
                uu.StrTaxpayerType = "VAT";

            if (uu.TaxPayer == TaxPayerType.NONVAT)
                uu.StrTaxpayerType = "NON-VAT";

            uu.StrCustomerId = uu.Id.ToString();
            return uu;

        }

        public static User ifValidLoginSpectrumServices(User user)
        {
            PasswordHash ph = new PasswordHash(user.Password);
            var db = conDB.client.GetDatabase("DBSPECTRUMSERVICES");
            var collection = db.GetCollection<User>("User");
            var filter = Builders<User>.Filter.And(
    Builders<User>.Filter.Where(p => p.Username.Equals(user.Username)),
    Builders<User>.Filter.Where(p => p.IsDeleted == false));

            User userr = collection.Find(filter).ToList().SingleOrDefault(u => u.Username.Equals(user.Username));


            if (userr != null)
            {
                ph = new PasswordHash(userr.bSalt, userr.bHash);
                userr.IsValidCredentials = ph.Verify(user.Password);
            }

            return userr;

        }

        public static void InitializeUser()
        {
            try
            {
                Customer customer = new Customer();
                customer.Firstname = "SIMPLE";
                customer.Lastname = "ADMIN";
                customer.TinNumber = "";
                customer.Address = "";
                customer.Businessname = "";

                var db = conDB.client.GetDatabase("DBSIMPLETAX");

                var collection = db.GetCollection<Customer>("Customer");
                User uuser = new User();
                uuser.Firstname = customer.Firstname;
                uuser.Lastname = customer.Lastname;
                uuser.Username = "simpleadmin";
                uuser.Password = "admin123";
                PasswordHash ph = new PasswordHash(uuser.Password);
                uuser.bHash = ph.Hash;
                uuser.bSalt = ph.Salt;
                customer.Credentials = uuser;

                collection.InsertOne(customer);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.StackTrace);
            }
        }

        public static void InitializeUserSpectrumServices()
        {
            try
            {
                User user = new User();
                user.Firstname = "Victor";
                user.Lastname = "Lim";
                user.IsAdmin = true;
                user.Username = "victor.admin";
                user.Password = "adminadmin";

                var db = conDB.client.GetDatabase("DBSPECTRUMSERVICES");
                var collection = db.GetCollection<User>("User");

                PasswordHash ph = new PasswordHash(user.Password);
                user.bHash = ph.Hash;
                user.bSalt = ph.Salt;

                collection.InsertOne(user);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.StackTrace);
            }
        }
        #endregion

        #region SPECTRUM SERVICES METHODS

        #region FHCC OVERVIEW

        public static List<DirectSalesDaily> GetDirectSalesDailyBoxesSold()
        {
            DateTime dteNow = DateTime.Parse("01/01/2021");
            DateTime dteFirstDay = DateTime.Parse("03/31/2021");
            //DateTime dteNow = DateTime.Parse(DateTime.Now.ToLocalTime().ToShortDateString());
            //DateTime dteFirstDay = DateTime.Parse(DateTime.Now.ToLocalTime().ToShortDateString());
            List<DirectSalesDaily> lstDS = new List<DirectSalesDaily>();
            List<DirectSalesDaily> ddOC;
            try
            {
                var db = conDB.client.GetDatabase("DBFH");
                var collection = db.GetCollection<DirectSalesDaily>("DirectSalesDaily");
                var filter = Builders<DirectSalesDaily>.Filter.And(
        Builders<DirectSalesDaily>.Filter.Where(p => p.isDeleted == false),
        Builders<DirectSalesDaily>.Filter.Gte("DateOrdered", dteNow),
         Builders<DirectSalesDaily>.Filter.Lte("DateOrdered", dteFirstDay));

                lstDS = collection.Find(filter).ToList();
                

                //foreach (DirectSalesDaily ds in lstDS)
                //{
                //    ds.strClientFullName = ds.Client.LastName + ", " + ds.Client.FirstName;
                //    ds.strBankName = ds.Bank.Description;

                //    ds.strCancelled = ds.isCancelled ? "YES" : "NO";

                //    ds.strDateOrdered = ds.DateOrdered.ToShortDateString();
                //    ds.Client.strFullName = ds.Client.LastName + ", " + ds.Client.FirstName;
                //}
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.StackTrace);
            }
            return ddOC = new List<DirectSalesDaily>(lstDS);

        }

        public static List<DirectSalesDaily> GetDirectSalesDailyBoxesSold(string strDateUpTo)
        {
            string strDay = "1";
            int iMonth = DateTime.Now.Month;
            int iYear = DateTime.Now.Year;

            string strDateNow = iMonth.ToString() + "/" + strDay + "/" + iYear.ToString();

            DateTime dteNow = DateTime.Parse(strDateNow);
            DateTime dteDateUpTo = DateTime.Parse(strDateUpTo);
            List<DirectSalesDaily> lstDS = new List<DirectSalesDaily>();
            List<DirectSalesDaily> ddOC;
            try
            {
                var db = conDB.client.GetDatabase("DBFH");
                var collection = db.GetCollection<DirectSalesDaily>("DirectSalesDaily");
                var filter = Builders<DirectSalesDaily>.Filter.And(
        Builders<DirectSalesDaily>.Filter.Where(p => p.isDeleted == false),
        Builders<DirectSalesDaily>.Filter.Gte("DateOrdered", dteNow),
         Builders<DirectSalesDaily>.Filter.Lte("DateOrdered", dteDateUpTo));

                lstDS = collection.Find(filter).ToList();


                //foreach (DirectSalesDaily ds in lstDS)
                //{
                //    ds.strClientFullName = ds.Client.LastName + ", " + ds.Client.FirstName;
                //    ds.strBankName = ds.Bank.Description;

                //    ds.strCancelled = ds.isCancelled ? "YES" : "NO";

                //    ds.strDateOrdered = ds.DateOrdered.ToShortDateString();
                //    ds.Client.strFullName = ds.Client.LastName + ", " + ds.Client.FirstName;
                //}
            }
            catch (Exception ex)
            {
                Debug.Assert(true, ex.StackTrace);
            }
            return ddOC = new List<DirectSalesDaily>(lstDS);

        }

        public static List<Products> LoadProductList()
        {

            var db = conDB.client.GetDatabase("DBFH");
            var collection = db.GetCollection<Products>("Products");

            var filter = Builders<Products>.Filter.And(
    Builders<Products>.Filter.Where(p => p.isDeleted == false));

            return collection.Find(filter).ToList().OrderBy(a => a.ProductName).ToList();
        }



        #endregion

        #endregion
    }
}
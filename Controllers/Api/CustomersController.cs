using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SimpleTax.Controllers.Api
{
    public class CustomersController : ApiController
    {
        static ConnectionDB conDB = new ConnectionDB();


        //GET /api/customers
        public IEnumerable<Customer> GetCustomers()
        {
            return DatabaseHelpers.LoadCustomers();
        }

        //GET /api/customers/id
        public Customer GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var customer = DatabaseHelpers.LoadCustomers().SingleOrDefault(m => m.Id == new MongoDB.Bson.ObjectId(id));

            if (customer == null || customer.Id == ObjectId.Empty)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return customer;
        }

        [HttpPost]
        public IHttpActionResult SaveCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            DatabaseHelpers.SaveCustomer(customer);

            return Created(new Uri(Request.RequestUri + "/" + customer.Id.ToString()), customer);
        }

        [HttpPut]
        public bool UpdateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return DatabaseHelpers.UpdateCustomer(customer);
        }

        [HttpDelete]
        public bool DeleteCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            return DatabaseHelpers.DeleteCustomer(id);
        }
    }
}

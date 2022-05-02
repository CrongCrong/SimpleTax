using System.Collections.Generic;

namespace SimpleTax
{
    public class CustomerViewModel
    {
        public string CustomerId { get; set; }
        public Customer customer { get; set; }

        public List<Customer> ListCustomers { get; set; }

        public List<string> TaxPayerTypeList { get; set; }
    }
}
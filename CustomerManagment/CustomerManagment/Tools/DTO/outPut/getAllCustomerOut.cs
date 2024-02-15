using CustomerManagment.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerService.DTO
{
    public class getAllCustomerOut
    {
        public int resultCode { get; set; }
        public string message { get; set; }
        public List<Customer> customers { get; set; }

        public getAllCustomerOut()
        {
            customers = new List<Customer>();
        }

    }
}
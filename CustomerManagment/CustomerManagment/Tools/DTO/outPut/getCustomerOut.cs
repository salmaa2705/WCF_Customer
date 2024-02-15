using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerService.DTO
{
    public class getCustomerOut
    {
        public int resultCode { get; set; }
        public string message
        {
            get; set;
        }
        public CustomerManagment.DAL.Customer customer { get; set; }
    }
}
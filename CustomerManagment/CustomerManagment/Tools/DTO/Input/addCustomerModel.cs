using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerService.DTO.Input
{
    public class addCustomerModel
    {
        public string customerName { get; set; }
        public string customerLastName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhoneNumber { get; set; }
        public string customerAddress { get; set; }
    }
}
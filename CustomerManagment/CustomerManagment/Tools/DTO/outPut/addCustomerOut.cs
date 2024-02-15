using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerService.DTO
{
    //output of add customer 
    public class addCustomerOut
    {
        public int  resultCode {  get; set; }
        public string message { get; set; }
    }
}
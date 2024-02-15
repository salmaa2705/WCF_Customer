using CustomerManagment.DAL;
using CustomerService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagment.CustomerService
{
    public interface ICustomerRepository
    {
        void insertCustomer(Customer c);
        //Customer getCustomerByID(int id);
        getAllCustomerOut getAllCustomer();
        void  deleteCustomer(int customer_id);
        updateCustomerOut updateCustomer(Customer customer);

        getCustomerOut getCustomer(int customer_id);

    }
}
using CustomerManagment.DAL;
using CustomerService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerManagment.CustomerService
{
    public class CustomerRepository : ICustomerRepository
    {

        // CustomerRepository constructor initializes the CustomerDBEntities for database operations
        private readonly CustomerDBEntities _customerDBEntities;


        //Constructeur de la classe
        //public CustomerRepository()
        //{
        //    // Create a new instance of CustomerDBEntities to interact with the database
        //    _customerDBEntities = new CustomerDBEntities();
        //}
        public CustomerRepository(CustomerDBEntities CustomerDBEntities)
        {
            // Create a new instance of CustomerDBEntities to interact with the database
            _customerDBEntities = CustomerDBEntities;
        }

        //--------------- Delete customer ----------------//
        public void deleteCustomer(int customer_id)
        {
            #region delete customer 
            try
            {
                //Check if customer id is >= 0
                if (!string.IsNullOrEmpty(customer_id.ToString()) && customer_id >= 0)
                {
                    //Find the customer to remove based on the provided customer_id
                    var customerToRemove = _customerDBEntities.Customers.FirstOrDefault(e => e.customer_id == customer_id);
                    //Check if a customer was found based on the provided customer_id
                    if (customerToRemove != null)
                    {
                        // If customer found, remove it from the DbContext and save changes to the database
                        _customerDBEntities.Customers.Remove(customerToRemove);
                        _customerDBEntities.SaveChanges();
                    }
                    else
                    {
                        //customer n'existe pas 

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("exception " + ex.Message);
            }
            #endregion

        }

        //--------------- Insert Customer ---------------//
        public void insertCustomer(Customer c)
        {
            try
            {
                //Check if customer is not empty
                if (c != null)
                {
                    //verifier l'existence du customer avant l'insertion 


                    //If customer !=null and does not exist in the data base, add it to the database
                    _customerDBEntities.Customers.Add(c);
                    _customerDBEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        //-------------- Update Customer ----------------//
        public updateCustomerOut updateCustomer(Customer customer)
        {
            //verification de la validité de l'input : 


            //output instance
            updateCustomerOut updatecustomerout = new updateCustomerOut();
            try
            {
                var existingCustomer = _customerDBEntities.Customers.Where(c =>
                   c.customer_id == customer.customer_id).FirstOrDefault();
                if (existingCustomer != null)
                {
                    //update fields 
                    existingCustomer.first_name = customer.first_name;
                    existingCustomer.last_name = customer.last_name;
                    existingCustomer.email = customer.email;
                    existingCustomer.address = customer.address;
                    existingCustomer.phone_number = customer.phone_number;

                    //save changes
                    _customerDBEntities.SaveChanges();
                    //display output
                    updatecustomerout.message = "Update successful!";
                    updatecustomerout.resultCode = 0;

                }
                else
                {
                    updatecustomerout.message = "Update failed";
                    updatecustomerout.resultCode = -1;
                }
            }
            catch (Exception Exp)
            {

                updatecustomerout.message = "Exception Error";
                updatecustomerout.resultCode = -2;
            }

            return updatecustomerout;


        }

        //-------------- Get All Customers ----------------//
        public getAllCustomerOut getAllCustomer()
        {
            getAllCustomerOut getAllCustomerOut = new getAllCustomerOut();
            #region get all customers
            try
            {
                //create list of customers
                List<Customer> customers = _customerDBEntities.Customers.ToList();
                if (customers != null)
                {
                    //display output
                    getAllCustomerOut.resultCode = 0;
                    getAllCustomerOut.message = "list is not empty";
                    //add customers to list of cutomer
                    getAllCustomerOut.customers = customers;

                }
                else
                {
                    //error output 
                    getAllCustomerOut.resultCode = -1;
                    getAllCustomerOut.message = "list is  empty";
                }
            }
            catch (Exception Exp)
            {
                getAllCustomerOut.resultCode = -2;
                getAllCustomerOut.message = "Exception";
                //log

            };

            return getAllCustomerOut;
            #endregion
        }


        //--------------- Get One Customer by ID ---------- //
        public getCustomerOut getCustomer(int customer_id)
        {
            //instanciation de l'output
            getCustomerOut getCustomerOutput = new getCustomerOut();
            // verification the l'input

            #region get customer by ID
            //check matched customer
            var matchedCustomer = _customerDBEntities.Customers.FirstOrDefault(c => c.customer_id == customer_id);
            //test if there is a matched customer
            try
            {
                if (matchedCustomer != null)
                {
                    //display output
                    getCustomerOutput.message = "customer exist";
                    getCustomerOutput.resultCode = 0;
                    //add matched customer to cutomer list
                    getCustomerOutput.customer = matchedCustomer;
                }
                else
                {
                    //displey output 
                    getCustomerOutput.message = "customer doesn't exist";
                    getCustomerOutput.resultCode = -1;
                }
            }
            catch (Exception Exp)
            {
                //exception
                getCustomerOutput.message = "exception!";
                getCustomerOutput.resultCode = -2;
            }
            #endregion
            return getCustomerOutput;
        }




    }
}
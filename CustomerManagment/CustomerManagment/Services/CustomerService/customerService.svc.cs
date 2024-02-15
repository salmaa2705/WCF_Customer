

using CustomerManagment.DAL;
using CustomerManagment.Tools;
//using CustomerManagment.Tools.DTO;
using CustomerService;
using CustomerService.DTO;
using CustomerService.DTO.Input;
using log4net;
//using Serilog;
//using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;


namespace CustomerManagment.CustomerService
{

    public class customerService : IcustomerService
    {
        //RC: 0:OK 
        //    -1:KO 
        //-2 exception

        public ICustomerRepository customerRepository;

        CustomerDBEntities ctx;

        //Default constructor 
        public customerService()
        {
            ctx = new CustomerDBEntities();
            customerRepository = new CustomerRepository(ctx);
        }
        //parameterized constructor
        public customerService(ICustomerRepository repository)
        {
            customerRepository = repository;
        }


        //log instancitiation 
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        #region CRUD with DbContext EF
        //******************* ADD CUSTOMER **************//
        public addCustomerOut addCustomer(addCustomerModel customerModel)
        {
            // Initialize logMessage for logging
            string logMessage = "";
            //instanciation de l'output de la methode
            addCustomerOut customerOut = new addCustomerOut();
            //instanciation of cutomer model 
            addCustomerModel model = new addCustomerModel();
            #region insert customer in dataBase
            //declaration du type de l'operation
            string opType = "1";
            //block gestion des exception Try / catch
            try
            {
                //verification de la validité de l'input 
                if (customerModel != null)
                {
                    //instanciation de la class treatments
                    Treatments Treat = new Treatments();
                    // check  data
                    int resultcheckCustomer = Treat.checkCustomer(opType, customerModel);
                    //si le checkCustomer est OK : Les données sont valides
                    if (resultcheckCustomer == 0)
                    {
                        Customer customerToInsert = new Customer();
                        customerToInsert.email = customerModel.customerEmail;
                        customerToInsert.first_name = customerModel.customerName;
                        customerToInsert.last_name = customerModel.customerLastName;
                        customerToInsert.address = customerModel.customerAddress;
                        customerToInsert.phone_number = customerModel.customerPhoneNumber;
                        //insert customer in dataBase
                        ctx.Customers.Add(customerToInsert);
                        ctx.SaveChanges();
                        customerOut.resultCode = 0;
                        customerOut.message = "Insertion customer avec succée";
                    }
                    else //si le checkCustomer est KO : Les données sont Invalides
                    {
                        customerOut.resultCode = -1;
                        customerOut.message = "Insertion customer echoue";
                    }
                }
            }
            catch (Exception Exp)
            {
                // Log the exception
                logMessage += "Exception caught in addCustomer method:\n";
                logMessage += "Exception Message: " + Exp.Message + "\n";
                logMessage += "StackTrace: " + Exp.StackTrace + "\n";

                // You should add appropriate logging here to capture information about the exception.
                Tools.Log.WriteLine(logMessage); // Assuming you have a Log class or method for writing logs.

                customerOut.resultCode = -2;
                customerOut.message = "Insertion customer echoue";

            }
            #endregion

            return customerOut;

        }

        //******************* GET CUSTOMER BY ID **************//
        public getCustomerOut GetCustomer(int id)
        {
            //instanciation de l'output
            getCustomerOut getCustomerOutput = new getCustomerOut();
            //instanciation du context
            #region get customer by ID
            //check matched customer
            var matchedCustomer = ctx.Customers.FirstOrDefault(c => c.customer_id == id);
            //test if there is a matched customer
            try
            {
                if (matchedCustomer != null && !string.IsNullOrEmpty(id.ToString()) && id >= 0)
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

        //******************* UPDATE CUSTOMER **************//
        public updateCustomerOut updateCustomer(Customer customer)
        {

            //output instance
            updateCustomerOut updatecustomerout = new updateCustomerOut();
            #region update cutomer database
            //check if costumer exist 
            if(customer != null)
            {
                try
                {
                    //check existing customer 
                    var existingCustomer = ctx.Customers.Where(c =>
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
                        ctx.SaveChanges();
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
            }
            #endregion
            return updatecustomerout;
        }

        //******************* GET ALL CUSTOMERS **************//
        public getAllCustomerOut GetAll()
        {

            //output instance
            getAllCustomerOut getAllCustomerOut = new getAllCustomerOut();
            #region get all customers
            try
            {
                //create list of customers
                List<Customer> customers = ctx.Customers.ToList();
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

            }
            #endregion
            return getAllCustomerOut;

        }

        //******************* DELETE CUSTOMER BY ID **************//
        public deleteCustomerOut deletecCustomer(int id)
        {

            //output instance
            deleteCustomerOut deletecustomerout = new deleteCustomerOut();
            #region delete customer
            //check matched customer
            var matchedCustomer = ctx.Customers.FirstOrDefault(c => c.customer_id == id);
            if (matchedCustomer != null && !string.IsNullOrEmpty(id.ToString()) && id >= 0)
            {
                //remove customer
                ctx.Customers.Remove(matchedCustomer);
                ctx.SaveChanges();
                //display output
                deletecustomerout.resultCode = 0;
                deletecustomerout.message = "Delete successful!";
            }
            else
            {
                //erreur output       
                deletecustomerout.resultCode = -1;
                deletecustomerout.message = "Delete failed!";
            }
            #endregion
            return deletecustomerout;
        }

        #endregion

        #region CRUD With Repository DataAccess
        //***************** ADD CUSTOMER ****************//
        addCustomerOut IcustomerService.addCustomerWithRepo(Customer customer)
        {
            addCustomerOut customerOut = new addCustomerOut();
            try
            {
                //verification de la validité de l'input
                if (customer == null)
                {
                    //call insertCustomer methode
                    customerRepository.insertCustomer(customer);
                    //display output message and result code
                    customerOut.resultCode = 0;
                    customerOut.message = "Customer added successfully! ";
                }
                else
                {
                    customerOut.resultCode = -1;
                    customerOut.message = "Customer added Failed! ";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception" + ex.Message);
                customerOut.resultCode = -2;
                customerOut.message = "Exception! ";
            }
            return customerOut;
        }

        //**************** DELETE CUSTOMER ************//
        public deleteCustomerOut deleteCustomerWithRepo(int customer_id)
        {
            deleteCustomerOut deleteCustomerOut = new deleteCustomerOut();
            try
            {
                //verification de la validité de l'input
                if (!string.IsNullOrEmpty(customer_id.ToString()) && customer_id >= 0)
                {
                    //call delete methode
                    customerRepository.deleteCustomer(customer_id);
                    deleteCustomerOut.resultCode = 0;
                    deleteCustomerOut.message = "Delete customer succeded!";
                }
                else
                {
                    deleteCustomerOut.resultCode = -1;
                    deleteCustomerOut.message = "Delete customer failed!";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("exception service " + ex.Message);
                deleteCustomerOut.resultCode = -2;
                deleteCustomerOut.message = "Exception!";
            }
            return deleteCustomerOut;
        }

        //**************** GET ALL CUSTOMERS *****************//
        public getAllCustomerOut getAllCustomerWithRepo()
        {
            // Create an instance of getAllCustomerOut to store the result
            getAllCustomerOut getAllCustomerOut = new getAllCustomerOut();
            try
            {
                customerRepository.getAllCustomer();
                getAllCustomerOut.resultCode = 0;
                getAllCustomerOut.message = "Get All customers succeeded!";

            }
            catch (Exception e)
            {
                Console.WriteLine("exception" + e.Message);
                getAllCustomerOut.resultCode = -1;
                getAllCustomerOut.message = "Get All customers failed!";
            }
            return getAllCustomerOut;
        }

        //************** GET ONE CUSTOMER BY ID ***********//
        public getCustomerOut getCustomerWithRepo(int id)
        {
            // Create an instance of getCustomerOut to store the result   
            getCustomerOut getcustomerout = new getCustomerOut();
            try
            {
                //verification de la validité de l'input
                if (!string.IsNullOrEmpty(id.ToString()) && id >= 0)
                {
                    // If customerRepository is not null and id is non-negative, return the result of getCustomer() method
                    customerRepository.getCustomer(id);
                    getcustomerout.resultCode = 0;
                    getcustomerout.message = "Get customer by id succeeded";
                }
                else
                {
                    getcustomerout.resultCode = -1;
                    getcustomerout.message = "Get customer by id failed";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("exception" + e.Message);
                getcustomerout.resultCode = -2;
                getcustomerout.message = "Exception";
            }
            return getcustomerout;
        }

        //************** Update Customer ***********//
        public updateCustomerOut updateCustomerWithRepo(Customer customer)
        {
            // Create an instance of updateCustomerOut to store the result
            updateCustomerOut updateCustomerOut = new updateCustomerOut();
            try
            {
                //verification de la validité de l'input


                // return the result of getAllCustomer() method
                return customerRepository.updateCustomer(customer);

            }
            catch (Exception e)
            {
                Console.WriteLine("exception" + e.Message);
            }
            return updateCustomerOut;
        }
        #endregion

        #region CRUD with sql native

        // --------------- ADD Customer ------------ //
        public addCustomerOut addCustomerSql(Customer customer)
        {
            
            //instanciation de l'output de la methode
            addCustomerOut customerOut = new addCustomerOut();
            // Initialize logMessage for logging
            string logMessage = "";

            //Start Logging information about add customer method
            logMessage += "**********************************************************(ADD CUSTOMER METHOD WITH SQL NATIVE)********************************************************************\n";
            logMessage += DateTime.Now.ToString("MMM ddd d HH:mm yyyy") + "  " + "Début de l'opération :\n";
            //Log.WriteLine(logMessage);
            try
            {
                // Creating a new SqlConnection // using provide ressource release no need for dispose() 
                using (SqlConnection con = new SqlConnection("Data Source=TEC-SALMA-MOUEL;initial catalog=CustomerDB;integrated security=True;multipleactiveresultsets=True;encrypt=False;application name=EntityFramework"))
                {

                    // Establish a connection to the database
                    SqlCommand cmd = new SqlCommand();
                    // SQL query to add customer to customers tables
                    string Query = @"INSERT INTO Customer (first_name,last_name,email,phone_number,address)  
                                               Values(@first_name,@last_name,@email,@phone_number,@address)";
                    // Set up the command with the query and connection
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@first_name", customer.first_name);
                    cmd.Parameters.AddWithValue("@last_name", customer.last_name);
                    cmd.Parameters.AddWithValue("@email", customer.email);
                    cmd.Parameters.AddWithValue("@phone_number", customer.phone_number);
                    cmd.Parameters.AddWithValue("@address", customer.address);
                    // Open db connection
                    con.Open();
                    // Execute the query and get the result set
                    cmd.ExecuteNonQuery();
                    // Close db connection
                    con.Close();
                }
            }
            catch (Exception Exp)
            {

                customerOut.resultCode = -2;
                customerOut.message = "Exception! Customer added failed";
                // Log the exception
                logMessage += "***********Exception caught in addCustomer method:\n************";
                logMessage += "***********Exception Message:*************" + Exp.Message + "\n*************";
                logMessage += "Result code " + customerOut.resultCode + "\n";
                logMessage += "Exception message" + customerOut.message + "\n";
                // You should add appropriate logging here to capture information about the exception.
                
                
            }
            Tools.Log.WriteLine(logMessage);

            return customerOut;
        }
        // -------------- GET CUSTOMER BY ID ---------------------- //
        public getCustomerOut getCustomerSql(int customer_id)
        {
            // Initialization of the Serilog logger
            //SeriLog.ConfigureLogger();
            //output instance
            getCustomerOut getCustomerOut = new getCustomerOut();
            // Establish a connection to the database
            SqlConnection con = new SqlConnection("Data Source=TEC-SALMA-MOUEL;initial catalog=CustomerDB;integrated security=True;multipleactiveresultsets=True;encrypt=False;application name=EntityFramework");
            try
            {
                // Logging information about add customer method
                //Serilog.Log.Information("**********************************************************(GET CUSTOMER WITH ID METHOD WITH SQL NATIVE)********************************************************************");
                //Serilog.Log.Information("Début de l'opération : {Timestamp:MMM ddd d HH:mm yyyy}", DateTime.Now);
                SqlCommand cmd = new SqlCommand();
                // SQL query to select records from the Customer table according to the giving id
                string Query = "SELECT * FROM Customer WHERE customer_id = @customer_id";
                // Set up the command with the query and connection
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                // Open db connection
                con.Open();
                // Execute the query and get the result set
                SqlDataReader reader = cmd.ExecuteReader();
                // Check if record was found
                if (reader.Read())
                {
                    // Create a Customer object for the record with the specified ID
                    Customer customer = new Customer
                    {
                        first_name = reader["first_name"].ToString(),
                        last_name = reader["last_name"].ToString(),
                        email = reader["email"].ToString(),
                        phone_number = reader["phone_number"].ToString(),
                        address = reader["address"].ToString()
                    };
                    // Assign the retrieved Customer object to the getCustomerOut instance                
                    getCustomerOut.customer = customer;
                    // Set result code and message in case of success
                    getCustomerOut.resultCode = 0;
                    getCustomerOut.message = "Fetching customer by ID succeeded";
                }
                else
                {
                    getCustomerOut.resultCode = -1;
                    getCustomerOut.message = "Customer not found";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching customer by ID: " + ex.Message);
                // Set error code and message in case of an exception
                getCustomerOut.resultCode = -1;
                getCustomerOut.message = "Error fetching customer by ID";
                //Handle the exception as needed
                //Serilog.Log.Error(ex, "Exception caught in getCustomer method:\nResult code {ResultCode}\nException message {Message}", getCustomerOut.resultCode, getCustomerOut.message);
            }
            finally
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                {
                    // Clode DB
                    con.Close();
                    Console.WriteLine("connection closed");
                }
                con?.Dispose();
            }

            //Serilog.Log.CloseAndFlush();

            return getCustomerOut;
        }

        //-------------- GET ALL Customers SQL NATIVE -------------- //
        public getAllCustomerOut getAllCustomerSql()
        {
            //output instance
            getAllCustomerOut getAllCustomerOut = new getAllCustomerOut();
            // Create a list to store customer records
            List<Customer> customers = new List<Customer>();
           
            try
            {
                // Establish a connection to the database
                using (SqlConnection con = new SqlConnection("Data Source=TEC-SALMA-MOUEL;initial catalog=CustomerDB;integrated security=True;multipleactiveresultsets=True;encrypt=False;application name=EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand();
                    // SQL query to select all records from the Customer table
                    string Query = "SELECT * FROM Customer";
                    // Set up the command with the query and connection
                    cmd = new SqlCommand(Query, con);
                    // Open the database connection
                    con.Open();
                    // Execute the query and get the result set
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Loop through each record in the result set
                    while (reader.Read())
                    {
                        // Create a Customer object for each record
                        Customer customer = new Customer
                        {
                            first_name = reader["first_name"].ToString(),
                            last_name = reader["last_name"].ToString(),
                            email = reader["email"].ToString(),
                            phone_number = reader["phone_number"].ToString(),
                            address = reader["address"].ToString()
                        };
                        // Add the Customer object to the list of customers
                        customers.Add(customer);
                        getAllCustomerOut.customers = customers;
                        //set output error code and message in case of sucess
                        getAllCustomerOut.resultCode = 0;
                        getAllCustomerOut.message = "Fetching list of customers succeeded";

                    }
                    // Close db connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                                
                // Set error code and message in case of an exception
                getAllCustomerOut.resultCode = -1;
                getAllCustomerOut.message = "Error fetching list of customers";
                
            }
           
            return getAllCustomerOut;
        }

        // ------------- UPDATE CUSTOMER SQL NATIVE --------------- //
        public updateCustomerOut updateCustomerSql(Customer customer)
        {
            // Create an output instance
            updateCustomerOut updateCustomerOut = new updateCustomerOut();

            try
            {
                // Establish a connection to the database
                using (SqlConnection con = new SqlConnection("Data Source=TEC-SALMA-MOUEL;initial catalog=CustomerDB;integrated security=True;multipleactiveresultsets=True;encrypt=False;application name=EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand();

                    // SQL query to update customer information
                    string Query = @"UPDATE Customer 
                         SET first_name = @first_name, 
                             last_name = @last_name, 
                             email = @email, 
                             phone_number = @phone_number, 
                             address = @address 
                         WHERE customer_id = @customer_id";

                    // Set up the command with the query and connection
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@customer_id", customer.customer_id);
                    cmd.Parameters.AddWithValue("@first_name", customer.first_name);
                    cmd.Parameters.AddWithValue("@last_name", customer.last_name);
                    cmd.Parameters.AddWithValue("@email", customer.email);
                    cmd.Parameters.AddWithValue("@phone_number", customer.phone_number);
                    cmd.Parameters.AddWithValue("@address", customer.address);

                    // Open the database connection
                    con.Open();

                    // Execute the query to update the customer information
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if the customer information was successfully updated
                    if (rowsAffected > 0)
                    {
                        updateCustomerOut.resultCode = 0;
                        updateCustomerOut.message = "Customer information updated successfully!";
                    }
                    else
                    {
                        updateCustomerOut.resultCode = -1;
                        updateCustomerOut.message = "Customer not found or update failed";
                    }

                    // Close the database connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Log the error and set error code and message in case of an exception
                Console.WriteLine("Error updating customer information: " + ex.Message);
                updateCustomerOut.resultCode = -1;
                updateCustomerOut.message = "Error updating customer information";
            }

            return updateCustomerOut;
        }

        // ------------------ DELETE CUSTOMER SQL NATIVE ---------- //
        public deleteCustomerOut deleteCustomerSql(int customer_id)
        {
            // Create an output instance
            deleteCustomerOut deleteCustomerOut = new deleteCustomerOut();

            try
            {
                // Establish a connection to the database
                using (SqlConnection con = new SqlConnection("Data Source=TEC-SALMA-MOUEL;initial catalog=CustomerDB;integrated security=True;multipleactiveresultsets=True;encrypt=False;application name=EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand();

                    // SQL query to delete a customer by ID
                    string Query = "DELETE FROM Customer WHERE customer_id = @customer_id";

                    // Set up the command with the query and connection
                    cmd = new SqlCommand(Query, con);
                    cmd.Parameters.AddWithValue("@customer_id", customer_id);

                    // Open the database connection
                    con.Open();

                    // Execute the query to delete the customer
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if the customer was successfully deleted
                    if (rowsAffected > 0)
                    {
                        deleteCustomerOut.resultCode = 0;
                        deleteCustomerOut.message = "Customer deleted successfully";
                    }
                    else
                    {
                        deleteCustomerOut.resultCode = -1;
                        deleteCustomerOut.message = "Customer not found or deletion failed";
                    }

                    // Close the database connection
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Log the error and set error code and message in case of an exception
                Console.WriteLine("Error deleting customer: " + ex.Message);
                deleteCustomerOut.resultCode = -1;
                deleteCustomerOut.message = "Error deleting customer";
            }

            return deleteCustomerOut;
        }
        #endregion

    }
}

using CustomerManagment.DAL;
using CustomerService.DTO;
using CustomerService.DTO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CustomerManagment.CustomerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IcustomerService" in both code and config file together.
    [ServiceContract]
    public interface IcustomerService
    {

        #region CRUD operations with dbContext
        [OperationContract]
        addCustomerOut addCustomer(addCustomerModel c);

        [OperationContract]
        getCustomerOut GetCustomer(int id);

        [OperationContract]
        getAllCustomerOut GetAll();

        [OperationContract]
        updateCustomerOut updateCustomer(Customer customer);

        [OperationContract]
        deleteCustomerOut deletecCustomer(int id);

        #endregion


        #region CRUD operations with repository
        [OperationContract]
        addCustomerOut addCustomerWithRepo(Customer customer);

        [OperationContract]
        getAllCustomerOut getAllCustomerWithRepo();

        [OperationContract]
        getCustomerOut getCustomerWithRepo(int id);

        [OperationContract]
        updateCustomerOut updateCustomerWithRepo(Customer customer);

        [OperationContract]
        deleteCustomerOut deleteCustomerWithRepo(int customer_id);
        #endregion


        #region CRUD operations with SQL native

        [OperationContract]
        addCustomerOut addCustomerSql(Customer customer);

        [OperationContract]
        getCustomerOut getCustomerSql(int customer_id);

        [OperationContract]
        getAllCustomerOut getAllCustomerSql();

        [OperationContract]
        updateCustomerOut updateCustomerSql(Customer customer);

        [OperationContract]
        deleteCustomerOut deleteCustomerSql(int customer_id);

        #endregion

    }
}

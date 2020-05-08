using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore
{
    interface IStoreDAO
    {
        Customer GetCustomerByUsername(string username);
        void CreateCustomer(Customer customer);

        List<Product> GetAllProducts();
        Product GetProductByID(int id);
        Product GetProductByName(string name);
        List<Product> GetAllSupplierProducts(int id);
        void CreateNewProduct(Product product);
        void UpdateProductQuantity(int id, int quantity);

        List<Order> GetCustomerOrders(int id);
        void CreateNewOrder(Order order);

        Supplier GetSupplierByUsername(string username);
        void CreateNewSupplier(Supplier supplier);

        List<LogRecord> GetAllLogRecords();
        void AddLogRecord(LogRecord logRecord);
    }
}

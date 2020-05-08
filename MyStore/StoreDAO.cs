using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore
{
    /// <summary>
    /// This is the data access class. It handles the connection to the SQL database.
    /// All the methods in this class use SQL stored procedures.
    /// </summary>
    public class StoreDAO : IStoreDAO
    {
        // Getting the connection string from the app.config file.
        private string connString = ConfigurationManager.AppSettings["connectionString"].ToString();

        // ***** Customers *****

        /// <summary>
        /// Gets a customer by its username.
        /// </summary>
        public Customer GetCustomerByUsername(string username)
        {
            Customer customer = new Customer();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_CUSTOMER_BY_USERNAME", conn);
                cmd.Parameters.Add(new SqlParameter("@username", username));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    customer.ID = (int)reader["ID"];
                    customer.UserName = (string)reader["Username"];
                    customer.Password = (string)reader["Password"];
                    customer.FirstName = (string)reader["FirstName"];
                    customer.LastName = (string)reader["LastName"];
                    customer.CreditNumber = (string)reader["CreditNumber"];
                }

                cmd.Connection.Close();
            }
            return customer;
        }

        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        public void CreateCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("CREATE_CUSTOMER", conn);
                cmd.Parameters.Add(new SqlParameter("@username", customer.UserName));
                cmd.Parameters.Add(new SqlParameter("@password", customer.Password));
                cmd.Parameters.Add(new SqlParameter("@firstname", customer.FirstName));
                cmd.Parameters.Add(new SqlParameter("@lastname", customer.LastName));
                cmd.Parameters.Add(new SqlParameter("@creditnumber", customer.CreditNumber));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
        }


        // ***** Products *****


        /// <summary>
        /// Gets all the products in the database.
        /// </summary>
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_ALL_PRODUCTS", conn);

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    if ((int)reader["Quantity"] == 0) // This method won't return out of stock products.
                        continue;

                    Product product = new Product();

                    product.ID = (int)reader["ID"];
                    product.Name = (string)reader["Name"];
                    product.SupplierName = (string)reader["Company"];
                    product.Price = (decimal)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];

                    products.Add(product);
                }

                cmd.Connection.Close();
            }
            return products;
        }

        /// <summary>
        /// Gets a certain product by its Id.
        /// </summary>
        public Product GetProductByID(int id)
        {
            Product product = new Product();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_PRODUCT_BY_ID", conn);
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    if ((int)reader["ID"] == 0)
                        throw new ProductDoesntExistException("Product doesn't exsit");

                    product.ID = (int)reader["ID"];
                    product.Name = (string)reader["Name"];
                    product.SupplierID = (int)reader["Supplier_ID"];
                    product.Price = (decimal)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                }

                cmd.Connection.Close();
            }
            return product;
        }

        /// <summary>
        /// Gets a product by its name.
        /// </summary>
        public Product GetProductByName(string name)
        {
            Product product = new Product();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_PRODUCT_BY_NAME", conn);
                cmd.Parameters.Add(new SqlParameter("@name", name));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    product.ID = (int)reader["ID"];
                    product.Name = (string)reader["Name"];
                    product.SupplierID = (int)reader["Supplier_ID"];
                    product.Price = (decimal)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                }
                cmd.Connection.Close();
            }
            return product;
        }

        /// <summary>
        /// Gets all the products of a certain supplier.
        /// </summary>
        public List<Product> GetAllSupplierProducts(int id)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_ALL_SUPPLIER_PRODUCTS", conn);
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    Product product = new Product();

                    product.ID = (int)reader["ID"];
                    product.Name = (string)reader["Name"];
                    product.Price = (decimal)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];

                    products.Add(product);
                }

                cmd.Connection.Close();
            }
            return products;
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        public void CreateNewProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("CREATE_PRODUCT", conn);
                cmd.Parameters.Add(new SqlParameter("@name", product.Name));
                cmd.Parameters.Add(new SqlParameter("@supplier_id", product.SupplierID));
                cmd.Parameters.Add(new SqlParameter("@price", product.Price));
                cmd.Parameters.Add(new SqlParameter("@quantity", product.Quantity));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Updates the quantity of a certain product to the given number.
        /// </summary>
        public void UpdateProductQuantity(int id, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SET_PRODUCT_QUANTITY", conn);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@quantity", quantity));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
        }


        // ***** Orders *****


        /// <summary>
        /// Gets all the orders that belongs to a certain customer.
        /// </summary>
        public List<Order> GetCustomerOrders(int id)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_ALL_CUSTOMER_ORDERS", conn);
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    Order order = new Order();

                    order.ID = (int)reader["ID"];
                    order.CustomerID = (int)reader["Customer_ID"];
                    order.ProductName = (string)reader["Name"];
                    order.Amount = (int)reader["Amount"];
                    order.TotalPrice = (decimal)reader["Total_Price"];

                    orders.Add(order);
                }

                cmd.Connection.Close();
            }
            return orders;
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        public void CreateNewOrder(Order order)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("CREATE_ORDER", conn);
                cmd.Parameters.Add(new SqlParameter("@customer_id", order.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@product_id", order.ProductID));
                cmd.Parameters.Add(new SqlParameter("@amount", order.Amount));
                cmd.Parameters.Add(new SqlParameter("@total_price", order.TotalPrice));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
        }


        // *****  Supplier  *****


        /// <summary>
        /// Gets a supplier by its username.
        /// </summary>
        public Supplier GetSupplierByUsername(string username)
        {
            Supplier supplier = new Supplier();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("GET_SUPPLIER_BY_USERNAME", conn);
                cmd.Parameters.Add(new SqlParameter("@username", username));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    supplier.ID = (int)reader["ID"];
                    supplier.UserName = (string)reader["UserName"];
                    supplier.Password = (string)reader["Password"];
                    supplier.Company = (string)reader["Company"];
                }

                cmd.Connection.Close();
            }
            return supplier;
        }

        /// <summary>
        /// Adds a new supplier to the database.
        /// </summary>
        public void CreateNewSupplier(Supplier supplier)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("CREATE_NEW_SUPPLIER", conn);
                cmd.Parameters.Add(new SqlParameter("@username", supplier.UserName));
                cmd.Parameters.Add(new SqlParameter("@password", supplier.Password));
                cmd.Parameters.Add(new SqlParameter("@company", supplier.Company));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
        }
        

        // ******  Log Records  ******


        /// <summary>
        /// Prints to the screen all the records from the log table.
        /// </summary>
        public List<LogRecord> GetAllLogRecords()
        {
            List<LogRecord> records = new List<LogRecord>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                //This procedure gets the last 10 records.
                SqlCommand cmd = new SqlCommand("GET_ALL_LOG_RECORDS", conn); 

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read() == true)
                {
                    LogRecord record = new LogRecord();

                    record.ID = (int)reader["ID"];
                    record.Date = (DateTime)reader["Date"];
                    record.Action = (string)reader["Action"];
                    record.Succeeded = (string)reader["Succeeded"];
                    record.FailCause = (string)reader["Fail_Cause"];

                    records.Add(record);
                }
                cmd.Connection.Close();
            }

            return records;
        }

        /// <summary>
        /// Adds a new log record to the database.
        /// </summary>
        public void AddLogRecord(LogRecord logRecord)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("ADD_LOG_RECORD", conn);
                cmd.Parameters.Add(new SqlParameter("@date", logRecord.Date));
                cmd.Parameters.Add(new SqlParameter("@action", logRecord.Action));
                cmd.Parameters.Add(new SqlParameter("@succeeded", logRecord.Succeeded));
                cmd.Parameters.Add(new SqlParameter("@fail_cause", logRecord.FailCause));

                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStore;
using MyStore.Forms;

namespace TestProject
{
    /// <summary>
    /// HOW THESE TESTS WORKS:
    /// There are two types of tests here:
    /// 1. ExpectedException - where the test injects some input that should cause an exception to be thrown.
    /// 2. Assert.AreEqual - where the test makes an action or injects information and then checks if it was recieved by the program correctly.
    /// All the tests are done with using dependency injection. See the UserInput class to understand how it works.
    /// </summary>

    [TestClass]
    public class StoreTests
    {
        #region Customer forms tests

        /// <summary>
        /// Trying to create a new customer account with a username that already exists.
        /// Expecting to get a "UserNameAlreadyExistException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserNameAlreadyExistException))]
        public void Customer_NewCustomerForm()
        {
            CustomerForms customerForms = new CustomerForms(TestMode.On);
            customerForms.input.Injections = new List<object>()
            {
                "jdoe", 
                "fake password",
                "fake name",
                "fake last name",
                "fake credit card"
            };
            customerForms.NewCustomerForm();
        }

        /// <summary>
        /// Trying to login with a wrong password.
        /// Expecting to get a "InvalidCredentialsException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Customer_LogIn()
        {
            CustomerForms customerForms = new CustomerForms(TestMode.On);
            customerForms.input.Injections = new List<object>()
            {
                "jdoe",
                "Wrong password" // Inserting a wrong password.
            };
            customerForms.CustomerLogInForm();
        }

        /// <summary>
        /// Trying to order a product that doesn't exist.
        /// Expecting to get a "ProductDoesntExistException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ProductDoesntExistException))]
        public void Customer_CreateNewOrder()
        {
            Customer customer = new Customer()
            {
                ID = 12,
                UserName = "jdoe",
                Password = "0000",
                FirstName = "John",
                LastName = "Doe",
                CreditNumber = "4580"
            };
            CustomerForms customerForms = new CustomerForms(TestMode.On, customer);
            customerForms.input.Injections = new List<object>()
            {
                1000,
                1 
            };
            customerForms.CreateNewOrder();
        }

        /// <summary>
        /// Trying to order a product with negative quantity number.
        /// Expecting to get a "OrderAmountCannotBeZeroOrNegativeNumberException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(OrderAmountCannotBeZeroOrNegativeNumberException))]
        public void Customer_CreateNewOrder2()
        {
            Customer customer = new Customer()
            {
                ID = 12,
                UserName = "jdoe",
                Password = "0000",
                FirstName = "John",
                LastName = "Doe",
                CreditNumber = "4580"
            };
            CustomerForms customerForms = new CustomerForms(TestMode.On, customer);
            customerForms.input.Injections = new List<object>()
            {
                23,
                -1
            };
            customerForms.CreateNewOrder();
        }

        /// <summary>
        /// Trying to order a product that is out of stock.
        /// Expecting to get a "ProductOutOfStockException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ProductOutOfStockException))]
        public void Customer_CreateNewOrder3()
        {
            Customer customer = new Customer()
            {
                ID = 12,
                UserName = "jdoe",
                Password = "0000",
                FirstName = "John",
                LastName = "Doe",
                CreditNumber = "4580"
            };
            CustomerForms customerForms = new CustomerForms(TestMode.On, customer);
            customerForms.input.Injections = new List<object>()
            {
                24,
                1
            };
            customerForms.CreateNewOrder();
        }

        /// <summary>
        /// Trying to order a product with quantity higher than available in stock.
        /// Expecting to get a "NotEnoughItemsInTheStockException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotEnoughItemsInTheStockException))]
        public void Customer_CreateNewOrder4()
        {
            Customer customer = new Customer()
            {
                ID = 12,
                UserName = "jdoe",
                Password = "0000",
                FirstName = "John",
                LastName = "Doe",
                CreditNumber = "4580"
            };
            CustomerForms customerForms = new CustomerForms(TestMode.On, customer);
            customerForms.input.Injections = new List<object>()
            {
                21,
                500
            };
            customerForms.CreateNewOrder();
        }

        /// <summary>
        /// Ordering a product and checking that the information of the created order is corrent;
        /// </summary>
        [TestMethod]
        public void Customer_CreateNewOrder5()
        {
            Customer customer = new Customer()
            {
                ID = 12,
                UserName = "jdoe",
                Password = "0000",
                FirstName = "John",
                LastName = "Doe",
                CreditNumber = "4580"
            };
            CustomerForms customerForms = new CustomerForms(TestMode.On, customer);
            customerForms.input.Injections = new List<object>()
            {
                16,
                1
            };
            customerForms.CreateNewOrder();

            List<Order> orders = new StoreDAO().GetCustomerOrders(customer.ID);

            // Getting the latest order:
            Order order = orders[orders.Count - 1];
            Assert.AreEqual(order.ProductName, "Batteries Pack");
            Assert.AreEqual(order.CustomerID, 12);
            Assert.AreEqual(order.Amount, 1);
            Assert.AreEqual(order.TotalPrice, 8);
        }


        #endregion

        // **** //

        #region Supplier forms tests

        /// <summary>
        /// Trying to create a new supplier account with a username that already exists.
        /// Expecting to get a "UserNameAlreadyExistException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserNameAlreadyExistException))]
        public void Supplier_NewSupplierForm()
        {
            SupplierForms supplierForms = new SupplierForms(TestMode.On);
            supplierForms.input.Injections = new List<object>()
            {
                "rshack",
                "fake password",
                "fake company name"
            };
            supplierForms.NewSupplierForm();
        }

        /// <summary>
        /// Trying to login with a wrong password.
        /// Expecting to get a "InvalidCredentialsException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Supplier_LogIn()
        {
            SupplierForms supplierForms = new SupplierForms(TestMode.On);
            supplierForms.input.Injections = new List<object>()
            {
                "rshack",
                "Wrong Password"
            };
            supplierForms.SupplierLogInForm();
        }

        /// <summary>
        /// Trying to update an existing product with a negative quantity.
        /// Expecting to get a "ProductQuantityCannotBeNegativeNumberException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ProductQuantityCannotBeNegativeNumberException))]
        public void Supplier_AddNewProduct()
        {
            Supplier supplier = new Supplier()
            {
                ID = 8,
                UserName = "rshack",
                Password = "1234",
                Company = "RadioShack"
            };
            SupplierForms supplierForms = new SupplierForms(TestMode.On, supplier);
            supplierForms.input.Injections = new List<object>()
            {
                "FM Radio",
                "y",
                -50
            };
            supplierForms.AddNewProduct();
        }

        /// <summary>
        /// Trying to add new product with a negative price.
        /// Expecting to get a "ProductPriceCannotBeNegativeException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ProductPriceCannotBeNegativeException))]
        public void Supplier_AddNewProduct2()
        {
            Supplier supplier = new Supplier()
            {
                ID = 8,
                UserName = "rshack",
                Password = "1234",
                Company = "RadioShack"
            };
            SupplierForms supplierForms = new SupplierForms(TestMode.On, supplier);
            supplierForms.input.Injections = new List<object>()
            {
                "Laptop",
                -1 
            };
            supplierForms.AddNewProduct();
        }

        /// <summary>
        /// Trying to add new product with a negative quantity.
        /// Expecting to get a "ProductQuantityCannotBeNegativeNumberException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ProductQuantityCannotBeNegativeNumberException))] 
        public void Supplier_AddNewProduct3()
        {
            Supplier supplier = new Supplier()
            {
                ID = 8,
                UserName = "rshack",
                Password = "1234",
                Company = "RadioShack"
            };
            SupplierForms supplierForms = new SupplierForms(TestMode.On, supplier);
            supplierForms.input.Injections = new List<object>()
            {
                "Laptop",
                "500",
                -1
            };
            supplierForms.AddNewProduct();
        }

        /// <summary>
        /// Adding a product and checking that the information of the created product is correct.
        /// This test will succeed only on the first run, beacuse you cannot add two products with the same name.
        /// </summary>
        [TestMethod]
        public void Supplier_AddNewProduct4()
        {
            Supplier supplier = new Supplier()
            {
                ID = 8,
                UserName = "rshack",
                Password = "1234",
                Company = "RadioShack"
            };
            SupplierForms supplierForms = new SupplierForms(TestMode.On, supplier);
            supplierForms.input.Injections = new List<object>()
            {
                "Laptop",
                "500",
                5
            };
            supplierForms.AddNewProduct();

            List<Product> products = new StoreDAO().GetAllSupplierProducts(supplier.ID);
            Product product = products[products.Count - 1];

            Assert.AreEqual(product.Name, "Laptop");
            Assert.AreEqual(product.Price, 500);
            Assert.AreEqual(product.Quantity, 5);

        }

        #endregion

        /// <summary>
        /// Trying to login as administrator with a wrong password.
        /// Expecting to get a "InvalidCredentialsException" exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Administrator_LogIn()
        {
            MainForm mainForm = new MainForm(TestMode.On);
            mainForm.input.Injections = new List<object>()
            {
                "Wrong Password",
            };
            mainForm.AdministratorScreen();
        }
    }
}

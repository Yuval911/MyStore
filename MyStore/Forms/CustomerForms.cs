using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Forms
{
    /// <summary>
    /// This class contains all the menus and actions of the customer.
    /// </summary>
    public class CustomerForms
    {
        private StoreDAO storeDAO = new StoreDAO();
        public UserInput input;
        private Customer currentCustomer;
        public TestMode mode;

        public CustomerForms(TestMode mode)
        {
            this.mode = mode;
            input = new UserInput(mode);
        }

        public CustomerForms(TestMode mode, Customer customer)
        {
            this.mode = mode;
            input = new UserInput(mode);
            currentCustomer = customer;
        }

        /// <summary>
        /// This is the initial menu where the user can sign up a new account or log in.
        /// </summary>
        public void SignInScreen()
        {
            Console.Clear();
            Console.WriteLine("\n Please select: \n");
            Console.WriteLine(" 1. Existing Customer");
            Console.WriteLine(" 2. New Customer");
            Console.WriteLine(" 3. < Go back to the main menu\n");

            int choice;

            while (true)
            {
                choice = input.GetUserInput<int>();

                switch (choice)
                {
                    case 1:
                        CustomerLogInForm();
                        return;
                    case 2:
                        NewCustomerForm();
                        return;
                    case 3:
                        MainForm mainForm = new MainForm(mode);
                        mainForm.StartScreen();
                        return;
                    default:
                        Console.Write("Invalid choice. please select one of the choices (1/2/3): ");
                        break;
                }
            }
        }

        /// <summary>
        /// This is the sign up form.
        /// </summary>
        public void NewCustomerForm()
        {
            Console.Clear();
            Customer customer = new Customer();

            Console.WriteLine("\n Please insert the following details: \n");

            #region FillForm:

            Console.Write(" 1. User Name: ");
            customer.UserName = input.GetUserInput<string>();

            Console.Write(" 2. Password: ");
            customer.Password = input.GetUserInput<string>();

            Console.Write(" 3. First Name: ");
            customer.FirstName = input.GetUserInput<string>();

            Console.Write(" 4. Last Name: ");
            customer.LastName = input.GetUserInput<string>();

            Console.Write(" 5. Credit Card Number: ");
            customer.CreditNumber = input.GetUserInput<string>();

            #endregion

            // The program will check if the selected username is taken or not.
            if (storeDAO.GetCustomerByUsername(customer.UserName).ID != 0)
            {
                storeDAO.AddLogRecord(
                    new LogRecord(DateTime.Now, $"A user attempted to create new customer account", "No", $"The username: {customer.UserName} Already exists"));

                HandleError(new UserNameAlreadyExistException("User name already exist"));
                return;
            }

            if (mode == TestMode.On)
                return;

            storeDAO.CreateCustomer(customer);

            Console.WriteLine("\n Your account created successfully! Press Enter to procced.");

            Console.ReadKey();

            storeDAO.AddLogRecord(
                new LogRecord(DateTime.Now, $"A new customer account has been created by user: {customer.UserName}", "Yes", ""));

            MainForm mainForm = new MainForm(mode);
            mainForm.StartScreen();
        }

        /// <summary>
        /// This is the log in screen.
        /// </summary>
        public void CustomerLogInForm()
        {
            Console.Clear();
            Console.WriteLine("\n Please insert your login credentials: ");

            string username;
            string password;

            Console.Write(" User name: ");
            username = input.GetUserInput<string>();

            Console.Write(" Password: ");
            password = input.GetUserInput<string>();

            Customer customer = storeDAO.GetCustomerByUsername(username);

            if (customer.ID == 0)
            {
                HandleError(new InvalidCredentialsException("Login denied. Invalid credentials."));
                SignInScreen();
                return;
            }

            if (password != customer.Password)
            {
                storeDAO.AddLogRecord(
                    new LogRecord(DateTime.Now, $"A Login to the user: {username}", "No", "Invalid password"));

                HandleError(new InvalidCredentialsException("Login denied. Invalid credentials."));
                SignInScreen();
                return;
            }

            if (mode == TestMode.On)
                return;

            storeDAO.AddLogRecord(
                new LogRecord(DateTime.Now, $"A Login to the user: {username}", "Yes", ""));

            currentCustomer = customer;

            RegisteredCustomerMenuForm();
        }

        /// <summary>
        /// This menu contains all the views and actions of the registered customer.
        /// </summary>
        public void RegisteredCustomerMenuForm()
        {
            Console.Clear();
            Console.WriteLine($"\n Welcome {currentCustomer.FirstName}! What would you like to do? \n");
            Console.WriteLine(" 1. See my previous orders");
            Console.WriteLine(" 2. See all products");
            Console.WriteLine(" 3. Make a new order");
            Console.WriteLine(" 4. < Go back to the main menu \n");

            int choice;

            while (true)
            {
                choice = input.GetUserInput<int>();

                switch (choice)
                {
                    case 1:
                        PrintCustomerOrders();
                        return;
                    case 2:
                        ShowAllProducts();
                        return;
                    case 3:
                        CreateNewOrder();
                        return;
                    case 4:
                        MainForm mainForm = new MainForm(mode);
                        mainForm.StartScreen();
                        return;
                    default:
                        Console.Write("Invalid choice. please select one of the choices (1/2/3/4): ");
                        break;
                }
            }
        }

        /// <summary>
        /// Prints all the previous orders of the customer.
        /// </summary>
        public void PrintCustomerOrders()
        {
            Console.Clear();
            Console.WriteLine("\n Here are your orders: \n *********");
            List<Order> orders = storeDAO.GetCustomerOrders(currentCustomer.ID);

            foreach(Order order in orders)
            {
                Console.Write($" Order ID: '{order.ID}' ");
                Console.Write($"Product Name: '{order.ProductName}' ");
                Console.Write($"Amount Ordered: '{order.Amount}' ");
                Console.Write($"Order Total: '{order.TotalPrice} $' ");
                Console.WriteLine(" ");
            }

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders to show.");
            }

            Console.WriteLine(" *********\n Press Enter to procced.");
            Console.ReadKey();
            RegisteredCustomerMenuForm();
        }

        /// <summary>
        /// Shows all the products from all suppliers.
        /// </summary>
        public void ShowAllProducts()
        {
            Console.Clear();
            Console.WriteLine("\n Here are all our fine products: \n *********");
            PrintAllProducts();
            Console.WriteLine(" *********\n Press Enter to procced.");
            Console.ReadKey();

            RegisteredCustomerMenuForm();
        }

        /// <summary>
        /// In this screen the customer can order a product.
        /// </summary>
        public void CreateNewOrder()
        {
            Console.Clear();
            Console.WriteLine("\n What would you like to order? \n *********");
            PrintAllProducts();
            Console.Write(" *********\n Please insert the ID of the selected product: ");

            Order order = new Order();
            order.CustomerID = currentCustomer.ID;
            order.ProductID = input.GetUserInput<int>();

            Console.Write(" *********\n Please insert the amount you would like to order: ");
            order.Amount = input.GetUserInput<int>();

            Product product = storeDAO.GetProductByID(order.ProductID);

            if (product.ID == 0)
            {
                HandleError(new ProductDoesntExistException("Product doesn't exist"));
                return;
            }

            if (order.Amount <= 0)
            {
                HandleError(new OrderAmountCannotBeZeroOrNegativeNumberException("Order amount cannot be zero or negative number"));
                return;
            }

            if (product.Quantity == 0)
            {
                HandleError(new ProductOutOfStockException("Product is out of stock"));
                return;
            }

            if (product.Quantity < order.Amount)
            {
                HandleError(new NotEnoughItemsInTheStockException("Not enough items in the stock to buy"));
                return;         
            }

            order.TotalPrice = order.Amount * product.Price;

            storeDAO.UpdateProductQuantity(product.ID, product.Quantity - order.Amount);
            storeDAO.CreateNewOrder(order);

            if (mode == TestMode.On)
                return;

            Console.WriteLine("\n Your order was placed successfully! press Enter to continue.");

            Console.ReadKey();

            storeDAO.AddLogRecord(
                new LogRecord(DateTime.Now, $"The user {currentCustomer.UserName} has purchased {order.Amount} units of a {product.Name}", "Yes", ""));

            RegisteredCustomerMenuForm();
        }

        public void PrintAllProducts()
        {
            List<Product> products = storeDAO.GetAllProducts();

            foreach (Product product in products)
            {
                Console.Write($" Product ID: '{product.ID}' ");
                Console.Write($"Product Name: '{product.Name}' ");
                Console.Write($"Company: '{product.SupplierName}' ");
                Console.Write($"Price: '{product.Price} $' ");
                Console.Write($"Quantity Available: '{product.Quantity}' ");
                Console.WriteLine(" ");
            }

            if (products.Count == 0)
            {
                Console.WriteLine("No products to show.");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// If the program runs on test mode, an exception will be thrown for each case.
        /// If not, the user will get a message to the screen that explains what's wrong.
        /// </summary>
        public void HandleError(Exception ex)
        {
            if (mode == TestMode.Off)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            else
            {
                throw ex;
            }
        }
    }
}

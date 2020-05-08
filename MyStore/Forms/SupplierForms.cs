using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Forms
{
    /// <summary>
    /// This class contains all the menus and actions of the supplier.
    /// </summary>
    public class SupplierForms
    {
        StoreDAO storeDAO = new StoreDAO();
        public UserInput input;
        private Supplier currentSupplier;
        public TestMode mode;

        public SupplierForms(TestMode mode)
        {
            this.mode = mode;
            input = new UserInput(mode);
        }

        public SupplierForms(TestMode mode, Supplier supplier)
        {
            this.mode = mode;
            input = new UserInput(mode);
            currentSupplier = supplier;
        }

        /// <summary>
        /// This is the initial menu where the supplier can sign up a new account or log in.
        /// </summary>
        public void SignInScreen()
        {
            Console.Clear();
            Console.WriteLine("\n Please select: \n");
            Console.WriteLine(" 1. Existing Supplier");
            Console.WriteLine(" 2. New Supplier");
            Console.WriteLine(" 3. < Go back to the main menu\n");

            int choice;

            while (true)
            {
                choice = input.GetUserInput<int>();

                switch (choice)
                {
                    case 1:
                        SupplierLogInForm();
                        return;
                    case 2:
                        NewSupplierForm();
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
        public void NewSupplierForm()
        {
            Console.Clear();
            Supplier supplier = new Supplier();

            Console.WriteLine("\n Please insert the following details: \n");

            Console.Write(" 1. User Name: ");
            supplier.UserName = input.GetUserInput<string>();

            Console.Write(" 2. Password: ");
            supplier.Password = input.GetUserInput<string>();

            Console.Write(" 3. Company Name: ");
            supplier.Company = input.GetUserInput<string>();

            // The program will check if the selected username is taken or not.
            if (storeDAO.GetSupplierByUsername(supplier.UserName).ID != 0)
            {
                storeDAO.AddLogRecord(
                    new LogRecord(DateTime.Now, $"A user attempted to create new supplier account", "No", $"The username: {supplier.UserName} Already exists"));

                HandleError(new UserNameAlreadyExistException("User name already exist"));
                SignInScreen();
                return;
            }

            if (mode == TestMode.On)
                return;

            storeDAO.CreateNewSupplier(supplier);
            Console.WriteLine("\n Your account created successfully! Press Enter to procced.");
            Console.ReadKey();

            storeDAO.AddLogRecord(
                new LogRecord(DateTime.Now, $"A new supplier account was created by the user: {supplier.UserName}", "Yes", ""));

            MainForm mainForm = new MainForm(mode);
            mainForm.StartScreen();
        }

        /// <summary>
        /// This is the log in screen.
        /// </summary>
        public void SupplierLogInForm()
        {
            Console.Clear();
            Console.WriteLine("\n Please insert your login credentials: ");

            string username;
            string password;

            Console.Write(" User name: ");
            username = input.GetUserInput<string>();

            Console.Write(" Password: ");
            password = input.GetUserInput<string>();

            Supplier supplier = storeDAO.GetSupplierByUsername(username);

            if (supplier.ID == 0)
            {
                HandleError(new InvalidCredentialsException("Login denied. Invalid credentials."));
                SignInScreen();
                return;
            }

            if (password != supplier.Password)
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

            currentSupplier = supplier;

            RegisteredSupplierMenuForm();
        }

        /// <summary>
        /// This menu contains all the views and actions of the registered supplier.
        /// </summary>
        public void RegisteredSupplierMenuForm()
        {
            Console.Clear();
            Console.WriteLine($"\n Welcome {currentSupplier.UserName}! What would you like to do? \n");
            Console.WriteLine(" 1. See all my products");
            Console.WriteLine(" 2. Add a new product");
            Console.WriteLine(" 3. < Go back to the main menu \n");

            int choice;

            while (true)
            {
                choice = input.GetUserInput<int>();

                switch (choice)
                {
                    case 1:
                        PrintSupplierProduct();
                        return;
                    case 2:
                        AddNewProduct();
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
        /// Shows all supplier's products in the store.
        /// </summary>
        public void PrintSupplierProduct()
        {
            Console.Clear();
            Console.WriteLine("\n Here are your products: \n *********");
            List<Product> products = storeDAO.GetAllSupplierProducts(currentSupplier.ID);

            foreach(Product product in products)
            {
                Console.Write($" Product ID: '{product.ID}' ");
                Console.Write($"Product Name: '{product.Name}' ");
                Console.Write($"Price: '{product.Price} $' ");
                Console.Write($"Quantity Available: '{product.Quantity}' ");
                Console.WriteLine(" ");
            }

            if (products.Count == 0)
            {
                Console.WriteLine("No products to show.");
            }

            Console.WriteLine(" *********\n Press Enter to procced.");
            Console.ReadKey();
            RegisteredSupplierMenuForm();
            return;
        }

        /// <summary>
        /// In this form the supllier can add a new product.
        /// </summary>
        public void AddNewProduct()
        {
            Console.Clear();

            Console.Write("\n Please insert the product's name: ");

            string productName = input.GetUserInput<string>();

            Product product = storeDAO.GetProductByName(productName);

            // if the product name exists in the store, but belongs to another supplier.
            if (product.ID != 0 && product.SupplierID != currentSupplier.ID) // 
            {
                Console.WriteLine("\n This product is already being offered by another supplier.");
                Console.WriteLine(" Press Enter to continue");
                Console.ReadKey();
                RegisteredSupplierMenuForm();
                return;
            }

            // if the supplier already have a product with that name.
            if (product.ID != 0 && product.SupplierID == currentSupplier.ID)
            {
                Console.WriteLine("\n This products already being offered by you.");
                Console.Write(" Would you like to add to the stock? (y/n) ");
                string choice = input.GetUserInput<string>();

                if (choice != "y")
                {
                    RegisteredSupplierMenuForm();
                    return;
                }

                Console.Write("\n How many items Would you like to add to the stock? ");
                int quantity = input.GetUserInput<int>();

                if ((product.Quantity + quantity) < 0)
                {
                    HandleError(new ProductQuantityCannotBeNegativeNumberException("Product quantity cannot be a negative number"));
                    RegisteredSupplierMenuForm();
                    return;
                }

                if (mode == TestMode.On)
                    return;

                storeDAO.UpdateProductQuantity(product.ID, product.Quantity + quantity);

                storeDAO.AddLogRecord(
                    new LogRecord(DateTime.Now, $"The supplier {currentSupplier.UserName} updated the quantity of the product '{product.Name}' to {product.Quantity + quantity}", "Yes", ""));

                Console.WriteLine("\n Your items has been added to the stock successfully! Press Enter to continue");
                Console.ReadKey();
                RegisteredSupplierMenuForm();
            }
            
            product.Name = productName;
            product.SupplierID = currentSupplier.ID;

            Console.Write("\n Please insert the price of your product: ");
            product.Price = input.GetUserInput<decimal>();

            if (product.Price < 0)
            {
                HandleError(new ProductPriceCannotBeNegativeException("Product's price cannot be negative"));
                RegisteredSupplierMenuForm();
                return;
            }

            Console.Write("\n How many items Would you like to add to the stock? ");
            product.Quantity = input.GetUserInput<int>();

            if ((product.Quantity) < 0)
            {
                HandleError(new ProductQuantityCannotBeNegativeNumberException("Product quantity cannot be a negative number"));
                RegisteredSupplierMenuForm();
                return;
            }

            storeDAO.CreateNewProduct(product);

            if (mode == TestMode.On)
                return;

            Console.WriteLine("\n Your product has been added successfully! Press Enter to continue");
            Console.ReadKey();

            storeDAO.AddLogRecord(
                new LogRecord(DateTime.Now, $"The supplier {currentSupplier.UserName} added a new product '{product.Name}'", "Yes", ""));

            RegisteredSupplierMenuForm();
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

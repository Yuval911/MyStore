using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Forms
{
    /// <summary>
    /// This class contains the main "form" - the main menu of the program.
    /// Here the user can identify as a customer, supplier or administrator.
    /// </summary>
    public class MainForm
    {
        StoreDAO storeDAO = new StoreDAO();
        public UserInput input;
        public TestMode mode;

        public MainForm(TestMode mode)
        {
            this.mode = mode;
            input = new UserInput(mode);
        }

        /// <summary>
        /// The main menu.
        /// </summary>
        public void StartScreen()
        {
            Console.Clear();
            Console.WriteLine(" \n Welcome to MyStore! \n\n To continue, please idenify yourself:\n");
            Console.WriteLine(" 1. I'm a customer");
            Console.WriteLine(" 2. I'm a supplier");
            Console.WriteLine(" 3. I'm an administrator");
            Console.WriteLine(" 4. Exit application \n");

            int choice;

            while (true)
            {
                choice = input.GetUserInput<int>();

                switch (choice)
                {
                    case 1:
                        CustomerForms customerForms = new CustomerForms(TestMode.Off);
                        customerForms.SignInScreen();
                        return;
                    case 2:
                        SupplierForms supplierForms = new SupplierForms(TestMode.Off);
                        supplierForms.SignInScreen();
                        return;
                    case 3:
                        AdministratorScreen();
                        return;
                    case 4:
                        return;
                    default:
                        Console.Write("Invalid choice. please select one of the choices (1/2/3): ");
                        break;
                }
            }
        }

        /// <summary>
        /// The administrator area.
        /// </summary>
        public void AdministratorScreen()
        {
            Console.Clear();
            Console.Write(" \n Please enter the password (hint - 1234): ");

            string pass = input.GetUserInput<string>();

            if (pass != "1234")
            {
                throw new InvalidCredentialsException("Invalid password");
            }

            if (mode == TestMode.On)
                return;

            Console.Clear();
            Console.WriteLine(" \n Here are the last 10 records from the log: \n");

            List<LogRecord> records = storeDAO.GetAllLogRecords();

            foreach(LogRecord record in records)
            {
                Console.Write($" (*) Record ID: '{record.ID}' ");
                Console.Write($"Date Time: '{record.Date}' ");
                Console.Write($"Action: ' {record.Action} ' ");
                Console.Write($"Succeeded: '{record.Succeeded}' ");
                Console.Write($"Fail Cause: '{record.FailCause}' \n");
                Console.WriteLine(" ");
            }

            Console.WriteLine("\n Press Enter to procced.");
            Console.ReadKey();
            StartScreen();
        }
    }
}

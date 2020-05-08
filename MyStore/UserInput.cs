using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore
{
    /// <summary>
    /// This class was created for the tests.
    /// The method GetUserInput returns the regular Console.ReadLine function when not running in a test.
    /// When it DOES run on a test, it will allow the test to inject any required input.
    /// </summary>

    public class UserInput
    {
        // This list contains all the input that the test method wishes to inject.
        // All the injections in the list must be at the same order it appears in the program.
        public List<object> Injections { get; set; } = new List<object>();
        public TestMode mode;

        public UserInput(TestMode mode)
        {
            this.mode = mode;
        }

        public T GetUserInput<T>()
        {
            // If in test mode, it will return the injected input from the list.
            if (mode == TestMode.On)
            {
                object injection = Injections[0];
                Injections.Remove(injection);
                return (T)Convert.ChangeType(injection, typeof(T));
            }

            // Else, it will return a Console.ReadLine function.
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            if (typeof(T) == typeof(int))
            {
                return (T)Convert.ChangeType(Convert.ToInt32(Console.ReadLine()), typeof(T));
            }
            if (typeof(T) == typeof(decimal))
            {
                return (T)Convert.ChangeType(Convert.ToDecimal(Console.ReadLine()), typeof(T));
            }

            return (T)Convert.ChangeType(null, typeof(T));
        }        
    }
}

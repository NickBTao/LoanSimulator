using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LoanSimulatorV2
{
    class Utils
    {
        /// <summary>
        /// Found this function on the Interwebs, quite elgant and useful
        /// Not sure about ConsoleKey and readKey... but it works.
        /// Promts user for 'y' or 'n' and loops until it gets it one or the other
        /// Made to be used inside an IF or a While Condition, The user's choice then deciding what happens next
        /// </summary>
        /// <param name="msg">A string message prompting the user with a yes or no question
        /// Example: Do you with to update X value?
        /// </param>
        /// <returns>true/false</returns>
        public static bool Confirm(string msg)
        {
            ConsoleKey Entry;
            do
            {
                Console.Write("{0} [y/n]", msg);
                Entry = Console.ReadKey(false).Key;
                if (Entry != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (Entry != ConsoleKey.Y && Entry != ConsoleKey.N);

            return (Entry == ConsoleKey.Y);

        }


        /* Roughly the same logic as the function above wth a few more elements
         * The Method name is somewhat self explanatory:
         * The function loops until the user enters a string which is equal to one of the elements in the array
         * Takes a user promt, msg as a parameter asking the user for some data.
         * Takes as second string as a parameter, error_msg in case the user doesn't enter the correct data
         * The last parameter is a string array we are testing*/
        public static string ValidateStringFoundInArray(string msg, string error_msg, string[] Array)
        {
            string Entry;
            Console.WriteLine(msg);
            do
            {
                Entry = Convert.ToString(Console.ReadLine());
                // The if condition here, same as the while condition,
                // is here to explain to the user why the data entered is incorrect
                // and why their mother is of ill repute...
                if (!Array.Any(Entry.Equals))
                {
                    Console.WriteLine(error_msg);
                    Console.WriteLine(msg);
                }
            } while (!Array.Any(Entry.Equals));
            return Entry;
        }



        //Similar to above validate Methods.
        //Testing only that the user entry is not empty
        public static string ValidateNotEmpty(string msg, string error_msg)
        {
            string Entry;
            Console.WriteLine(msg);
            do
            {
                Entry = Convert.ToString(Console.ReadLine());

                if (Entry == string.Empty)
                {
                    Console.WriteLine(error_msg);
                    Console.WriteLine(msg);
                }
            } while (Entry == string.Empty);
            return Entry;
        }


        /* Again, roughly the same logic as the functions above wth a few more elements
         * 3 basic things we are testing:
         * 1. if user types 'quit' the value returned is used to break the user 
         *          out of the parent procedure and back to the main menu
         * 2. Checks to see if the value is the correct datatype. In this case, a double.
         * 3. Checks that the double is between a Min and Max Value
         * The Loop continues until the user quits or passes test 2 or 3.
         * Parameters: user prompt, error message and the min, max values we are testing
*/
        public static double ValidateDoubleBetween(string msg, string error_msg, double min, double max)
        {
            string Entry;
            double EntryDouble;
            Console.WriteLine(msg);
            do
            {
                Entry = Convert.ToString(Console.ReadLine());
                // In the case that the user types 'quit' or 'q':
                // The value returned is lower than the asked for minimum minimum value
                // See the Procedure in which this method is called:
                // if val < min --> Break --> Retrun to main menu.
                if (Entry.Equals("quit") || Entry.Equals("q"))
                {
                    EntryDouble = min - 1;
                    return EntryDouble;
                }
                //Here we're trying to see if we can convert the users entry into a double...
                // try {} catch (FormatException) {} --> 
                // ---> necessary or the cumputor will explode 
                try
                {

                    EntryDouble = Convert.ToDouble(Entry);
                    // The if condition here, same as the while condition,
                    // is here to explain to the user Why the data entered is incorrect
                    // and why their mother is of ill repute...
                    if (EntryDouble < min || EntryDouble > max)
                    {
                        Console.WriteLine(error_msg);
                        Console.WriteLine(msg);
                    }
                }
                //Catch: In the case the data type is wrong e.i. ask for a number and user types : "Bob and his pet tiger Lilly"...
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Data Type...");
                    Console.WriteLine(error_msg);
                    Console.WriteLine(msg);
                    //The EntryDouble var needs a value or the While condition bellow will display an error
                    // EntryDouble = min - 1 --> same as above but with diffent very outcome.
                    // the variable has a value, but is garanted to fail the while condition and restart the loop.
                    // which is exactly what we want with invalid data type
                    EntryDouble = min - 1;
                }
            } while (EntryDouble < min || EntryDouble > max);
            return EntryDouble;
        }


        /* In allmost identical to the above function
         * BUT NOW: New and Improved, with Integers, instead of pesky doubles...*/
        public static int ValidateIntBetween(string msg, string error_msg, int min, int max)
        {
            string Entry;
            int EntryInt;
            Console.WriteLine(msg);
            do
            {
                Entry = Convert.ToString(Console.ReadLine());
                if (Entry.Equals("quit") || Entry.Equals("q"))
                {
                    EntryInt = min - 1;
                    return EntryInt;
                }

                try
                {
                    EntryInt = int.Parse(Entry);
                    if (EntryInt < min || EntryInt > max)
                    {
                        Console.WriteLine(error_msg);
                        Console.WriteLine(msg);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Data Type...");
                    Console.WriteLine(error_msg);
                    Console.WriteLine(msg);
                    EntryInt = min - 1;
                }
            } while (EntryInt < min || EntryInt > max);
            return EntryInt;
        }


        /// <summary>
        /// Validate that user entered value contains no SpecialCharacters (except underscore)
        /// Found Regex stuff online... 
        /// Don't understand it well, but this particular
        /// formulation checks to that string contains only numbers, characters and underscore
        /// </summary>
        /// <param name="msg">message promting user for a string</param>
        /// <param name="error_msg"> Message in case the string contains unwanted special characters</param>
        /// <returns> either a quit value that ends the parrent method, or a valid string without special characters</returns>
        public static string ValidateStringNoSpecialCharacters(string msg, string error_msg)
        {
            string Entry;
            Console.WriteLine(msg);
            var regexItem = new Regex(@"^[a-zA-Z0-9_]*$");
            do
            {
                Entry = Convert.ToString(Console.ReadLine());


                if (Entry.Equals("quit") || Entry.Equals("q"))
                {
                    Entry = "quit/";
                    return Entry;
                }
                if (!regexItem.IsMatch(Entry))
                {
                    Console.WriteLine(error_msg);
                    Console.WriteLine(msg);
                }
            } while (!regexItem.IsMatch(Entry));
            return Entry;
        }
    }
}

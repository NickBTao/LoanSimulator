using System;
using System.Threading;

namespace LoanSimulatorV2
{
    class Program
    {
        static void Main(string[] args)
        {
            //--BASE VALUES------------------------------------------------------------------------------------------------

            //List of Loans
            LoanRegister loan_register = new LoanRegister();
            /* Class contains the meat of the Pograms functionality
             * Smaal functions: CountLoans, FindIndex, CalculateMonthlyPayment, CalculateDebtOwed, DisplayLoanDeatails
             * and CRUD : ViewLoans, NewLoans, UpdateLoans, DeleteLoans
             * Others : UpdateInterest, log (export view data to txt file)
             */

            int[] Dur = new int[]{400, 900, 1400};


            //User name is written into the exported log file.
            string UserName;

            // Payment Periods per year : e.i  12 months.
            int PperYear = 12;

            //Minimum and maximum values of Loan Principals and Years till repayment and Interest
            double MinPrincipal = 100, MaxPrincipal = 1000000;
            int MinYears = 1, MaxYears = 50;
            //The interest value gets devided by 100 after the Min Max test. See Utils.UpdateInterest.
            double MinInterest = 0.1, MaxInterest = 50;

            /* Unlike what was requested in assignment, Interest is not recorded and applied to loans invidually,
             * Instead, one single global value affects All loans.
             * This is in theory closer to an Ajustable Rate morgage, rather tan a fixed morgage.
             * This allows user to enter several loans in the register, 
             * alter the global interest rate and observe the change to monhtly payments and total debt.*/
            double GlobalInterest = 0.05;

            //List of commands that users can type
            string[] Commands = new string[] { "help", "view", "new", "update", "delete", "interest", "log", "quit" };
            //String version of that list to display
            string CommandsDisplay = "Commands: " + string.Join(" - ", Commands);
            // The specific command that the user has typed that is used to initiate a method
            string UserCommand;



            //--MAIN PROGRAM SEQUENCE---------------------------------------------------------------------------------------
            /* Simple enough: 
             * - the program runs on a do while loop, asking user to type in a command from the list
             * - each command sends the user to a method defined either in class LoanRegister or Later here in Programs
             * - ends when the user types in the 'quit' command from the main menu */

            Message.Intro(Dur);
            Console.WriteLine("");
            //Promps user for a name, validates that string is not empty.
            //See Utils.ValidateNotEmpty
            UserName = Utils.ValidateNotEmpty("Please Enter Your Name : ", "Error! Cannot be empty!");
            
            do
            {
                Thread.Sleep(Dur[0]);
                Console.WriteLine("");
                Console.WriteLine(Message.MainMenuTitle());
                Console.WriteLine("");
                Console.WriteLine("Currently {0} loan(s) in the Register", loan_register.CountLoans());
                Console.WriteLine("Global Interest Rate: {0}%", (GlobalInterest * 100));
                Console.WriteLine("");
                
                //Promps user for a command, validates that user entry is found in the command array.
                //See Utils.ValidateStringFoundInArray
                Console.WriteLine(CommandsDisplay);
                UserCommand = Utils.ValidateStringFoundInArray("Enter a Command :", "Incorrect Input!", Commands);
                Thread.Sleep(Dur[0]);
                //Used switch case instead of Multiple IFs
                switch (UserCommand)
                {
                    case "help":
                        Message.Help(Dur);
                        break;
                    case "view":
                        loan_register.ViewLoans(false, Dur, UserCommand, GlobalInterest, PperYear);
                        break;
                    case "new":
                        loan_register.NewLoans(Dur,
                            GlobalInterest, PperYear, MinPrincipal, MaxPrincipal, MinYears, MaxYears);
                        break;
                    case "update":
                        loan_register.UpdateLoans(Dur,
                            UserCommand, GlobalInterest, PperYear, MinPrincipal, MaxPrincipal, MinYears, MaxYears);
                        break;
                    case "delete":
                        loan_register.DeleteLoans(Dur, UserCommand, GlobalInterest, PperYear);
                        break;
                    case "interest":
                        GlobalInterest = loan_register.UpdateInterest(Dur, GlobalInterest, PperYear, MinInterest, MaxInterest);
                        break;
                    case "log":
                        loan_register.Log(Dur, UserCommand, UserName, loan_register, GlobalInterest, PperYear);
                        break;
                }
                //Program loops forever until quit command
            } while (UserCommand != "quit");
            //The End
            Message.Outro(Dur);
        }
    }
}

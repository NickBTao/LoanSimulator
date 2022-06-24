using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoanSimulatorV2
{
    class Message
    {
        //-MESSAGES--------------------------------------------------------------------------------------------------

        /* INTRO : Greeting message
         * OUTRO : Goodby message after quit
         * HELP MESSAGE : Help message with command list and some notes
         * NO LOANS MESSAGE: When user types ina command and there aren't any Loans in the register yet.
         * TITLES: All Program Module Titles
         * None of these had their place in LoanRegister, Program, Utils so separate Messages class was created
         * Removing them, made the above a lot leaner
        */

        public static void Intro(int[] Dur)
        {
            Console.WriteLine(PageDivider());
            Console.WriteLine(MainTitle());
            Console.WriteLine(PageDivider());
            Console.WriteLine("");
            Console.WriteLine("                       Welcome to Loan Calculator!");
            Thread.Sleep(Dur[1]);
        }

        public static void Outro(int[] Dur)
        {
            Console.WriteLine("");
            Console.WriteLine(PageDivider());
            Console.WriteLine(GoodbyeTitle());
            Console.WriteLine(PageDivider());
            Console.WriteLine("");
            Console.WriteLine("                Thank you very much and have a nice day.");
            Console.WriteLine("");

            ///The following lines are needed in the Published version of the application
            ///Otherwise the application quits instantly
            //Console.WriteLine("Press any key...");
            //ConsoleKey Entry;
            //Entry = Console.ReadKey(false).Key;
            Thread.Sleep(Dur[2]);
            Thread.Sleep(Dur[2]);
            Thread.Sleep(Dur[2]);
        }

        public static void Help(int[] Dur)
        {
            Console.WriteLine("");
            Console.WriteLine(HelpTitle());

            Console.WriteLine("");
            Console.WriteLine("Register one or several loans.");
            Console.WriteLine("The program will calculate your monthly payments and total debt.");
            Console.WriteLine("");
            Console.WriteLine("List of text Commands :");
            Console.WriteLine("help: You're reading this now...");
            Console.WriteLine("view: view a list of the loans in the register");
            Console.WriteLine("new: add one or more loans into the register");
            Console.WriteLine("update: edit a loan in the register");
            Console.WriteLine("delete: delete a loan in the register");
            Console.WriteLine("interest: edit the Global Interest rate");
            Console.WriteLine("log: outputs a .txt file with the current view data");
            Console.WriteLine("quit: exit the program");
            Console.WriteLine("");
            Console.WriteLine("NOTA BENE:");
            Console.WriteLine("- Type 'quit' anytime entering data entry to return to the main menu.");
            Console.WriteLine("- Interest is a single global value: it will affect All loans in the Register. ");
            Console.WriteLine("- Log Files are saved to folder: ../LoanSimulatorV2 --> /logs");
            Console.WriteLine("");
            do
            {
                Thread.Sleep(Dur[0]);
            } while (!Utils.Confirm("Do you wish to continue?"));
        }

        //Many of the commands are meaningless unless the user first inputs loans in the register.
        //Since this applies to many situations, it deserved it's own message.
        public static void NoLoans(string AbortedCommand)
        {
            Console.WriteLine("");
            Console.WriteLine("No loans in the Register... nothing to do...");
            Console.WriteLine("Type 'new' command to enter one or more loans.");
            Console.WriteLine("Then try '{0}' afterwards.", AbortedCommand);
            Console.WriteLine("");
            Console.WriteLine("Returning to main menu...");
        }


        //-TITLES--------------------------------------------------------------------------------------------------
        //Having these all in the same place makes standardising the characters a lot easier.
        public static string PageDivider()
        {
            return "========================================================================";
        }
        public static string MainTitle()
        {
            return "----------------------------- LOAN CALCULATOR --------------------------";
        }
        public static string GoodbyeTitle()
        {
            return "-------------------------------- GOODBYE -------------------------------";
        }
        public static string MainMenuTitle()
        {
            return "=============================== MAIN MENU ==============================";
        }
        public static string HelpTitle()
        {
            return "=============================== HELP/INFO ==============================";
        }
        public static string ViewTitle()
        {
            return "============================== VIEW LOANS ==============================";
        }
        public static string NewTitle()
        {
            return "=============================== NEW LOAN ===============================";
        }
        public static string UpdateTitle()
        {
            return "============================= UPDATE LOANS =============================";
        }
        public static string DeleteTitle()
        {
            return "============================== DELETE LOANS ============================";
        }
        public static string InterestTitle()
        {
            return "============================ UPDATE INTEREST ===========================";
        }
        public static string LogTitle()
        {
            return "============================= LOG VIEW DATA ============================";
        }
        public static string FileInfoTitle()
        {
            return "=============================== FILE INFO ==============================";
        }
    }
}

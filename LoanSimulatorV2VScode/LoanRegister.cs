using System;
using System.IO; // Needed for Log Method, exporting data to text file
using System.Collections.Generic;
using System.Linq;
using System.Threading;
//using System.Text;
//using System.Threading.Tasks;

namespace LoanSimulatorV2
{
    class LoanRegister
    {
        //--BASIC---------------------------------------------------------------------------------------
        public List<Loan> loan_register { get; set; }

        public LoanRegister()
        {
            this.loan_register = new List<Loan>();
        }


        //--SHORT FUNCTIONS---------------------------------------------------------------------------------------

        //NOTE: MATH.ROUND() is used (overused) throughout because of a persistant display error.


        //Count loans in the register
        internal int CountLoans()
        {
            return loan_register.Count();
        }

        /*In order to indentify specific loans in the register, list index numbers are used.
         * Specifically in order to manipulate existing loans (update, delete)
         * Another option would have been to add an ID attribute in the Loan class,
         * which would be populated automatically from an auto incrementing sequence.
         * Not sure if C# provides another or a simpler unique identifyer.*/
        private int FindIndex(Loan loan)
        {
            return loan_register.IndexOf(loan);
        }

        /*Calculates Montly payments based on the Global Interst, periods per Year,
         *And the specific loan's Principal and Years to repayment.
         *The original formula provided didn't work...
         *Found this instead: https://www.matrixlab-examples.com/calculate-loan-payment.html */
        public double CalculateMonthlyPayment(double Interest, double Principal, int Years, int PperYear)
        {
            return Math.Round(Interest * Principal / PperYear /
                (1 - Math.Pow((Interest / PperYear + 1), (-PperYear * Years))), 2);
        }

        /* Total Amount per loan that is scheduled be repaid.
         * I assume this could be calculated differently...
         *  but was too lazy to look it up*/
        /*Note: When interest and years to repayment are high, 
         * the total interest paid can be much higher thant the original principle
         * Einstein: "Compound interest is the eighth wonder of the world. 
         * He who understands it, earns it; he who doesn't, pays it."
         */
        public double CalculateDebtOwed(double MonthlyPayment, int Years, int PperYear)
        {
            return Math.Round(MonthlyPayment * Years * PperYear, 2);
        }

        //Single string for display purposes containing Loan attributes and calculations
        public string DisplayLoanDeatails(int Index, Loan loan, double MonthlyPayment, double DebtOwed)
        {
            return "ID: " + Index + " - " + loan + " - Month Pay: $" + MonthlyPayment + " - Debt Owed: $" + DebtOwed;
        }

        //--PROGRAM MAIN METHODS---------------------------------------------------------------------------------------

        //CRUD: READ METHOD
        public void ViewLoans(bool auto, int[] Dur, string ThisCommand, double Interest, int PperYear)
        {
            if (CountLoans() == 0)
            {
                //No sense going throught the below calculation if there are 0 loans in the register
                Message.NoLoans(ThisCommand);
                Thread.Sleep(Dur[2]);
            }
            else
            {

                Console.WriteLine("");
                Console.WriteLine(Message.ViewTitle());
                Console.WriteLine("");
                Console.WriteLine("Currently {0} loan(s) in the Register", CountLoans());
                Console.WriteLine("");
                Console.WriteLine("@ {0}% Interest: ", (Interest * 100));
                Console.WriteLine("");
                
                //Initialise Agregate Values, equivalent of what you'd get from a GROUP BY Sql command
                double AvailableCapital = 0;
                double SumMonthlyPayments = 0;
                double SumDebtOwed = 0;
                int YearsTillDebtFree = 0;

                //Loop through all loans in the register
                foreach (Loan loan in loan_register)
                {
                    //The Calculated values for each loan.
                    int LoanIndex = FindIndex(loan);
                    double MonthlyPayment = CalculateMonthlyPayment(Interest, loan.Principal, loan.Years, PperYear);
                    double DebtOwed = CalculateDebtOwed(MonthlyPayment, loan.Years, PperYear);

                    //Add up the Agregate values for all loans
                    AvailableCapital += loan.Principal;
                    SumMonthlyPayments += Math.Round(MonthlyPayment,2);
                    SumDebtOwed += Math.Round(DebtOwed,2);
                    if (loan.Years > YearsTillDebtFree) { YearsTillDebtFree = loan.Years; }


                    //Display the details of each loan
                    Console.WriteLine(DisplayLoanDeatails(LoanIndex, loan, MonthlyPayment, DebtOwed));

                }

                //Display the agregate values, (but only if there is more than 1 loan...)
                if (CountLoans() > 1)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Availale Capital: ${0}", AvailableCapital);
                    Console.WriteLine("Sum of Montly Payments: ${0}", Math.Round(SumMonthlyPayments,2));
                    Console.WriteLine("Years until Completely Debt Free: {0}", YearsTillDebtFree);
                    Console.WriteLine("Total Debt Owed: ${0}", Math.Round(SumDebtOwed,2));
                }
                Console.WriteLine("");


                //VERY IMPORTANT
                // VIEW LOANS called as from the Log command (auto = true) Omits the user prompt
                // And runs automatically without stoping here.
                if (!auto)
                {
                    do
                    {
                        Thread.Sleep(Dur[0]);
                    } while (!Utils.Confirm("Do you wish to continue?"));
                }

            }
        }

        //CRUD: CREATE METHOD
        public void NewLoans(int[] Dur,
            double Interest, int PperYear, double MinPrincipal, double MaxPrincipal, int MinYears, int MaxYears)
        {
            do
            {
                Console.WriteLine("");
                Console.WriteLine(Message.NewTitle());
                Console.WriteLine("");

                //User is asked to enter to enter the principle of the loans and years to repayment
                //The 2 validate methods(see Utils class) ensure both valid data type and Min/Max of values
                double Principal = Utils.ValidateDoubleBetween("Enter the principle of the Loan : ",
                    "Invalid Entry! Enter a number between " + MinPrincipal + " and " + MaxPrincipal,
                    MinPrincipal, MaxPrincipal);
                //Additionaly, if the user enters 'quit' or 'q', they will return to the Main Menu
                if (Principal < MinPrincipal) { Console.WriteLine("Returning to main menu..."); break; }

                Thread.Sleep(Dur[0]);
                Console.WriteLine("");
                int Years = Utils.ValidateIntBetween("Enter the Years to Repayment : ",
                    "Invalid Entry! Enter a whole number between " + MinYears + " and " + MaxYears,
                    MinYears, MaxYears);
                if (Years < MinYears) { Console.WriteLine("Returning to main menu..."); break; }
                Thread.Sleep(Dur[0]);
                Loan loan = new Loan(Principal, Years);

                // Unlike view, updatade and delete methods, the Index value doesn't exist
                // Since the loan hasn't been added to the register yet
                // However, CountLoans method provides a temporary id for display purposes.
                int LoanIndex = CountLoans();

                double MonthlyPayment = CalculateMonthlyPayment(Interest, loan.Principal, loan.Years, PperYear);
                double DebtOwed = CalculateDebtOwed(MonthlyPayment, loan.Years, PperYear);

                // The user is shown the data to review and asked to Confirm that they want the loan added to the register
                // See Utils.Confirm method in Utils Class
                Console.WriteLine("");
                Console.WriteLine(DisplayLoanDeatails(LoanIndex, loan, MonthlyPayment, DebtOwed));
                Console.WriteLine("");
                Thread.Sleep(Dur[1]);
                if (Utils.Confirm("Do you wish to proceed (Enter this in the Register)?"))
                {
                    loan_register.Add(loan);
                    Console.WriteLine("");
                    Console.WriteLine("Loan Added...");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Loan Not Added to Registry...");
                }

                Console.WriteLine("");
                Thread.Sleep(Dur[1]);
                Console.WriteLine("There are {0} loans(s) in the Register", CountLoans());
                
                /*The entire method runs on a loop. user can opt to add as many loans they want
                 * Loop exits and returns to main program only when they do not wish to enter a new loan*/

            } while (Utils.Confirm("Do you wish to enter another loan in the Register?"));
        }


        //CRUD: UPDATE METHOD
        public void UpdateLoans(int[] Dur,
            string ThisCommand, double Interest, int PperYear, double MinPrincipal, double MaxPrincipal, int MinYears, int MaxYears)
        {
            if (CountLoans() == 0)
            {
                //No sense going throught the below calculation if there are 0 loans in the register
                Message.NoLoans(ThisCommand);
                Thread.Sleep(Dur[2]);
            }
            else
            {
                do
                {
                    Console.WriteLine("");
                    Console.WriteLine(Message.UpdateTitle());
                    Console.WriteLine("");
                    Console.WriteLine("Currently {0} loan(s) in the Register", CountLoans());
                    Console.WriteLine("");

                    //Loop through all loans in the register
                    foreach (Loan loan in loan_register)
                    {
                        //The Calculated values for each loan.
                        int LoanIndex = FindIndex(loan);
                        double MonthlyPayment = CalculateMonthlyPayment(Interest, loan.Principal, loan.Years, PperYear);
                        double DebtOwed = CalculateDebtOwed(MonthlyPayment, loan.Years, PperYear);

                        //Display the details of each loan
                        Console.WriteLine(DisplayLoanDeatails(LoanIndex, loan, MonthlyPayment, DebtOwed));
                    }


                    // User selects the index of the loan they wish to update
                    // Validate Method here ensures the user enters a valid id number
                    Console.WriteLine("");
                    int SelectedIndex = Utils.ValidateIntBetween(
                        "Type in the ID of the Loan you wish to update.", "That ID is not in the List!", 0, CountLoans() - 1);
                    if (SelectedIndex < 0) { Console.WriteLine("Returning to main menu..."); break; }
                    Console.WriteLine("");
                    Thread.Sleep(Dur[0]);
                    //The Current Loan Calculations
                    Loan old_loan = new Loan(loan_register[SelectedIndex].Principal, loan_register[SelectedIndex].Years);
                    double OldMonthlyPayment = CalculateMonthlyPayment(Interest, old_loan.Principal, old_loan.Years, PperYear);
                    double OldDebtOwed = CalculateDebtOwed(OldMonthlyPayment, old_loan.Years, PperYear);

                    //The New Loan attributes are stored
                    double NewPrincipal = Utils.ValidateDoubleBetween("Enter the principle of the Loan : ",
                        "Invalid Entry! Enter a number between " + MinPrincipal + " and " + MaxPrincipal,
                        MinPrincipal, MaxPrincipal);
                    if (NewPrincipal < MinPrincipal) { Console.WriteLine("Returning to main menu..."); break; }
                    Console.WriteLine("");
                    Thread.Sleep(Dur[0]);
                    int NewYears = Utils.ValidateIntBetween("Enter the Years to Repayment : ",
                        "Invalid Entry! Enter a whole number between " + MinYears + " and " + MaxYears,
                        MinYears, MaxYears);
                    if (NewYears < MinYears) { Console.WriteLine("Returning to main menu..."); break; }
                    
                    Loan new_loan = new Loan(NewPrincipal, NewYears);

                    //The New Loan Calculations
                    double NewMonthlyPayment = CalculateMonthlyPayment(Interest, new_loan.Principal, new_loan.Years, PperYear);
                    double NewDebtOwed = CalculateDebtOwed(NewMonthlyPayment, new_loan.Years, PperYear);

                    // The user is is shown and compares the old and new values...
                    Console.WriteLine("");
                    Thread.Sleep(Dur[0]);
                    Console.WriteLine("Current values in register: ");
                    Console.WriteLine(DisplayLoanDeatails(SelectedIndex, old_loan, OldMonthlyPayment, OldDebtOwed));
                    Console.WriteLine("Update to: ");
                    Console.WriteLine(DisplayLoanDeatails(SelectedIndex, new_loan, NewMonthlyPayment, NewDebtOwed));
                    Console.WriteLine("");
                    Thread.Sleep(Dur[2]);
                    // In case the values are identical.
                    if (old_loan.Equals(new_loan))
                    {
                        Console.WriteLine("Values are identical..What's the G%^&*med point!!!!");
                        Console.WriteLine("Loan Not Updated...");
                    }
                    else
                    {
                        //...and asked to confirm the update.
                        if (Utils.Confirm("Do you wish to proceed (Update loan values)?"))
                        {
                            loan_register[SelectedIndex].Principal = NewPrincipal;
                            loan_register[SelectedIndex].Years = NewYears;
                            Console.WriteLine("");
                            Console.WriteLine("Loan Updated...");
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Loan Not Updated...");
                        }
                    }
                    Thread.Sleep(Dur[1]);
                    //Like Create Method, Update runs on a loop until user does not want to update an loan.
                } while (Utils.Confirm("Do you wish to update another loan in the Register?"));

            }
        }
        //CRUD: DELETE METHOD
        public void DeleteLoans(int[] Dur, string ThisCommand, double Interest, int PperYear)
        {
            if (CountLoans() == 0)
            {
                //No sense going throught the below calculation if there are 0 loans in the register
                Message.NoLoans(ThisCommand);
                Thread.Sleep(Dur[2]);
            }
            else
            {

                do
                {
                    Console.WriteLine("");
                    Console.WriteLine(Message.DeleteTitle());
                    Console.WriteLine("");
                    Console.WriteLine("Currently {0} loan(s) in the Register", CountLoans());
                    Console.WriteLine("");

                    //Loop through all loans in the register
                    foreach (Loan loan in loan_register)
                    {
                        //The Calculated values for each loan.
                        int LoanIndex = FindIndex(loan);
                        double MonthlyPayment = CalculateMonthlyPayment(Interest, loan.Principal, loan.Years, PperYear);
                        double DebtOwed = CalculateDebtOwed(MonthlyPayment, loan.Years, PperYear);

                        //Display the details of each loan
                        Console.WriteLine(DisplayLoanDeatails(LoanIndex, loan, MonthlyPayment, DebtOwed));
                    }


                    // User selects the index of the loan they wish to delete
                    Console.WriteLine("");
                    int SelectedIndex = Utils.ValidateIntBetween(
                        "Type in the ID of the Loan you wish to delete.", "That ID is not in the List!", 0, CountLoans() - 1);
                    if (SelectedIndex < 0) { Console.WriteLine("Returning to main menu..."); break; }
                    Thread.Sleep(Dur[0]);
                    //The Current Loan attributes are stored
                    double OldMonthlyPayment = CalculateMonthlyPayment(
                        Interest, loan_register[SelectedIndex].Principal, loan_register[SelectedIndex].Years, PperYear);
                    double OldDebtOwed = CalculateDebtOwed(OldMonthlyPayment, loan_register[SelectedIndex].Years, PperYear);

                    // The user is asked to review the loan before confirming Delete.
                    Console.WriteLine("");
                    Console.WriteLine("This is loan you have selected: ");
                    Console.WriteLine(DisplayLoanDeatails(
                        SelectedIndex, loan_register[SelectedIndex], OldMonthlyPayment, OldDebtOwed));

                    Console.WriteLine("");
                    Thread.Sleep(Dur[2]);
                    if (Utils.Confirm("Do you wish to proceed (Delete Loan from Register)?"))
                    {
                        loan_register.RemoveAt(SelectedIndex);
                        Console.WriteLine("");
                        Console.WriteLine("Loan Deleted...");
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Loan Not Deleted...");
                    }

                    //Like Create Method, Delete runs on a loop until user does not want to delete an loan.
                    //The CountLoans > 0 condition is very important
                    //Otherwise the loop will continue even without any loans in the register!
                } while (CountLoans() > 0 && Utils.Confirm("Do you wish to delete another loan in the Register?"));

            }
        }


        //UPDATE INTEREST
        /// <summary>
        /// Used to prompt the user for a new interet rate then update it.
        /// ??? This is a function with a return value, but is this a situation where a
        /// Parameter passed as a REF would have made more sense???
        /// </summary>
        /// <returns>Either a New User defined Interest Rate, or the old InterestRate, unchanged.</returns>
        public double UpdateInterest(int[] Dur, double Interest, int PperYear, double Min, double Max)
        {
            Console.WriteLine("");
            Console.WriteLine(Message.InterestTitle());
            Console.WriteLine("");
            Console.WriteLine("Current Interest Rate : {0}%", Interest * 100);
            Console.WriteLine("");

            //User enters new interest rate, as a number...
            Console.WriteLine("--NOTE: enter '7' for 7% interest, NOT '0.07'");
            double NewInterest = Utils.ValidateDoubleBetween(
                "Enter a new Interest Rate :", "Error! Enter a number between " + Min + " and " + Max,
                Min, Max);
            if (NewInterest < Min) { Console.WriteLine("Returning to main menu..."); return Interest; }
            Thread.Sleep(Dur[0]);
            //... then converted to percentage
            NewInterest = NewInterest / 100;

            // In case values are identical
            if (Interest.Equals(NewInterest))
            {
                Console.WriteLine("Values are identical..What's the G%^&*med point!!!!");
                Console.WriteLine("Interest Not Updated...");
                return Interest;
            }
            else
            {
                if (CountLoans() == 0)
                {
                    // A simplified comparisson of new and old values if there are no loans in the register
                    Console.WriteLine("");
                    Console.WriteLine("Current Interest Rate : {0}%", Interest * 100);
                    Console.WriteLine("UPDATE TO:");
                    Console.WriteLine("New Interest Rate : {0}%", NewInterest * 100);
                    Console.WriteLine("");
                }
                else
                {
                    // Before Changing interest to new value:
                    // Calculate the SumMonthlyPayments & SumDebtOwed
                    // Both for Current Interest Rate and New Proposed Interest Rate
                    double SumMonthlyPayments = 0;
                    double SumDebtOwed = 0;
                    double NewSumMonthlyPayments = 0;
                    double NewSumDebtOwed = 0;

                    //Loop through all loans in the register
                    foreach (Loan loan in loan_register)
                    {
                        //The Calculated values for each loan Using both Current and New Interest Value
                        double MonthlyPayment = CalculateMonthlyPayment(Interest, loan.Principal, loan.Years, PperYear);
                        double DebtOwed = CalculateDebtOwed(MonthlyPayment, loan.Years, PperYear);
                        double NewMonthlyPayment = CalculateMonthlyPayment(NewInterest, loan.Principal, loan.Years, PperYear);
                        double NewDebtOwed = CalculateDebtOwed(NewMonthlyPayment, loan.Years, PperYear);

                        //Add up the Agregate values for all loans Using both Current and New Interest Value
                        SumMonthlyPayments += Math.Round(MonthlyPayment,2);
                        SumDebtOwed += Math.Round(DebtOwed,2);
                        NewSumMonthlyPayments += Math.Round(NewMonthlyPayment,2);
                        NewSumDebtOwed += Math.Round(NewDebtOwed,2);
                    }

                    //Current and new values compared
                    Console.WriteLine("");
                    Console.WriteLine("@ Current Interest Rate : {0}%", Interest * 100);
                    Console.WriteLine("Current Sum of Montly Payments: ${0}", Math.Round(SumMonthlyPayments,2));
                    Console.WriteLine("Current Total Debt Owed: ${0}", Math.Round(SumDebtOwed,2));

                    Console.WriteLine("");
                    Console.WriteLine("UPDATE TO");
                    Console.WriteLine("@ New Interest Rate : {0}%", NewInterest * 100);
                    Console.WriteLine("New Sum of Montly Payments: ${0}", Math.Round(NewSumMonthlyPayments,2));
                    Console.WriteLine("New Total Debt Owed: ${0}", Math.Round(NewSumDebtOwed,2));
                    Console.WriteLine("");
                }
                Thread.Sleep(Dur[2]);
                //User confirms the update
                if (Utils.Confirm("Do you wish to proceed (Update Interest Rate)?"))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Interest Updated...");
                    return NewInterest;
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Interest Not Updated...");
                    return Interest;
                }
            }
        }

        // https://stackoverflow.com/questions/4470700/how-to-save-console-writeline-output-to-text-file
        // I adapted the above code to export View Loan data into a txt file.
        public void Log(int[] Dur, string ThisCommand, string UserName, LoanRegister loan_register, double Interest, int PperYear)
        {

            if (CountLoans() == 0)
            {
                Message.NoLoans(ThisCommand);
                Thread.Sleep(Dur[2]);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(Message.LogTitle());
                Console.WriteLine("");


                Console.WriteLine("Attempting to Export View Loan data to txt file...");
                //Since the following string is used to create a file name, we don't want any special charaters.
                string FileName = Utils.ValidateStringNoSpecialCharacters(
                    "Enter a file name: ", "Error! Enter only letters, numbers and underscore.");
                //If user types 'q' or 'quit' in the preceding Validate function, 'quit/' is returned 
                if (FileName == "quit/")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Returning to main menu...");
                    Thread.Sleep(Dur[0]);
                }
                else
                {
                    //First date string used inside the file, the second in the file name
                    string FullLocalDateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                    string ShortlocalDate = DateTime.Now.ToString("dd-MM-yyyy"); ;

                    string FullFileName = FileName + "_" + ShortlocalDate + ".txt";

                    //Found Path.GetFullPath() here
                    //https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getfullpath?view=net-5.0
                    //string FullPath = Path.GetFullPath(FullFileName);//

                    //The Above Full Path.GetFullPath() point to /LoanSimulator -- > /bin --> /Debug folder by default
                    //Instead I want to save files in a seperate log folder.


                    //string DirPath = @"logs/"; ---> This doesn't always work, depenping on program file structure.
                    //string FullDir = Path.GetFullPath(DirPath);

                    //NOTE: DEPENDING ON THE TYPE OF CONSOLE APPLICATION USED THE FILE STRUCTURE IS DIFFERENT
                    //EXPERIMENTATION NEEDED WHEN SWITCHING FROM ONE TO ANOTHER.
                    //DID THIS TO MAKE SURE IT FOUND A LOG FOLDER SOMEWHERE...
                    string FullDir;
                    if (Directory.Exists(Path.GetFullPath(@"logs/")))
                    {
                        FullDir = Path.GetFullPath(@"logs/");
                        Console.WriteLine(FullDir);
                    }
                    else if (Directory.Exists(Path.GetFullPath(@"../logs/")))
                    {
                        FullDir = Path.GetFullPath(@"../logs/");
                        Console.WriteLine(FullDir);
                    } else if (Directory.Exists(Path.GetFullPath(@"../../logs/")))
                    {
                        FullDir = Path.GetFullPath(@"../../logs/");
                        Console.WriteLine(FullDir);
                    } else
                    {
                        FullDir = Path.GetFullPath(@"../../../logs/");
                        Console.WriteLine(FullDir);
                    }


                    string FullPathFileName = FullDir + FullFileName;

                    Console.WriteLine("");
                    Thread.Sleep(Dur[0]);
                    Console.WriteLine("Preparing to write file '{0}'...", FullFileName);
                    Thread.Sleep(Dur[1]);

                    //Wana check if file with identical name exists already (otherwise, existing file is Squished! by default.
                    //If the file doesn't exit OR (if file does exist and user confirms Replacement) Export is attempted
                    bool TryExport = false;
                    if (!File.Exists(FullPathFileName))
                    {
                        TryExport = true;
                    }
                    else
                    {

                        Console.WriteLine("");
                        Console.WriteLine("A file with the name '{0}' already exists!", FullFileName);
                        Thread.Sleep(Dur[1]);
                        if (Utils.Confirm("Do you wish to replace it?"))
                        {
                            TryExport = true;
                        }
                    }

                    if (!TryExport)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Returning to main menu...");
                        Thread.Sleep(Dur[0]);
                    }
                    else
                    {
                        FileStream ostrm;
                        StreamWriter writer;
                        TextWriter oldOut = Console.Out;

                        try
                        {
                            //Creates instance allowing file export.
                            //Not sure how this works exactly...
                            ostrm = new FileStream(FullPathFileName, FileMode.OpenOrCreate, FileAccess.Write);
                            writer = new StreamWriter(ostrm);
                        }
                        catch (Exception e)
                        {
                            //In case of error
                            Console.WriteLine("");
                            Console.WriteLine("Cannot create txt file...");
                            Console.WriteLine(e.Message);
                            return;
                        }

                        //Every Console.Write Commands after this point gets exported to the text file.
                        Console.SetOut(writer);

                        //Simple header to the file with basic meta info
                        Console.WriteLine("");
                        Console.WriteLine(Message.FileInfoTitle());
                        Console.WriteLine("");
                        Console.WriteLine("File Name : " + FullFileName);
                        Console.WriteLine("Created By : " + UserName);
                        Console.WriteLine("Created On : " + FullLocalDateTime);

                        //Body of the file, using the View Loans method.
                        //the true param is to indicate that ViewLoans runs Automatically
                        //without the user prompt at the end.
                        loan_register.ViewLoans(true, Dur, ThisCommand, Interest, PperYear);

                        //Ends the Writer instance
                        Console.SetOut(oldOut);
                        writer.Close();
                        ostrm.Close();

                        //Completion message, informs the user where to find the file
                        Thread.Sleep(Dur[1]);
                        Console.WriteLine("");

                        Console.WriteLine("Success!");
                        Console.WriteLine("File '{0}' Created!", FullFileName);
                        Console.WriteLine("Directory: {0}", FullDir);
                        Thread.Sleep(Dur[2]);
                    }
                }
            }
        }
    }
}

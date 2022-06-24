---------------------------------------------------------------------------------

LOAN SIMULATOR

A Simple Console Program in C# built with Visual Studio 2019

Created by: Nicolas Beattie
On: june 24, 2022

---------------------------------------------------------------------------------


DESCRIPTION

Register one or several loans.
The program will calculate your monthly payments and total debt.

List of text Commands :
help: You're reading this now...
view: view a list of the loans in the register
new: add one or more loans into the register
update: edit a loan in the register
delete: delete a loan in the register
interest: edit the Global Interest rate
log: outputs a .txt file with the current view data
quit: exit the program

NOTA BENE:
- Type 'quit' anytime entering data entry to return to the main menu.
- Interest is a single global value: it will affect All loans in the Register.
- Log Files are saved to folder: ../LoanSimulatorV2 --> /logs


FEATURES

- full set of text commands to create, read, update and delete loans
- user data validataion prevents incorrect data entry
- log function exports data in .txt file
- Program runs on Do While Loop until user enters the 'quit' command

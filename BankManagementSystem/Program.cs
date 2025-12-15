using System;

namespace SimpleBankingApp
{
    // Entry point of the application (console UI)
    class Program
    {
        static void Main(string[] args)
        {
            BankingSystem bankingSystem = new BankingSystem();
            bool isRunning = true;

            Console.WriteLine("========== SIMPLE BANKING APPLICATION ==========\n");

            // Main menu loop (runs until user selects "Exit")
            while (isRunning)
            {
                // Display main menu
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Create New Account");
                Console.WriteLine("2. Deposit Funds");
                Console.WriteLine("3. Withdraw Funds");
                Console.WriteLine("4. Check Balance");
                Console.WriteLine("5. Display Account Details");
                Console.WriteLine("6. Exit");
                Console.Write("\nEnter your choice (1-6): ");

                // Handle user input
                string choice = Console.ReadLine();
                Console.WriteLine(); // Empty line for readability

                switch (choice)
                {
                    case "1":
                        CreateNewAccount(bankingSystem);
                        break;
                    case "2":
                        DepositFunds(bankingSystem);
                        break;
                    case "3":
                        WithdrawFunds(bankingSystem);
                        break;
                    case "4":
                        CheckBalance(bankingSystem);
                        break;
                    case "5":
                        DisplayAccountDetails(bankingSystem);
                        break;
                    case "6":
                        isRunning = false;
                        Console.WriteLine("Thank you for using our banking app! Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please enter a number between 1 and 6.\n");
                        break;
                }
            }
        }

        // Helper method: Create new account (separated for modularity)
        static void CreateNewAccount(BankingSystem bankingSystem)
        {
            try
            {
                Console.Write("Enter account holder name: ");
                string name = Console.ReadLine();
                bankingSystem.CreateAccount(name);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError creating account: {ex.Message}\n");
            }
        }

        // Helper method: Deposit funds to an existing account
        static void DepositFunds(BankingSystem bankingSystem)
        {
            try
            {
                Console.Write("Enter your account number: ");
                string accNumber = Console.ReadLine();
                Account account = bankingSystem.FindAccount(accNumber);

                if (account == null)
                {
                    Console.WriteLine("Account not found!\n");
                    return;
                }

                Console.Write("Enter deposit amount: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid amount! Please enter a numeric value.\n");
                    return;
                }

                account.Deposit(amount);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError depositing funds: {ex.Message}\n");
            }
        }

        // Helper method: Withdraw funds from an existing account
        static void WithdrawFunds(BankingSystem bankingSystem)
        {
            try
            {
                Console.Write("Enter your account number: ");
                string accNumber = Console.ReadLine();
                Account account = bankingSystem.FindAccount(accNumber);

                if (account == null)
                {
                    Console.WriteLine("Account not found!\n");
                    return;
                }

                Console.Write("Enter withdrawal amount: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid amount! Please enter a numeric value.\n");
                    return;
                }

                account.Withdraw(amount);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError withdrawing funds: {ex.Message}\n");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}\n");
            }
        }

        // Helper method: Check balance of an existing account
        static void CheckBalance(BankingSystem bankingSystem)
        {
            Console.Write("Enter your account number: ");
            string accNumber = Console.ReadLine();
            Account account = bankingSystem.FindAccount(accNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found!\n");
                return;
            }

            decimal balance = account.CheckBalance();
            Console.WriteLine($"\nCurrent Balance for {account.AccountNumber}: ${balance:F2}\n");
        }

        // Helper method: Display full details of an existing account
        static void DisplayAccountDetails(BankingSystem bankingSystem)
        {
            Console.Write("Enter your account number: ");
            string accNumber = Console.ReadLine();
            Account account = bankingSystem.FindAccount(accNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found!\n");
                return;
            }

            account.DisplayAccountDetails();
        }
    }
}
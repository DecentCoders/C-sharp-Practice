using System;

namespace SimpleBankingApp
{
    // Encapsulates account data and core transaction logic
    public class Account
    {
        // Private fields (encapsulation: restrict direct external access)
        private string _accountNumber;
        private string _accountHolderName;
        private decimal _balance;

        // Public read-only properties (controlled access to private fields)
        public string AccountNumber => _accountNumber;
        public string AccountHolderName 
        { 
            get => _accountHolderName; 
            set => _accountHolderName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Name cannot be empty!");
        }
        public decimal Balance => _balance;

        // Constructor: Initialize account with valid data
        public Account(string accountNumber, string accountHolderName)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty!");
            if (string.IsNullOrWhiteSpace(accountHolderName))
                throw new ArgumentException("Account holder name cannot be empty!");

            _accountNumber = accountNumber;
            _accountHolderName = accountHolderName;
            _balance = 0.00m; // Use decimal for currency (precision critical)
        }

        // Deposit funds (validates positive amount)
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than 0!");
            
            _balance += amount;
            Console.WriteLine($"\nSuccessfully deposited: ${amount:F2}");
        }

        // Withdraw funds (validates positive amount and sufficient balance)
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than 0!");
            if (amount > _balance)
                throw new InvalidOperationException("Insufficient balance for withdrawal!");
            
            _balance -= amount;
            Console.WriteLine($"\nSuccessfully withdrew: ${amount:F2}");
        }

        // Return current balance (user-friendly wrapper for Balance property)
        public decimal CheckBalance()
        {
            return _balance;
        }

        // Display full account details in a readable format
        public void DisplayAccountDetails()
        {
            Console.WriteLine("\n================ ACCOUNT DETAILS ================");
            Console.WriteLine($"Account Number: {_accountNumber}");
            Console.WriteLine($"Account Holder: {_accountHolderName}");
            Console.WriteLine($"Current Balance: ${_balance:F2}");
            Console.WriteLine("=================================================\n");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBankingApp
{
    // Manages the collection of accounts and account creation logic
    public class BankingSystem
    {
        // Private list to store accounts (encapsulation: no external modification)
        private List<Account> _accounts;
        private int _nextAccountId; // Auto-generate unique account numbers

        // Constructor: Initialize account list and starting ID
        public BankingSystem()
        {
            _accounts = new List<Account>();
            _nextAccountId = 1000; // Start account numbers from 1000 (e.g., ACC1000)
        }

        // Create a new account with auto-generated unique account number
        public Account CreateAccount(string accountHolderName)
        {
            string accountNumber = $"ACC{_nextAccountId++}"; // Format: ACC1000, ACC1001, etc.
            Account newAccount = new Account(accountNumber, accountHolderName);
            _accounts.Add(newAccount);
            
            Console.WriteLine($"\nAccount created successfully!");
            Console.WriteLine($"Your Account Number: {accountNumber}");
            return newAccount;
        }

        // Find an account by account number (helper method for transactions)
        public Account FindAccount(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return null;
            
            return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        // Return a copy of all accounts (prevent external modification of the original list)
        public List<Account> GetAllAccounts()
        {
            return new List<Account>(_accounts);
        }
    }
}
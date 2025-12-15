using System;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public class Member
    {
        // Properties
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public List<Book> BorrowedBooks { get; private set; }

        // Constructor with validation (prevents invalid member creation)
        public Member(int id, string name, string email)
        {
            // Validate name (non-empty/whitespace)
            if (string.IsNullOrEmpty(name?.Trim()))
            {
                throw new ArgumentException("Member name cannot be empty or just whitespace.", nameof(name));
            }

            // Validate email (non-empty + basic format check for "@")
            if (string.IsNullOrEmpty(email?.Trim()) || !email.Trim().Contains("@"))
            {
                throw new ArgumentException("Invalid email format. Must contain '@' and not be empty.", nameof(email));
            }

            Id = id;
            Name = name.Trim();
            Email = email.Trim();
            BorrowedBooks = new List<Book>(); // Initialize empty list of borrowed books
        }

        // Optional: For display purposes
        public override string ToString()
        {
            return $"ID: {Id} | Name: {Name} | Email: {Email} | Borrowed Books: {BorrowedBooks.Count}";
        }
    }
}
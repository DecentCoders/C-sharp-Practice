using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public class LibrarySystem
    {
        // Collections to store books and members
        private List<Book> _books = new List<Book>();
        private List<Member> _members = new List<Member>();

        // Auto-increment IDs for books/members
        private int _nextBookId = 1;
        private int _nextMemberId = 1;

        // Add a new book (called after input validation in Program.cs)
        public void AddBook(string title, string author)
        {
            try
            {
                Book newBook = new Book(_nextBookId++, title, author);
                _books.Add(newBook);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Book '{title}' by {author} added successfully (ID: {newBook.Id})!");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Failed to add book: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Add a new member (called after input validation in Program.cs)
        public void AddMember(string name, string email)
        {
            try
            {
                Member newMember = new Member(_nextMemberId++, name, email);
                _members.Add(newMember);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Member '{name}' added successfully (ID: {newMember.Id})!");
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Failed to add member: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Borrow a book (validate IDs exist before borrowing)
        public void BorrowBook(int memberId, int bookId)
        {
            // Find member by ID
            Member member = _members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Member with ID {memberId} not found!");
                Console.ResetColor();
                return;
            }

            // Find book by ID
            Book book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Book with ID {bookId} not found!");
                Console.ResetColor();
                return;
            }

            // Check if book is already borrowed
            if (book.IsBorrowed)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Book '{book.Title}' (ID: {bookId}) is already borrowed!");
                Console.ResetColor();
                return;
            }

            // All checks passed: borrow the book
            book.IsBorrowed = true;
            member.BorrowedBooks.Add(book);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ Book '{book.Title}' borrowed by {member.Name} successfully!");
            Console.ResetColor();
        }

        // Return a book (validate IDs exist before returning)
        public void ReturnBook(int memberId, int bookId)
        {
            // Find member by ID
            Member member = _members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Member with ID {memberId} not found!");
                Console.ResetColor();
                return;
            }

            // Find book by ID
            Book book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Book with ID {bookId} not found!");
                Console.ResetColor();
                return;
            }

            // Check if member borrowed this book
            if (!member.BorrowedBooks.Contains(book))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Member {member.Name} did not borrow book '{book.Title}' (ID: {bookId})!");
                Console.ResetColor();
                return;
            }

            // All checks passed: return the book
            book.IsBorrowed = false;
            member.BorrowedBooks.Remove(book);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ Book '{book.Title}' returned by {member.Name} successfully!");
            Console.ResetColor();
        }

        // Show all borrowed books
        public void ShowBorrowedBooks()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📋 ALL BORROWED BOOKS");
            Console.WriteLine("----------------------");
            Console.ResetColor();

            var borrowedBooks = _books.Where(b => b.IsBorrowed).ToList();
            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("ℹ️ No books are currently borrowed.");
                return;
            }

            foreach (var book in borrowedBooks)
            {
                Console.WriteLine(book);
            }
        }

        // List all books (available + borrowed)
        public void ListAllBooks()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📚 ALL BOOKS IN LIBRARY");
            Console.WriteLine("------------------------");
            Console.ResetColor();

            if (_books.Count == 0)
            {
                Console.WriteLine("ℹ️ No books in the library yet.");
                return;
            }

            foreach (var book in _books)
            {
                Console.WriteLine(book);
            }
        }

        // List all members
        public void ListAllMembers()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n👥 ALL LIBRARY MEMBERS");
            Console.WriteLine("-----------------------");
            Console.ResetColor();

            if (_members.Count == 0)
            {
                Console.WriteLine("ℹ️ No members in the library yet.");
                return;
            }

            foreach (var member in _members)
            {
                Console.WriteLine(member);
            }
        }
    }
}
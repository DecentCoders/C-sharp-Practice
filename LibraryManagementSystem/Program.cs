using System;

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize library system
            LibrarySystem library = new LibrarySystem();
            bool isRunning = true;

            // Welcome message
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------ LIBRARY MANAGEMENT SYSTEM------------");            Console.ResetColor();

            // Main menu loop
            while (isRunning)
            {
                // Show menu
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n📋 MAIN MENU");
                Console.WriteLine("------------");
                Console.ResetColor();
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. Add New Member");
                Console.WriteLine("3. Borrow a Book");
                Console.WriteLine("4. Return a Book");
                Console.WriteLine("5. Show Borrowed Books");
                Console.WriteLine("6. List All Books");
                Console.WriteLine("7. List All Members");
                Console.WriteLine("8. Exit");
                Console.Write("\nEnter your choice (1-8): ");

                // Validate menu choice (numeric + 1-8 range)
                string choiceInput = Console.ReadLine()?.Trim();
                if (!int.TryParse(choiceInput, out int menuChoice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Invalid input! Please enter a NUMBER between 1-8.");
                    Console.ResetColor();
                    continue; // Restart loop to show menu again
                }

                // Handle menu choice
                switch (menuChoice)
                {
                    case 1:
                        AddNewBook(library);
                        break;
                    case 2:
                        AddNewMember(library);
                        break;
                    case 3:
                        BorrowBook(library);
                        break;
                    case 4:
                        ReturnBook(library);
                        break;
                    case 5:
                        library.ShowBorrowedBooks();
                        break;
                    case 6:
                        library.ListAllBooks();
                        break;
                    case 7:
                        library.ListAllMembers();
                        break;
                    case 8:
                        isRunning = false;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n👋 Exiting Library System... Thank you!");
                        Console.ResetColor();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n❌ Invalid choice! Please enter a number between 1-8.");
                        Console.ResetColor();
                        break;
                }

                // Pause before showing menu again (improves UX)
                if (isRunning)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear(); // Clear screen for clean menu display
                }
            }
        }

        // Add new book (with input validation for title/author)
        private static void AddNewBook(LibrarySystem library)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n📖 ADD NEW BOOK");
            Console.WriteLine("----------------");
            Console.ResetColor();

            Console.Write("Enter book title: ");
            string title = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error: Book title cannot be empty or just whitespace!");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter book author: ");
            string author = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(author))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error: Author name cannot be empty or just whitespace!");
                Console.ResetColor();
                return;
            }

            library.AddBook(title, author);
        }

        // Add new member (with input validation for name/email)
        private static void AddNewMember(LibrarySystem library)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n👥 ADD NEW MEMBER");
            Console.WriteLine("------------------");
            Console.ResetColor();

            Console.Write("Enter member name: ");
            string name = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error: Member name cannot be empty or just whitespace!");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter member email: ");
            string email = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Error: Invalid email format! Must contain '@' and not be empty.");
                Console.ResetColor();
                return;
            }

            library.AddMember(name, email);
        }

        // Borrow book (with input validation for positive integer IDs)
        private static void BorrowBook(LibrarySystem library)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n📤 BORROW A BOOK");
            Console.WriteLine("-----------------");
            Console.ResetColor();

            Console.Write("Enter member ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId) || memberId <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid member ID! Must be a positive whole number (1, 2, 3...).");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter book ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId) || bookId <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid book ID! Must be a positive whole number (1, 2, 3...).");
                Console.ResetColor();
                return;
            }

            library.BorrowBook(memberId, bookId);
        }

        // Return book (with input validation for positive integer IDs)
        private static void ReturnBook(LibrarySystem library)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n📥 RETURN A BOOK");
            Console.WriteLine("-----------------");
            Console.ResetColor();

            Console.Write("Enter member ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId) || memberId <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid member ID! Must be a positive whole number (1, 2, 3...).");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter book ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId) || bookId <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid book ID! Must be a positive whole number (1, 2, 3...).");
                Console.ResetColor();
                return;
            }

            library.ReturnBook(memberId, bookId);
        }
    }
}
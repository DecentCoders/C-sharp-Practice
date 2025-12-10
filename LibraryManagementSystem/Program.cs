namespace SimpleLibrarySystem;

class Program
{
    static void Main(string[] args)
    {
        // Initialize library system
        var library = new LibrarySystem();

        // Subscribe to notification events (colorful alerts)
        library.Notification += (message) => 
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n📢 {message}");
            Console.ResetColor();
        };

        // Main menu loop
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            
            // === Colorful Header ===
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===================== LIBRARY MANAGEMENT SYSTEM =====================");
            Console.ResetColor();

            // === Menu Options (Colored) ===
            Console.ForegroundColor = ConsoleColor.Gray; // Fixed: LightGray → Gray
            Console.WriteLine("\n[1] Add a new book");
            Console.WriteLine("[2] Add a new member");
            Console.WriteLine("[3] Borrow a book");
            Console.WriteLine("[4] Return a book");
            Console.WriteLine("[5] View borrowed books");
            Console.WriteLine("[6] View all books");
            Console.WriteLine("[7] View all members");
            Console.WriteLine("[8] Exit");
            Console.ResetColor();

            // === Input Prompt ===
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nEnter your choice (1-8): ");
            Console.ResetColor();

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNewBook(library);
                    break;
                case "2":
                    AddNewMember(library);
                    break;
                case "3":
                    BorrowBook(library);
                    break;
                case "4":
                    ReturnBook(library);
                    break;
                case "5":
                    library.ShowBorrowedBooks();
                    break;
                case "6":
                    library.ListAllBooks();
                    break;
                case "7":
                    library.ListAllMembers();
                    break;
                case "8":
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

            // Pause before returning to menu (except exit)
            if (isRunning)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\nPress any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }

    // Helper: Add new book (colored input prompts)
    private static void AddNewBook(LibrarySystem library)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n📖 ADD NEW BOOK");
        Console.WriteLine("----------------");
        Console.ResetColor();

        Console.Write("Enter book title: ");
        string title = Console.ReadLine();
        Console.Write("Enter book author: ");
        string author = Console.ReadLine();
        
        library.AddBook(title, author);
    }

    // Helper: Add new member (colored input prompts)
    private static void AddNewMember(LibrarySystem library)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n👥 ADD NEW MEMBER");
        Console.WriteLine("------------------");
        Console.ResetColor();

        Console.Write("Enter member name: ");
        string name = Console.ReadLine();
        Console.Write("Enter member email: ");
        string email = Console.ReadLine();
        
        library.AddMember(name, email);
    }

    // Helper: Borrow book (colored input + validation)
    private static void BorrowBook(LibrarySystem library)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n📤 BORROW A BOOK");
        Console.WriteLine("-----------------");
        Console.ResetColor();

        Console.Write("Enter member ID: ");
        if (!int.TryParse(Console.ReadLine(), out int memberId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid member ID (must be a number)!");
            Console.ResetColor();
            return;
        }
        
        Console.Write("Enter book ID: ");
        if (!int.TryParse(Console.ReadLine(), out int bookId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid book ID (must be a number)!");
            Console.ResetColor();
            return;
        }
        
        library.BorrowBook(memberId, bookId);
    }

    // Helper: Return book (colored input + validation)
    private static void ReturnBook(LibrarySystem library)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n📥 RETURN A BOOK");
        Console.WriteLine("-----------------");
        Console.ResetColor();

        Console.Write("Enter member ID: ");
        if (!int.TryParse(Console.ReadLine(), out int memberId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid member ID (must be a number)!");
            Console.ResetColor();
            return;
        }
        
        Console.Write("Enter book ID: ");
        if (!int.TryParse(Console.ReadLine(), out int bookId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid book ID (must be a number)!");
            Console.ResetColor();
            return;
        }
        
        library.ReturnBook(memberId, bookId);
    }
}
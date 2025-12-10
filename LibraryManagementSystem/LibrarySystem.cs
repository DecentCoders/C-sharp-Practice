namespace SimpleLibrarySystem;

public class LibrarySystem
{
    // Core collections (initialized in constructor)
    private List<Book> _books = new List<Book>();
    private List<Member> _members = new List<Member>();
    private List<BorrowRecord> _borrowRecords = new List<BorrowRecord>();

    // Auto-increment IDs
    private int _nextBookId = 1;
    private int _nextMemberId = 1;
    private int _nextRecordId = 1;

    // Event for notifications (e.g., borrow/return alerts)
    public event Action<string> Notification;

    // Constructor: Initialize collections
    public LibrarySystem()
    {
        _books = new List<Book>();
        _members = new List<Member>();
        _borrowRecords = new List<BorrowRecord>();
    }

    // Add a new book to the library (colored success message)
    public void AddBook(string title, string author)
    {
        var newBook = new Book(_nextBookId++, title, author);
        _books.Add(newBook);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Book added: {title} (ID: {newBook.BookId})");
        Console.ResetColor();
    }

    // Add a new member to the library (colored success message)
    public void AddMember(string name, string email)
    {
        var newMember = new Member(_nextMemberId++, name, email);
        _members.Add(newMember);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Member added: {name} (ID: {newMember.MemberId})");
        Console.ResetColor();
    }

    // Borrow a book (core logic + colored feedback)
    public void BorrowBook(int memberId, int bookId)
    {
        // Find member and book
        var member = _members.FirstOrDefault(m => m.MemberId == memberId);
        var book = _books.FirstOrDefault(b => b.BookId == bookId);

        // Validation checks (red errors)
        if (member == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Error: Member not found!");
            Console.ResetColor();
            return;
        }
        if (book == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Error: Book not found!");
            Console.ResetColor();
            return;
        }
        if (!book.IsAvailable)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Error: Book is already borrowed!");
            Console.ResetColor();
            return;
        }

        // Create borrow record
        var newRecord = new BorrowRecord(_nextRecordId++, bookId, memberId);
        _borrowRecords.Add(newRecord);

        // Update book/member state
        book.IsAvailable = false;
        member.BorrowedBookIds.Add(bookId);

        // Trigger notification
        Notification?.Invoke($"{member.Name} borrowed '{book.Title}' (Book ID: {bookId})");
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Book borrowed successfully!");
        Console.ResetColor();
    }

    // Return a book (core logic + colored feedback)
    public void ReturnBook(int memberId, int bookId)
    {
        // Find active borrow record (not returned)
        var record = _borrowRecords.FirstOrDefault(br => 
            br.MemberId == memberId && br.BookId == bookId && !br.IsReturned);

        if (record == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Error: No active borrow record found for this member/book!");
            Console.ResetColor();
            return;
        }

        // Find book/member
        var book = _books.FirstOrDefault(b => b.BookId == bookId);
        var member = _members.FirstOrDefault(m => m.MemberId == memberId);

        // Update record/book/member state
        record.ReturnDate = DateTime.Now;
        book.IsAvailable = true;
        member.BorrowedBookIds.Remove(bookId);

        // Trigger notification
        Notification?.Invoke($"{member.Name} returned '{book.Title}' (Book ID: {bookId})");
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Book returned successfully!");
        Console.ResetColor();
    }

    // Show all borrowed books (colored section + details)
    public void ShowBorrowedBooks()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n📚 CURRENT BORROWED BOOKS");
        Console.WriteLine("---------------------------");
        Console.ResetColor();

        var borrowedBooks = _books
            .Where(b => !b.IsAvailable)
            .Join(
                _borrowRecords.Where(br => !br.IsReturned),
                book => book.BookId,
                record => record.BookId,
                (book, record) => new { Book = book, Record = record }
            )
            .Join(
                _members,
                br => br.Record.MemberId,
                member => member.MemberId,
                (br, member) => new 
                { 
                    BookTitle = br.Book.Title,
                    BookId = br.Book.BookId,
                    BorrowerName = member.Name,
                    BorrowerId = member.MemberId,
                    BorrowDate = br.Record.BorrowDate.ToString("dd/MM/yyyy HH:mm")
                }
            );

        if (!borrowedBooks.Any())
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("No books are currently borrowed.");
            Console.ResetColor();
            return;
        }

        foreach (var item in borrowedBooks)
        {
            Console.WriteLine($"📖 Book ID: {item.BookId} | Title: {item.BookTitle}");
            Console.WriteLine($"👤 Borrowed by: {item.BorrowerName} (ID: {item.BorrowerId})");
            Console.WriteLine($"📅 Borrow Date: {item.BorrowDate}\n");
        }
    }

    // List all books (colored section + availability status)
    public void ListAllBooks()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n📖 ALL LIBRARY BOOKS");
        Console.WriteLine("----------------------");
        Console.ResetColor();

        if (!_books.Any())
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("No books in the library yet.");
            Console.ResetColor();
            return;
        }

        foreach (var book in _books)
        {
            Console.Write($"ID: {book.BookId} | Title: {book.Title} | Author: {book.Author} | ");
            
            if (book.IsAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Available");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Borrowed");
            }
            Console.ResetColor();
        }
    }

    // List all members (colored section + borrow status)
    public void ListAllMembers()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n👥 LIBRARY MEMBERS");
        Console.WriteLine("-------------------");
        Console.ResetColor();

        if (!_members.Any())
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("No members in the library yet.");
            Console.ResetColor();
            return;
        }

        foreach (var member in _members)
        {
            Console.Write($"ID: {member.MemberId} | Name: {member.Name} | Email: {member.Email} | ");
            
            if (member.BorrowedBookIds.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{member.BorrowedBookIds.Count} book(s) borrowed");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("No books borrowed");
            }
            Console.ResetColor();
        }
    }
}
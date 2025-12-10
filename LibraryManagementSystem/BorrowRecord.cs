namespace SimpleLibrarySystem;

public class BorrowRecord
{
    public int RecordId { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned => ReturnDate.HasValue; // True if book is returned

    public BorrowRecord(int recordId, int bookId, int memberId)
    {
        RecordId = recordId;
        BookId = bookId;
        MemberId = memberId;
        BorrowDate = DateTime.Now; // Auto-set borrow date
    }
}

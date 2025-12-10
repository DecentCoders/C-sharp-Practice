namespace SimpleLibrarySystem;

public class Member
{
    public int MemberId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<int> BorrowedBookIds { get; set; } = new List<int>(); // Track borrowed book IDs

    public Member(int memberId, string name, string email)
    {
        MemberId = memberId;
        Name = name;
        Email = email;
    }
}
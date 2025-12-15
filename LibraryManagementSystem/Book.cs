using System;

namespace LibraryManagementSystem
{
    public class Book
    {
        // Properties
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public bool IsBorrowed { get; set; }

        // Constructor with validation (prevents invalid book creation)
        public Book(int id, string title, string author)
        {
            // Validate title (non-empty/whitespace)
            if (string.IsNullOrEmpty(title?.Trim()))
            {
                throw new ArgumentException("Book title cannot be empty or just whitespace.", nameof(title));
            }

            // Validate author (non-empty/whitespace)
            if (string.IsNullOrEmpty(author?.Trim()))
            {
                throw new ArgumentException("Author name cannot be empty or just whitespace.", nameof(author));
            }

            Id = id;
            Title = title.Trim();
            Author = author.Trim();
            IsBorrowed = false; // Default: book is available
        }

        // Optional: For display purposes
        public override string ToString()
        {
            return $"ID: {Id} | Title: {Title} | Author: {Author} | Status: {(IsBorrowed ? "Borrowed" : "Available")}";
        }
    }
}
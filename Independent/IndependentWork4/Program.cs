using System;
using System.Collections.Generic;

namespace IndependentWork4
{
    public class LibraryItem
    {
        private string _uniqueId;
        private string _title;
        private int _publicationYear;

        public string UniqueId => _uniqueId;
        public string Title => _title;
        public int PublicationYear => _publicationYear;

        public LibraryItem(string uniqueId, string title, int publicationYear)
        {
            _uniqueId = uniqueId;
            _title = title;
            _publicationYear = publicationYear;
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"[ID: {UniqueId}] Назва: \"{Title}\", Рік видання: {PublicationYear}");
        }
    }

    public class Book : LibraryItem
    {
        private string _author;

        public string Author => _author;

        public Book(string uniqueId, string title, int publicationYear, string author)
            : base(uniqueId, title, publicationYear)
        {
            _author = author;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"   Автор: {Author}");
        }
    }

    public class Journal : LibraryItem
    {
        private int _issueNumber;

        public int IssueNumber => _issueNumber;

        public Journal(string uniqueId, string title, int publicationYear, int issueNumber)
            : base(uniqueId, title, publicationYear)
        {
            _issueNumber = issueNumber;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"   Випуск №: {IssueNumber}");
        }
    }

    public class AudioBook : Book
    {
        private string _narrator;

        public string Narrator => _narrator;

        public AudioBook(string uniqueId, string title, int publicationYear, string author, string narrator)
            : base(uniqueId, title, publicationYear, author)
        {
            _narrator = narrator;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"   Диктор/Оповідач: {Narrator}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<LibraryItem> items = new List<LibraryItem>
            {
                new Book("B-001", "Кобзар", 1840, "Тарас Шевченко"),
                new Journal("J-102", "Cybersecurity & IT", 2024, 42),
                new AudioBook("AB-505", "Тигролови", 1944, "Іван Багряний", "Олександр Бондаренко")
            };

            foreach (LibraryItem item in items)
            {
                item.DisplayDetails();
            }
        }
    }
}

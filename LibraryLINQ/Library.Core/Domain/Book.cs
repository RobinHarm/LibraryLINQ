using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }
        ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}

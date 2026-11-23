using Library.Core.Domain;

namespace LibraryLINQ.LINQ
{
    public class LINQclass
    {
        public List<Book> BooksYearFind
            (List<Book> books, string year)
        {
            return books
                .Where(x => x.PublishedYear.ToString().Contains(year))
                .ToList();
        }

        public List<Book> BooksTitle
            (List<Book> books, string Title)
        {
            return books
                .Where(x => x.Title.ToLower().Contains(Title.ToLower()))
                .ToList();
        }

        public List<Book> Unavailablebooks
            (List<Book> books)
        {
            return books
                .Where(x => !x.IsAvailable)
                .ToList();
        }

        public List<Loan> LoansReaders
            (List<Loan> loans,  Guid readerId)
        {
            return loans
                .Where(x => x.ReaderId == readerId)
                .ToList();
        }
    }
}

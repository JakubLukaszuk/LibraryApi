using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public class BookRepository : IBookRepository
    {
        private LibraryDbContext _bookDbContext;

        public BookRepository(LibraryDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public Book GetBook(int bookId)
        {
           return _bookDbContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBook(string isbn)
        {
            return _bookDbContext.Books.Where(b => b.Isbn == isbn).FirstOrDefault();
        }

        public decimal GetBookRating(int bookId)
        {
            var reviews = _bookDbContext.Reviews.Where(r => r.Book.Id == bookId);
            if (reviews.Count() <= 0)
            {
                return 0;
            }

            return reviews.Sum(r => r.Rating) / reviews.Count();
        }

        public ICollection<Book> GetBooks()
        {
            return _bookDbContext.Books.ToList();
        }

        public bool IsBookExist(int bookId)
        {
            return _bookDbContext.Books.Any(b => b.Id == b.Id);
        }

        public bool IsDuplicateISBN(string isbn, int bookId)
        {
            var book = _bookDbContext.Books.Where(b =>
                b.Isbn.Trim().ToUpper() == isbn.Trim().ToUpper() && b.Id == bookId);

            return book ==  null ? false : true;
        }
    }
}

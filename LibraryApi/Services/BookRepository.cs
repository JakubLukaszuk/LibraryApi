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

        public bool CreateBook(Book book, List<int> authorsId, List<int> categoriesId)
        {
            IEnumerable<Author> authors = _bookDbContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            IEnumerable<Category> categories = _bookDbContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            foreach (var author in authors)
            {
                BookAuthor bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _bookDbContext.Add(bookAuthor);
            }


            foreach (var category in categories)
            {
                BookCategory bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };
                _bookDbContext.Add(bookCategory);
            }

            _bookDbContext.Add(book);

            return Save();

        }

        public bool DeleteBook(Book book)
        {
            _bookDbContext.Remove(book);
            return Save();
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
            return _bookDbContext.Books.Any(b => b.Id == bookId);
        }

        public bool IsDuplicateISBN(string isbn, int bookId)
        {
            var book = _bookDbContext.Books.Where(b =>
                b.Isbn.Trim().ToUpper() == isbn.Trim().ToUpper() && b.Id == bookId).FirstOrDefault();

            return book ==  null ? false : true;
        }

        public bool Save()
        {
            return _bookDbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateBook(Book book, List<int> authorsId, List<int> categoriesId)
        {
            IEnumerable<Author> authors = _bookDbContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            IEnumerable<Category> categories = _bookDbContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            IEnumerable<BookAuthor> bookAuthorsToDelete = _bookDbContext.BookAuthors.Where(b => b.BookId == book.Id);
            IEnumerable<BookCategory> bookCategoriesToDelete = _bookDbContext.BookCategories.Where(b => b.BookId == book.Id);

            _bookDbContext.RemoveRange(bookAuthorsToDelete);
            _bookDbContext.RemoveRange(bookCategoriesToDelete);


            foreach (var author in authors)
            {
                BookAuthor bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _bookDbContext.Add(bookAuthor);
            }


            foreach (var category in categories)
            {
                BookCategory bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };
                _bookDbContext.Add(bookCategory);
            }

            _bookDbContext.Add(book);

            return Save();
        }
    }
}

using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        Book GetBook(int bookId);
        Book GetBook(string isbn);
        bool IsBookExist(int bookId);
        bool IsDuplicateISBN(string isbn, int bookId);
        decimal GetBookRating(int bookId);

        bool CreateBook(Book book, List<int> authorsId, List<int> categoriesId);
        bool UpdateBook(Book book, List<int> authorsId, List<int> categoriesId);
        bool DeleteBook(Book book);
        bool Save();
    }
}

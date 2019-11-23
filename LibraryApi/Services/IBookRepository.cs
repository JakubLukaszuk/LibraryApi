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
        bool IsBookExist(int bookId);
        bool IsDuplicateISBN(string isbn);
        decimal GetBookRating(int bookId);
    }
}

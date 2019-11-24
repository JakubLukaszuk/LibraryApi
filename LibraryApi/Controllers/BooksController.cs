using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Dtos;
using LibraryApi.Services;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDataTransferObjects>))]
        public IActionResult GetBooks()
        {
            ICollection<Book> books = _bookRepository.GetBooks();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDtoList = new List<BookDataTransferObjects>();
            foreach (Book book in books)
            {
                booksDtoList.Add(new BookDataTransferObjects
                {
                    Id = book.Id,
                    Title = book.Title,
                    Ibsn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDtoList);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDataTransferObjects))]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.IsBookExist(bookId))
                return NotFound();

            Book book = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var bookDto = new BookDataTransferObjects
            {
                Id = book.Id,
                Title = book.Title,
                Ibsn = book.Isbn,
                DatePublished = book.DatePublished
            };

            return Ok(bookDto);
        }

        [HttpGet("{bookId}/raiting")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public IActionResult GetBookRaiting(int bookId)
        {
            if (!_bookRepository.IsBookExist(bookId))
                return NotFound();

            decimal rating = _bookRepository.GetBookRating(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

    }
}

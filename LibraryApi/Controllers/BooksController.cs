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
        private ICategoryRepository _categoryRepository;
        private IAuthorRepository _authorRepository;
        private IReviewRepository _reviewRepository;

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository, IAuthorRepository authorRepository, IReviewRepository reviewRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _reviewRepository = reviewRepository;
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

        [HttpGet("{bookId}", Name = "GetBook")]
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

        //api/books?authId=1&authId=2&catId=1&catId=2
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Book))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateBook([FromQuery] List<int> authIds, [FromQuery] List<int> categoryIds, [FromBody] Book bookToCreate)
        {
            StatusCodeResult statusCode = ValdateBook(authIds, categoryIds, bookToCreate);

            if (ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (_bookRepository.CreateBook(bookToCreate, authIds, categoryIds))
            {
                ModelState.AddModelError("", $"Somethink went wrong savving book");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBook", new { bookId = bookToCreate.Id }, bookToCreate);
        }

        //api/books/bookId?authId=1&authId=2&catId=1&catId=2
        [HttpPut("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBook(int bookId, [FromQuery] List<int> authIds, [FromQuery] List<int> categoryIds, [FromBody] Book bookToUpdate)
        {
            StatusCodeResult statusCode = ValdateBook(authIds, categoryIds, bookToUpdate);

            if (bookId == bookToUpdate.Id)
                return BadRequest();

            if (!_bookRepository.IsBookExist(bookId))
                return NotFound();

            if (ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (_bookRepository.UpdateBook(bookToUpdate, authIds, categoryIds))
            {
                ModelState.AddModelError("", $"Somethink went wrong updating book");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBook", new { bookId = bookToUpdate.Id }, bookToUpdate);
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer(int bookId)
        {
            if (!_bookRepository.IsBookExist(bookId))
            {
                return BadRequest(ModelState);
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfBook(bookId);
            var bookToDelete = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Somethink went wrong. Reviews not deleted");
                return StatusCode(500, ModelState);
            }

            if (_bookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("", $"Somethink went wrong. Book: {bookToDelete.Title} not deleted");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        private StatusCodeResult ValdateBook(List<int> authId, List<int> categoryIds, Book books)
        {
            if (books == null)
            {
                ModelState.AddModelError("", "Missing book");
                return BadRequest();
            }
            else if(authId.Count() <= 0)
            {
                ModelState.AddModelError("", "Missing author id");
                return BadRequest();
            }
            else if(categoryIds.Count() <= 0)
            {
                ModelState.AddModelError("", "Missing category id");
                return BadRequest();
            }
            
            if(_bookRepository.IsDuplicateISBN(books.Isbn, books.Id))
            {
                ModelState.AddModelError("", "Duplicate ISBN");
                return StatusCode(422);
            }

            foreach (var id in authId)
            {
                if (!_authorRepository.IsAuthorExist(id))
                {
                    ModelState.AddModelError("", "Author Not Found");
                    return StatusCode(404);
                }
            }


            foreach (var id in categoryIds)
            {
                if (!_categoryRepository.IsCategoryExist(id))
                {
                    ModelState.AddModelError("", "Category Not Found");
                    return StatusCode(404);
                }
            }

            if(ModelState.IsValid)
            {
                ModelState.AddModelError("", "Critical Error");
                return BadRequest();
            }

            return NoContent();
        }
    }
}

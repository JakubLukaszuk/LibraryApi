using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Services;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetAuthors()
        {
            ICollection<Author> authors = _authorRepository.GetAuthors();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDtoList = new List<AuthorDataTransferObjects>();
            foreach (Author author in authors)
            {
                authorsDtoList.Add(new AuthorDataTransferObjects
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                });
            }
            return Ok(authorsDtoList);
        }


        [HttpGet("{authorId}", Name="GetAuthor")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.IsAuthorExist(authorId))
                return NotFound();

            Author author = _authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorDto = new AuthorDataTransferObjects
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                };
            

            return Ok(authorDto);
        }

        [HttpGet("book/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetAuthorsOfBook(int bookId)
        {
            if (!_bookRepository.IsBookExist(bookId))
            {
                bool x = _bookRepository.IsBookExist(bookId);
                return NotFound();
            }

            ICollection<Author> authors = _authorRepository.GetAuthorsOfBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDtoList = new List<AuthorDataTransferObjects>();
            foreach (Author author in authors)
            {
                authorsDtoList.Add(new AuthorDataTransferObjects
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                });
            }
            return Ok(authorsDtoList);
        }

        [HttpGet("{authorId}/book")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepository.IsAuthorExist(authorId))
                return NotFound();

            ICollection<Book> books = _authorRepository.GetBooksByAuthor(authorId);

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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Author))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateAuthor([FromBody] Author authorToCreate)
        {
            if (authorToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_authorRepository.IsAuthorExist(authorToCreate.Id))
            {
                ModelState.AddModelError("", "Author exist");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("", "Somethink went wrong when saving author! Review has not been saved");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAuthor", new { reviewId = authorToCreate.Id }, authorToCreate);
        }


        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateAuthor([FromBody] Author authorToUpdate)
        {
            if (authorToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_authorRepository.IsAuthorExist(authorToUpdate.Id))
            {
                ModelState.AddModelError("", "Author not exist");
            }

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            if (!_authorRepository.UpdateAuthor(authorToUpdate))
            {
                ModelState.AddModelError("", "Somethink went wrong when updating author! Review has not been saved");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        //api/countries/countryId
        [HttpDelete("{authorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepository.IsAuthorExist(authorId))
            {
                return NotFound();
            }

            Author authorToDelete = _authorRepository.GetAuthor(authorId);

            ICollection<Book> books = _authorRepository.GetBooksByAuthor(authorId);


            if (books.Count() > 0)
            {
                ModelState.AddModelError("", "At first you have to delete this books: \n" + books.ToString() +
                                             $"Has not {authorToDelete.FirstName} {authorToDelete.LastName} delted");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepository.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("", $"Somethink went wrong.Has not {authorToDelete.FirstName} {authorToDelete.LastName} delted");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}

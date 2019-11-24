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

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
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


        [HttpGet("{authorId}")]
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

        [HttpGet("author/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetAuthorsOfBook(int bookId)
        {

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

    }
}

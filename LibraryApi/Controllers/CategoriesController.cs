using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Dtos;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDataTransferObjects>))]
        public IActionResult GetCategories()
        {

            ICollection<Category> categoires = _categoryRepository.GetCategories();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countreisDtoList = new  List<CategoryDataTransferObjects>();
            foreach (Category category in categoires)
            {
                countreisDtoList.Add( new CategoryDataTransferObjects
                {
                    Name = category.Name,
                    Id = category.Id
                });

            }

            return Ok(countreisDtoList);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDataTransferObjects))]
        public IActionResult GetCategory(int categoryId)
        {
            Category category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CategoryDataTransferObjects categoryDto =
                new CategoryDataTransferObjects
                {
                    Name = category.Name,
                    Id = category.Id
                };

            return Ok(categoryDto);
        }

        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDataTransferObjects>))]
        public IActionResult GetCategoriesOfBook(int bookId)
        {
            IEnumerable<Category> categoryies = _categoryRepository.GetCategoriesOfBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countreisDtoList = new List<CategoryDataTransferObjects>();

            foreach (var category in categoryies)
            {
                countreisDtoList.Add(new CategoryDataTransferObjects
                {
                    Name = category.Name,
                    Id = category.Id
                });
            }

            return Ok(countreisDtoList);
        }

        //api/countries/{countryId}/authors
        [HttpGet("{categoryId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetAllBooksForCategory(int categoryId)
        {
            if (!_categoryRepository.IsCategoryExist(categoryId))
            {
                return NotFound();
            }

            IEnumerable<Book> authors = _categoryRepository.GetBooksOfCategories(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<BookDataTransferObjects> bookDotsList = new List<BookDataTransferObjects>();

            foreach (var book in bookDotsList)
            {
                bookDotsList.Add(
                    new BookDataTransferObjects
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Ibsn = book.Ibsn,
                        DatePublished = book.DatePublished
                    });
            }
            return Ok(bookDotsList);
        }
    }

}

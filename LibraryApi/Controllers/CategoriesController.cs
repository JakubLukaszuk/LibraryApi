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
        private IBookRepository _bookRepository;

        public CategoriesController(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
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

        [HttpGet("{categoryId}" , Name ="GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDataTransferObjects))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.IsCategoryExist(categoryId))
                return NotFound();

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
            if (!_bookRepository.IsBookExist(bookId))
            {
                return NotFound();
            }

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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody] Category categoryToCreate)
        {
            if (categoryToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _categoryRepository.GetCategories().Where(c =>
                c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"Category: {categoryToCreate.Name} already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.CreateCategory(categoryToCreate))
            {
                ModelState.AddModelError("", $"Somethink went wrong. {categoryToCreate.Name} not saved");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { countryId = categoryToCreate.Id }, categoryToCreate);
        }


        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCategory([FromBody] Category categoryToUpdateInfo)
        {
            if (categoryToUpdateInfo == null)
            {
                return BadRequest(ModelState);
            }
;

            if (!_categoryRepository.IsCategoryExist(categoryToUpdateInfo.Id))
            {
                ModelState.AddModelError("", $"Category: {categoryToUpdateInfo.Name} not exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.UpdateCategory(categoryToUpdateInfo))
            {
                ModelState.AddModelError("", $"Somethink went wrong. {categoryToUpdateInfo.Name} not updated");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/countryId
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteAuthor(int categoryId)
        {
            if (!_categoryRepository.IsCategoryExist(categoryId))
            {
                return NotFound();

            }

            Category categoyToDelete = _categoryRepository.GetCategory(categoryId);

            ICollection<Book> books = _categoryRepository.GetBooksOfCategories(categoryId);


            if (books.Count() > 0)
            {
                ModelState.AddModelError("", "At first you have to delete this books: \n" + books.ToString() +
                                             $"Has not {categoyToDelete.Name} delted");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoyToDelete))
            {
                ModelState.AddModelError("", $"Somethink went wrong.Has not {categoyToDelete.Name} delted");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }

}

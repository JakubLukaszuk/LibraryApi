using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        LibraryDbContext _categoryContext;
        public CategoryRepository(LibraryDbContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        public ICollection<Category> GetCategories()
        {
            return _categoryContext.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public ICollection<Book> GetBooksOfCategories(int categoryId)
        {
            return _categoryContext.BookCategories.Where(bc => bc.CategoryId == categoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategoriesOfBook(int bookId)
        {
            return _categoryContext.BookCategories.Where(bc => bc.BookId == bookId).Select(c => c.Category).ToList();
        }

        public bool IsCategoryExist(int categoryId)
        {
            return _categoryContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool IsDuplicatedCategoryName(int categoryId, string categoryName)
        {
            var category = _categoryContext.Books.Where(c =>
                c.Isbn.Trim().ToUpper() == categoryName.Trim().ToUpper() && c.Id == categoryId).FirstOrDefault();

            return category == null ? false : true;
        }

        public bool CreateCategory(Category category)
        {
            _categoryContext.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _categoryContext.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _categoryContext.Remove(category);
            return Save();
        }

        public bool Save()
        {
            return _categoryContext.SaveChanges() >=0 ? true : false;
        }
    }
}

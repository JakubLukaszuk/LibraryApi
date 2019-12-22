using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetCategoriesOfBook(int bookId);
        ICollection<Book> GetBooksOfCategories(int categoryId);
        bool IsCategoryExist(int categoryId);
        bool IsDuplicatedCategoryName(int categoryId, string categoryName);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}

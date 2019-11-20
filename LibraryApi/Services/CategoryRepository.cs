using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    class CategoryRepository : ICategoryRepository
    {
        public ICollection<Book> GetBooksOfCategories(int categoryId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public ICollection<Category> GetCategoriesOfBook(int bookId)
        {
            throw new NotImplementedException();
        }

        public Category GetCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public bool IsCategoryExist(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}

using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category GetCategory(int categoryId);
        List<Category> GetCategoriesOfBook();
    }
}

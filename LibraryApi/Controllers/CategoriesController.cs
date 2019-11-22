using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    class CategoriesController : Controller
    {
        private CategoryRepository _categoriesRepository;

        public CategoriesController(CategoryRepository categoryRepository)
        {
            _categoriesRepository = categoryRepository;
        }
    }
}

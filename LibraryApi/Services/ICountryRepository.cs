﻿using System;
using System.Collections.Generic;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryOfAuthor(int authorId);
        ICollection<Author> GetAuthorsFromACountry(int countryId);
        bool IsCountryExists(int countryId);
    }
}
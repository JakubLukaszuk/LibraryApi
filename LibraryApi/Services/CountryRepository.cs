using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public class CountryRepository : ICountryRepository
    {
        private LibraryDbContext _contryContext;

        public CountryRepository(LibraryDbContext contryContext)
        {
            _contryContext = contryContext;
        }

        public bool CreateCountry(Country country)
        {
            _contryContext.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
             _contryContext.Remove(country);
            return Save();
        }

        public ICollection<Author> GetAuthorsFromACountry(int countryId)
        {
            return _contryContext.Authors.Where(c => c.Id == countryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _contryContext.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _contryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryOfAuthor(int authorId)
        {
            return _contryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public bool IsCountryExists(int countryId)
        {
            return _contryContext.Categories.Any(c => c.Id == countryId);
        }

        public bool isDuplicateCountryName(int countryId, string countryName)
        {
            var country = _contryContext.Countries.Where(c =>
                c.Name.Trim().ToUpper() == countryName.Trim().ToUpper() && c.Id == countryId).FirstOrDefault();

            return country == null ? false : true;
        }

        public bool Save()
        {
            int  saved = _contryContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _contryContext.Update(country);
            return Save();
        }
    }
}

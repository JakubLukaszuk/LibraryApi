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
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDataTransferObjects>))]
        public IActionResult GetCountries()
        {
            List<Country> countries = _countryRepository.GetCountries().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countiresDto = new List<CountryDataTransferObjects>();
            foreach (var country in countries)
            {
                countiresDto.
                    Add(new CountryDataTransferObjects
                    {
                        Id = country.Id,
                        Name =country.Name 
                    });
            }

            return Ok(countiresDto);
        }

        //api/countries/{countryId}
        [HttpGet("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDataTransferObjects))]
        public IActionResult GetCountry(int countryId)
        {
            if (_countryRepository.IsCountryExists(countryId))
                return NotFound();

            Country country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CountryDataTransferObjects countryDto = new CountryDataTransferObjects();

            countryDto.Id = country.Id;
            countryDto.Name = country.Name;
         

            return Ok(countryDto);
        }

        //api/countries/authors/{authorId}
        [HttpGet("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDataTransferObjects))]
        public IActionResult GetCountryOfAuthor(int authorId)
        {
            //To do - IsAuthorExist
             Country country = _countryRepository.GetCountryOfAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            CountryDataTransferObjects countryDto = new CountryDataTransferObjects();

            countryDto.Id = country.Id;
            countryDto.Name = country.Name;


            return Ok(countryDto);

        }
    }
}

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
        private IAuthorRepository _authorRepository;
        public CountriesController(ICountryRepository countryRepository, IAuthorRepository authorRepository)
        {
            _countryRepository = countryRepository;
            _authorRepository = authorRepository;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
        [HttpGet("{countryId}", Name = "GetCountry")]
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
        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDataTransferObjects))]
        public IActionResult GetCountryOfAuthor(int authorId)
        {
            if (!_authorRepository.IsAuthorExist(authorId))
            {
                return NotFound();
            }

             Country country = _countryRepository.GetCountryOfAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            CountryDataTransferObjects countryDto = new CountryDataTransferObjects();

            countryDto.Id = country.Id;
            countryDto.Name = country.Name;


            return Ok(countryDto);

        }

        //api/countries/{countryId}/authors
        [HttpGet("{countryId}/authors/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDataTransferObjects>))]
        public IActionResult GetAuthorsFromCountry(int countryId)
        {
            if (!_countryRepository.IsCountryExists(countryId))
            {
                return NotFound();
            }

            IEnumerable<Author> authors = _countryRepository.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AuthorDataTransferObjects> authorsDotsList = new List<AuthorDataTransferObjects>();
            
            foreach (var author in authors)
            {
                authorsDotsList.Add(
                    new AuthorDataTransferObjects
                    {
                        Id = author.Id,
                        FirstName = author.FirstName,
                        LastName = author.LastName
                    });
            }
            return Ok(authorsDotsList);
        }

        //api/countries
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody] Country countryToCreate)
        {
            if (countryToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries().Where(c =>
                c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"Country: {countryToCreate.Name} already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Somethink went wrong. {countryToCreate.Name} not saved");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new {countryId = countryToCreate.Id}, countryToCreate);
        }

        //api/countries/countryId
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCountry(int countryId, [FromBody] Country updatedCountryinfo)
        {
            if (updatedCountryinfo == null)
            {
                return BadRequest(ModelState);
            }

            if (countryId != updatedCountryinfo.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.IsCountryExists(countryId))
            {
                return NotFound();
            }

            if (_countryRepository.isDuplicateCountryName(countryId, updatedCountryinfo.Name))
            {
                ModelState.AddModelError("", $"Country: {updatedCountryinfo.Name} already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.UpdateCountry(updatedCountryinfo))
            {
                ModelState.AddModelError("", $"Somethink went wrong when updating: {updatedCountryinfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/countryId
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.IsCountryExists(countryId))
            {
                return NotFound();
            }

            Country countyToDelete = _countryRepository.GetCountry(countryId);

            ICollection<Author> authors = _countryRepository.GetAuthorsFromACountry(countryId);


            if (authors.Count() > 0)
            {
                ModelState.AddModelError("", "At first you have to delete this authors: \n"+authors.ToString()+
                                             $"Has not {countyToDelete.Name} delted");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countyToDelete))
            {
                ModelState.AddModelError("", $"Somethink went wrong.Has not {countyToDelete.Name} delted");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

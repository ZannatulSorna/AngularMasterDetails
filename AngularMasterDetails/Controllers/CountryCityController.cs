using AngularMasterDetails.Models;
using AngularMasterDetails.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularMasterDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryCityController : ControllerBase
    {
        private readonly ICountryCityRepo _repo;

        public CountryCityController(ICountryCityRepo repo)
        {
            _repo = repo;
        }
        [HttpGet("GetAllCountriesWithCities")]
        public async Task<IActionResult> GetAllCountriesWithCities()
        {
            try
            {
                var countries = await _repo.GetAllCountries();

                var countriesWithCities = new List<CountryWithCities>();
                foreach (var country in countries)
                {
                    var cities = await _repo.GetCitiesByCountryId(country.Id);
                    countriesWithCities.Add(new CountryWithCities { Country = country, Cities = cities.ToList() });

                }
                return Ok(countriesWithCities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Getting country and cities:{ex.Message}");
            }
        }

        [HttpGet("GetCountryWithCitiesById/{countryId}")]
        public async Task<IActionResult> GetCountryWithCitiesById(int countryId)
        {
            try
            {
                var country = await _repo.GetCountry(countryId);
                if (country == null)

                    return NotFound("No Country Found");
                var cities = await _repo.GetCitiesByCountryId(countryId);
                return Ok(new { Country = country, Cities = cities });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Geting country and cities:{ex.Message}");
            }
        }
        [HttpPost("AddCountryAndCity")]
        public async Task<IActionResult> AddCountryAndCity(CountryWithCities countryWithCities)
        {
            try
            {
                var addCountry = await _repo.AddCountry(countryWithCities.Country);
                foreach (var city in countryWithCities.Cities)
                {
                    city.CountryId = addCountry.Id;
                    await _repo.AddCity(city);
                }
                return Ok(new { Country = addCountry, cities = countryWithCities.Cities });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Adding country and cities:{ex.Message}");
            }
        }
        [HttpPut("PutCountryAndCities/{countryId}")]
        public async Task<IActionResult> PutCountryAndCities(int countryId, Country obj)
        {
            try
            {
                if (countryId == 0)

                    return BadRequest("Country Id Must be Provided");
                var existingCountry = await _repo.GetCountry(obj.Id);
                if (existingCountry == null)
                    return NotFound("Country Not Found");
                existingCountry.Name = obj.Name;
                existingCountry.ISO2 = obj.ISO2;
                existingCountry.ISO3 = obj.ISO3;
                await _repo.UpdateCountry(existingCountry);

                var updatedCities = new List<City>();
                foreach (var city in obj.Cities)
                {
                    if (city.Id == 0)
                    {
                        city.CountryId = existingCountry.Id;
                        var addedCity = await _repo.AddCity(city);
                        updatedCities.Add(city);
                    }
                    else
                    {
                        var existingCity = await _repo.GetCityById(city.Id);
                        if (existingCity == null || existingCity.CountryId != existingCity.Id)
                            return NotFound("City not found");
                        existingCity.Name = city.Name;
                        existingCity.Lat = city.Lat;
                        existingCity.Lan = city.Lan;
                        await _repo.UpdateCity(existingCity);
                        updatedCities.Add(existingCity);
                    }
                    var existingCities = await _repo.GetCitiesByCountryId(existingCountry.Id);
                    var deletedCities = existingCities.Where(ec => !updatedCities.Any(uc => uc.Id == ec.Id)).ToList();
                    foreach (var deleteCities in deletedCities)
                    {
                        await _repo.DeleteCity(deleteCities.Id);
                    }

                }
                return Ok(new { Country = existingCountry, Cities = updatedCities });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Updating country and cities:{ex.Message}");
            }
        }
        [HttpDelete("DeleteCountryAndCities/{countryId}")]
        public async Task<IActionResult> DeleteCountryAndCities(int countryId)
        {
            try
            {
                var existingCountry = await _repo.GetCountry(countryId);
                if (existingCountry == null)
                    return NotFound("Country Not found");
                var cities=await _repo.GetCitiesByCountryId(countryId);
                foreach (var city in cities)
                {
                    await _repo.DeleteCity(city.Id);
                }
                await _repo.DeleteCountry(countryId);
                return Ok(new { message = "Country and Cities were deleted Successfully" });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Deleting country and cities:{ex.Message}");
            }
        }
    }
}

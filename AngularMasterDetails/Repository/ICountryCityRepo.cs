using AngularMasterDetails.Models;

namespace AngularMasterDetails.Repository
{
    public interface ICountryCityRepo
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<Country> GetCountry(int id);
        Task<Country> AddCountry(Country country);
        Task DeleteCountry(int id);
        Task<IEnumerable<City>> GetCitiesByCountryId(int countryId);
        Task<City> GetCityById(int id);
        Task<City> AddCity(City city);
        Task UpdateCity(City city);
        Task DeleteCity(int id);
        Task UpdateCountry(Country country);

    }
}

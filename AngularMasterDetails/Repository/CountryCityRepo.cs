using AngularMasterDetails.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularMasterDetails.Repository
{
    public class CountryCityRepo : ICountryCityRepo
    {
        private readonly AppDbContext _db;

        public CountryCityRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<City> AddCity(City city)
        {
            _db.Cities.Add(city);
            await _db.SaveChangesAsync();
            return city;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task DeleteCity(int id)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city != null)
            {
                _db.Cities.Remove(city);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteCountry(int id)
        {
            var country = await _db.Countries.FindAsync(id);
            if (country != null)
            {
                _db.Countries.Remove(country);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesByCountryId(int countryId)
        {
            return await _db.Cities.Where(c=>c.CountryId==countryId).ToListAsync();
        }

        public async Task<City> GetCityById(int id)
        {
            return await _db.Cities.FindAsync(id);
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _db.Countries.FindAsync(id);
        }

        public async Task UpdateCity(City city)
        {
             _db.Entry(city).State=EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCountry(Country country)
        {
            _db.Entry(country).State=EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}

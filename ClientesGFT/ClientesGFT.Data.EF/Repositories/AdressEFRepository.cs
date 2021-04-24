using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.Data.EF.Repositories
{
    public class AdressEFRepository : IAdressRepository
    {
        private readonly ClientesGFTContext _context;

        public AdressEFRepository(ClientesGFTContext context)
        {
            _context = context;
        }

        public IList<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public IList<State> GetStates(int countryId)
        {
            var states = _context.States.Where(x => x.CountryId == countryId).Include(x => x.Country);

            return states.ToList();
        }

        public IList<City> GetCities(int stateId)
        {
            var cities = _context.Cities.Where(x => x.StateId == stateId).Include(x => x.State).ThenInclude(x => x.Country);

            return cities.ToList();
        }
    }
}

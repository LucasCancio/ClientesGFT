using ClientesGFT.Domain.Entities.AdressEntities;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Repositories
{
    public interface IAdressRepository
    {
        IList<Country> GetCountries();
        IList<State> GetStates(int countryId);
        IList<City> GetCities(int stateId);
    }
}

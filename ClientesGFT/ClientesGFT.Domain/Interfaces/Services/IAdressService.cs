using ClientesGFT.Domain.Entities.AdressEntities;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Interfaces.Services
{
    public interface IAdressService
    {
        IList<Country> GetCountries();
        IList<State> GetStates(int countryId);
        IList<City> GetCities(int stateId);
    }
}

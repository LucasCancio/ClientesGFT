using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Services
{
    public class AdressService : IAdressService
    {
        private readonly IAdressRepository _adressRepository;

        public AdressService(IAdressRepository adressRepository)
        {
            _adressRepository = adressRepository;
        }

        public IList<Country> GetCountries()
        {
            return _adressRepository.GetCountries();
        }

        public IList<City> GetCities(int stateId)
        {
            return _adressRepository.GetCities(stateId);
        }

        public IList<State> GetStates(int countryId)
        {
            return _adressRepository.GetStates(countryId);
        }
    }
}

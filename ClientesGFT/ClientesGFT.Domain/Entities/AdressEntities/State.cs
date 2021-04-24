using System.Collections.Generic;

namespace ClientesGFT.Domain.Entities.AdressEntities
{
    public class State
    {
        internal State(int id)
        {
            Id = id;
        }
        public State(int id, string description, Country country)
            : this(description, country)
        {
            Id = id;
        }

        public State(string description, Country country)
        {
            Description = description;
            Country = country;
            if (country != null) CountryId = country.Id;
        }

        public State(int id, string description, int countryId)
        {
            Id = id;
            Description = description;
            Country = new Country(countryId);
            CountryId = countryId;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}

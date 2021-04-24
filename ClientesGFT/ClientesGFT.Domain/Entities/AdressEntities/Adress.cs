using ClientesGFT.Domain.Util;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Entities.AdressEntities
{
    public class Adress
    {
        private Adress() { }
        public Adress(int id, City city, string street, string district,
            int number, string complement = null, string cep = null)
            : this(city,street,district,number,complement,cep)
        {
            Id = id;
        }
        public Adress(City city, string street, string district, 
            int number, string complement = null, string cep = null)
        {
            Cep = DocumentFixer.Fix(cep);
            City = city;
            CityId = city?.Id;
            Street = street;
            District = district;
            Number = number;
            Complement = complement;
        }

        public int Id { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }


        public City City { get; set; }
        public int? CityId { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
    }
}

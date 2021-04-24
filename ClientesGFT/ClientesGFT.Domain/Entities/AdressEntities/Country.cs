using System.Collections.Generic;

namespace ClientesGFT.Domain.Entities.AdressEntities
{
    public class Country
    {
        public Country(int id, string description)
        {
            Id = id;
            Description = description;
        }
        public Country(int id, string description, string initials)
            : this(description, initials)
        {
            Id = id;
        }
        public Country(string description, string initials)
        {
            Description = description;
            Initials = initials;
        }
        internal Country(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Initials { get; private set; }

        public virtual ICollection<State> States { get; set; }
    }
}

using System.Collections.Generic;

namespace ClientesGFT.Domain.Entities.AdressEntities
{
    public class City
    {
        public City(int id)
        {
            Id = id;
        }

        public City(int id, string description, State state)
            : this(description, state)
        {
            Id = id;
        }

        public City(string description, State state)
        {
            Description = description;
            State = state;
            if (state != null) StateId = state.Id;
        }

        public City(int id, string description, int stateId)
        {
            Id = id;
            Description = description;
            State = new State(stateId);
            StateId = stateId;
        }



        public int Id { get; private set; }
        public string Description { get; private set; }

        public int StateId { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
    }
}

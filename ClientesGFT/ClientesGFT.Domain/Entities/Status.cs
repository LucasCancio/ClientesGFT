using ClientesGFT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientesGFT.Domain.Entities
{
    public class Status
    {
        public Status(int id,EStatus description) : this(description)
        {
            Id = id;
        }
        public Status(EStatus description) : this()
        {
            Description = description;
        }
        public Status()
        {
            Clients = new HashSet<Client>();
            Fluxos = new HashSet<Fluxo>();
        }



        public int Id { get; private set; }
        public EStatus Description { get; private set; }

        public virtual ICollection<Client> Clients { get; private set; }
        public virtual ICollection<Fluxo> Fluxos { get; private set; }
    }
}

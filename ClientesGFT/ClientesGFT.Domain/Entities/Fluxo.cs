using ClientesGFT.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClientesGFT.Domain.Entities
{
    public class Fluxo
    {
        public Fluxo(Client client, User user, EStatus status, DateTime createDate)
        {
            this.Client = client;
            this.User = user;
            this.CreateDate = createDate;
            this.Status = new Status(status);
        }

        public Fluxo(int clientId, int userId, int statusId)
        {
            this.ClientId = clientId;
            this.UserId = userId;
            this.StatusId = statusId;
            this.CreateDate = DateTime.Now;
        }

        public Fluxo(Client client, User user, int statusId)
        {
            this.Client = client;
            this.User = user;
            this.StatusId = statusId;
            this.CreateDate = DateTime.Now;
        }

        private Fluxo() {}

        public int Id { get; private set; }
        public int ClientId { get; private set; }
        public virtual Client Client { get; private set; }

        public int StatusId { get; private set; }
        public virtual Status Status { get; private set; }

        public int UserId { get; private set; }
        public virtual User User { get; private set; }

        [Display(Name = "Data de Criação")]
        public DateTime CreateDate { get; private set; }
    }
}

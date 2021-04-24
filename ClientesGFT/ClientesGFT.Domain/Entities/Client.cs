using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Util;
using System;
using System.Collections.Generic;

namespace ClientesGFT.Domain.Entities
{
    public class Client
    {
        public Client(int id, string name, string cpf, string rg, DateTime birthDate, string email, Status currentStatus,
            Adress adress, List<Phone> phones = null, DateTime? modifiedDate = null, DateTime? createdDate = null)
            : this(name, cpf, rg, birthDate, email, adress, phones, modifiedDate)
        {
            Id = id;
            CurrentStatus = currentStatus;
            if (CurrentStatus != null)  CurrentStatusId = CurrentStatus.Id;
            CreatedDate = createdDate.GetValueOrDefault();
        }

        public Client(string name, string cpf, string rg, DateTime birthDate, string email, Adress adress, 
            List<Phone> phones = null, DateTime? modifiedDate = null) : this()
        {
            Name = name;
            CPF = DocumentFixer.Fix(cpf);
            RG = DocumentFixer.Fix(rg);
            BirthDate = birthDate;
            Email = email;

            if (CurrentStatus == null)
            {
                int idStatusEmCadastro = EnumHelper.StatusParaStatusId(EStatus.EM_CADASTRO);
                CurrentStatus = new Status(idStatusEmCadastro, EStatus.EM_CADASTRO);
                CurrentStatusId = idStatusEmCadastro;
            }

            Adress = adress;
            AdressId = adress?.Id;

            if (phones != null) Phones = phones;

            CreatedDate = DateTime.Now;

            if (modifiedDate.HasValue && modifiedDate.Value != DateTime.MinValue)
                ModifiedDate = modifiedDate.Value;
            else
                ModifiedDate = DateTime.Now;

            SetIsInternacional();
        }

        private Client()
        {
            Phones = new HashSet<Phone>();
            Fluxos = new HashSet<Fluxo>();
            IsEnableToModify = true;
        }


        public int Id { get; private set; }
        
        public string Name { get; private set; }
        public string CPF { get; private set; }
        public string RG { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Email { get; private set; }

        public DateTime ModifiedDate { get; private set; }
        public DateTime CreatedDate { get; private set; }


        public bool IsInternacional { get; private set; }
        public bool IsEnableToModify { get; private set; }





        public int? AdressId { get; private set; }
        public virtual Adress Adress { get; private set; }
        public int CurrentStatusId { get; private set; }
        public Status CurrentStatus { get; private set; }
        public virtual ICollection<Phone> Phones { get; private set; }
        public virtual ICollection<Fluxo> Fluxos { get; private set; }


        public void AddPhone(Phone phone)
        {
            if (!Phones.Contains(phone))
                this.Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            this.Phones.Remove(phone);
        }

        public void SetCurrentStatus(EStatus status)
        {
            int idStatus = EnumHelper.StatusParaStatusId(status);

            this.CurrentStatusId = idStatus;
            //this.CurrentStatus = null;
            this.ModifiedDate = DateTime.Now;
        }

        public void DisableToModify()
        {
            this.IsEnableToModify = false;
        }

        public void SetIsInternacional()
        {
            if (Adress == null)
                throw new ArgumentException("Endereço não definido.");

            IsInternacional = Adress?.City?.State?.Country?.Description?.ToLower() != "brasil";
        }
    }
}

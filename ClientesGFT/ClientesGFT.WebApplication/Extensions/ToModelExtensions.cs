using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Util;
using ClientesGFT.WebApplication.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ClientesGFT.WebApplication.Extensions
{
    public static class ToModelExtensions
    {
        public static Client ToModel(this ClientViewModel client)
        {
            var country = new Country(client.CountryId.Value, client.Country);
            var state = new State(client.StateId.Value, client.State, country);
            var city = new City(client.CityId.Value, client.City, state);

            var adress = new Adress(
                city,
                client.Street,
                client.District,
                client.Number,
                client.Complement,
                client.Cep
            );

            Status status = null;
            if (client.CurrentStatus != EStatus.TODOS)
            {
                int idStatus = EnumHelper.StatusParaStatusId(client.CurrentStatus);
                status = new Status(idStatus, client.CurrentStatus);
            }

            return new Client(
                id: client.Id,
                birthDate: client.BirthDate.GetValueOrDefault(),
                cpf: client.CPF,
                email: client.Email,
                name: client.Name,
                rg: client.RG,
                currentStatus: status,
                adress: adress,
                phones: client.Phones,
                modifiedDate: client.ModifiedDate
           );
        }

        public static User ToModel(this UserViewModel user)
        {
            var userRoles = user.Roles.Select(role => new UserRole(user.Id, role.Id)).ToList();

            return new User(user.Id, user.Name, user.Login, user.Password, user.CreatedDate, userRoles);
        }
    }
}

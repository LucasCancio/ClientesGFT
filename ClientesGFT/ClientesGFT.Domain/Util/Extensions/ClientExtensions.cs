using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Entities.AdressEntities;
using System;
using System.Linq;

namespace ClientesGFT.Domain.Util.Extensions
{
    public static class ClientExtensions
    {
        public static Client FixIncompleteClient(this Client client, Client dbClient)
        {
            City city = client.Adress.City ?? dbClient.Adress.City;

            var adress = new Adress(dbClient.AdressId.GetValueOrDefault(), 
                                    city,
                                    client.Adress.Street,
                                    client.Adress.District,
                                    client.Adress.Number,
                                    client.Adress.Complement,
                                    client.Adress.Cep);

            return new Client(dbClient.Id,
                              client.Name, 
                              client.CPF, 
                              client.RG, 
                              client.BirthDate, 
                              client.Email,
                              dbClient.CurrentStatus, 
                              adress,
                              client.Phones.ToList(), 
                              createdDate: dbClient.CreatedDate);
        }
    }
}

using ClientesGFT.Domain.Entities;
using ClientesGFT.WebApplication.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ClientesGFT.WebApplication.Extensions
{
    public static class ToViewModelExtensions
    {
        public static ClientViewModel ToViewModel(this Client client)
        {
            var state = client.Adress.City.State;
            var country = state.Country;

            return new ClientViewModel()
            {
                Id = client.Id,
                BirthDate = client.BirthDate,
                CPF = client.CPF,
                CurrentStatus = client.CurrentStatus.Description,
                Email = client.Email,
                IsInternational = client.IsInternacional,
                Name = client.Name,
                Phones = client.Phones.ToList(),
                RG = client.RG,


                Cep = client.Adress.Cep,
                City = client.Adress.City.Description,
                CityId = client.Adress.City.Id,
                Complement = client.Adress.Complement,
                Country = country.Description,
                CountryId = country.Id,
                District = client.Adress.District,
                Number = client.Adress.Number,
                State = state.Description,
                StateId = state.Id,
                Street = client.Adress.Street,

                ModifiedDate = client.ModifiedDate,
                IsEnableToModify = client.IsEnableToModify
            };
        }

        public static IList<ClientViewModel> ToViewModel(this IList<Client> clients)
        {
            return clients
                        .Select(client => client.ToViewModel())
                        .ToList();
        }

        public static UserViewModel ToViewModel(this User user)
        {
            return new UserViewModel()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                CreatedDate = user.CreatedDate,
                IsEnableToModify = user.IsEnableToModify,
                Roles = user.UserRoles.Select(ur=>ur.Role).ToList()
            };
        }

        public static IList<UserViewModel> ToViewModel(this IList<User> users)
        {
            return users
                        .Select(user => user.ToViewModel())
                        .ToList();
        }
    }
}

using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Util.Extensions;

namespace ClientesGFT.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }


        public User Login(string login, string senha)
        {
            try
            {
                var user = _userRepository.GetByCredentials(login, senha);

                if (user == null)
                    throw new InvalidLoginException();

                user.ClearPassword();

                return user;
            }
            catch (InvalidLoginException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao realizar login: {ex.Message}", ex);
            }
        }

        public IList<User> GetAll(User loggedUser)
        {
            var users = _userRepository
                                        .GetAll()
                                        .Where(user => user.Id != loggedUser.Id)
                                        .ToList();

            users.ForEach(user =>
            {
                if (user.Roles.Contains(ERoles.ADMINISTRACAO))
                    user.DisableToModify();
            });

            return users;
        }

        public List<Role> FixRoles(List<int> roleIds)
        {
            return roleIds.Select(id => new Role(id)).ToList();
        }

        #region Crud

        public User Get(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);

                if (user == null)
                    throw new InvalidLoginException();

                return user;
            }
            catch (InvalidLoginException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao realizar login: {ex.Message}", ex);
            }

        }

        public User Get(ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Get(Convert.ToInt32(userId));
        }

        public void Register(User user)
        {
            if (string.IsNullOrEmpty(user.Password?.Trim()))
                throw new InvalidUserException("A senha é obrigatória.");

            _userRepository.Insert(user);
        }

        public void Edit(User user)
        {
            var userInDb = _userRepository.GetById(user.Id);

            var fixedUser = user.FixIncompleteUser(userInDb, out bool passwordChanged);

            _userRepository.Update(fixedUser, passwordChanged);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user.Id);
        }

        #endregion
        public List<Role> GetRoles()
        {
            var roles = _roleRepository.GetAll().Where(r => r.Description != ERoles.ADMINISTRACAO);

            return roles.ToList();
        }
    }
}

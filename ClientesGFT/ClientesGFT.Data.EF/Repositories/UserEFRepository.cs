using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ClientesGFT.Data.EF.Repositories
{
    public class UserEFRepository : IUserRepository
    {
        private readonly ClientesGFTContext _context;
        private readonly IRoleRepository _roleRepository;

        public UserEFRepository(ClientesGFTContext context, IRoleRepository roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }



        public IList<User> GetAll()
        {
            var users = _context.Users.Include(u => u.UserRoles)
                                      .ThenInclude(ur => ur.Role)
                                      .Where(u => u.Ativo.Value)
                                      .AsNoTracking()
                                      .ToList();

            users.ForEach(user => user.FixRoles());

            return users;
        }

        public User GetByCredentials(string login, string password)
        {
            var user = _context.Users.FromSqlRaw("[dbo].[SP_Logar] @Login, @Senha",
                                                            new SqlParameter("Login", login),
                                                            new SqlParameter("Senha", password)).AsNoTracking().AsEnumerable().FirstOrDefault();

            var roles = _roleRepository.GetById(user.Id);

            user.Roles = roles;

            return user;
        }

        public User GetById(int id)
        {
            var user = _context.Users
                                    .Include(u=>u.UserRoles)
                                    .ThenInclude(ur=>ur.Role)
                                    .AsNoTracking()
                                    .FirstOrDefault(x => x.Id == id);

            user.FixRoles();

            return user;
        }

        private string ToMd5(string text)
        {
            var textParam = new SqlParameter("Text", text);
            var md5TextParam = new SqlParameter("Md5Text", SqlDbType.VarChar, 255);
            md5TextParam.Direction = ParameterDirection.Output;

            _context.Database.ExecuteSqlRaw("[dbo].[SP_TransformarEmMD5] @Text, @Md5Text OUT", parameters: new object[] { textParam, md5TextParam });
            return Convert.ToString(md5TextParam.Value);
        }

        private void EditRoles(User user, List<UserRole> roles)
        {
            var rolesInDb = _context.UserRoles.Where(p => p.UserId == user.Id).ToList();

            var rolesToDelete = rolesInDb.Where(ur => !roles.Contains(ur)).ToList();
            var rolesToInsert = roles.Where(ur => !rolesInDb.Contains(ur)).ToList();

            //rolesToInsert.ForEach(p => p.userid = client.Id);

            _context.UserRoles.RemoveRange(rolesToDelete);
            _context.UserRoles.AddRange(rolesToInsert);
        }

        public void Insert(User user)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var hashedPassword = ToMd5(user.Password);
                var userToInsert = new User(user.Name, user.Login, hashedPassword, user.UserRoles);

                _context.Entry(userToInsert).State = EntityState.Added;

                _context.UserRoles.AddRange(userToInsert.UserRoles);

                _context.SaveChanges();

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                if (ex.Number > 50000)
                    throw new InvalidUserException(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void Update(User user, bool passwordChanged)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var hashedPassword = passwordChanged ? ToMd5(user.Password.Trim()) : user.Password;
                var userToUpdate = new User(user.Id, user.Name, user.Login, hashedPassword, user.CreatedDate, user.UserRoles);

                _context.Entry(userToUpdate).State = EntityState.Modified;

                EditRoles(userToUpdate, userToUpdate.UserRoles.ToList());

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                if (ex.Number > 50000)
                    throw new InvalidUserException(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void Delete(int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                user.Disable();

                _context.Entry(user).State = EntityState.Modified;

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }
}

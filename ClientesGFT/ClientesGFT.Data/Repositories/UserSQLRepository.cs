using ClientesGFT.Data.Contexts;
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ClientesGFT.Data.Repositories
{
    public class UserSQLRepository : IUserRepository
    {
        private readonly IRoleRepository _perfilRepository;
        public UserSQLRepository(IRoleRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }
        public IList<User> GetAll()
        {
            var users = new List<User>();
            var dbContext = new SQLDbContext();

            var SQL = @$"SELECT * FROM VW_Usuarios ORDER BY Id DESC";

            DataTable dtresult = dbContext.ExecutarConsulta(SQL);

            foreach (DataRow dataRow in dtresult.Rows)
            {
                var user = new User(
                         Convert.ToInt32(dataRow["Id"]),
                         dataRow["Nome"].ToString(),
                         dataRow["Login"].ToString(),
                          dataRow["Senha"].ToString(),
                         Convert.ToDateTime(dataRow["DataCadastro"])
                 );

                users.Add(user);
            }

            return users;
        }

        public User GetById(int id)
        {
            User user = null;
            var dbContext = new SQLDbContext();

            string SQL = @"SELECT * FROM VW_Usuarios WHERE Id = @Id";

            var parametros = new SqlParameter[] {
                new SqlParameter("@Id",id)
            };

            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);
            if (dtResult.Rows.Count > 0)
            {
                DataRow dataRow = dtResult.Rows[0];

                user = new User(
                        Convert.ToInt32(dataRow["Id"]),
                        dataRow["Nome"].ToString(),
                        dataRow["Login"].ToString(),
                        dataRow["Senha"].ToString(),
                        Convert.ToDateTime(dataRow["DataCadastro"])
               );

                var roles = _perfilRepository.GetById(user.Id);
                user.Roles = roles;
            }

            return user;
        }

        public User GetByCredentials(string login, string password)
        {
            try
            {
                User user = null;
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                new SqlParameter("@Login",login),
                new SqlParameter("@Senha",password),
                };

                DataTable dtResult = dbContext.ExecutarProcedure("SP_Logar", parametros);

                if (dtResult.Rows.Count > 0)
                {
                    DataRow dataRow = dtResult.Rows[0];

                    user = new User(
                            id: Convert.ToInt32(dataRow["Id"]),
                            name: dataRow["Nome"].ToString(),
                            login,
                            password,
                            Convert.ToDateTime(dataRow["DataCadastro"])
                      );

                    var roles = _perfilRepository.GetById(user.Id);
                    user.Roles = roles;


                }

                return user;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    throw new InvalidLoginException(ex.Message, ex);
                }

                throw ex;

            }

        }

        public void Insert(User user)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                    new SqlParameter("@Nome",user.Name),
                    new SqlParameter("@Login",user.Login),
                    new SqlParameter("@Senha",user.Password),
                };

                dbContext.ExecutarProcedure("SP_InserirUsuario", parametros);
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    throw new InvalidClientException(ex.Message);
                throw ex;
            }
        }

        public void Update(User user, bool passwordChanged)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                    new SqlParameter("@Id",user.Id),
                    new SqlParameter("@Nome",user.Name),
                    new SqlParameter("@Login",user.Login),
                    new SqlParameter("@Senha",user.Password),
                };

                dbContext.ExecutarProcedure("SP_AtualizarUsuario", parametros);
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    throw new InvalidClientException(ex.Message);
                throw ex;
            }
        }

        public void Delete(int userId)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                     new SqlParameter("@Id", userId),
                };

                dbContext.ExecutarProcedure("SP_DeletarUsuario", parametros);
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    throw new InvalidClientException(ex.Message);
                throw ex;
            }
        }
    }
}

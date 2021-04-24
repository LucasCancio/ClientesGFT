using ClientesGFT.Data.Contexts;
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Enums;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ClientesGFT.Data.Repositories
{
    public class RoleSQLRepository : IRoleRepository
    {
        public ERoles[] GetById(int userId)
        {
            List<ERoles> roles = new List<ERoles>();

            var dbContext = new SQLDbContext();

            string SQL = @"SELECT * FROM VW_Usuarios_Perfils WHERE IdUsuario = @IdUsuario";

            var parametros = new SqlParameter[] {
                new SqlParameter("@IdUsuario", userId)
            };

            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);
            foreach (DataRow dataRow in dtResult.Rows)
            {
                int perfilId = Convert.ToInt32(dataRow["IdPerfil"]);

                ERoles role = EnumHelper.PerfilIdParaPerfil(perfilId);

                roles.Add(role);
            }


            return roles.ToArray();
        }

        public void AplicarPerfil(int usuarioId, int perfilId)
        {
            var dbContext = new SQLDbContext();

            var parametros = new SqlParameter[] {
                new SqlParameter("@IdUsuario", usuarioId),
                new SqlParameter("@IdPerfil",perfilId)
            };

            dbContext.ExecutarProcedure("SP_AplicarPerfil", parametros);
        }


        public void RetirarPerfil(int usuarioId, int perfilId)
        {
            var dbContext = new SQLDbContext();

            var parametros = new SqlParameter[] {
                new SqlParameter("@IdUsuario", usuarioId),
                new SqlParameter("@IdPerfil",perfilId)
            };

            dbContext.ExecutarProcedure("SP_RetirarPerfil", parametros);
        }

        public List<Role> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

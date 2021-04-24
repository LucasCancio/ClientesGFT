using ClientesGFT.Data.Contexts;
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Util;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ClientesGFT.Data.Repositories
{
    public class FluxoSQLRepository : IFluxoRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;

        public FluxoSQLRepository(IUserRepository userRepository, IClientRepository clientRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
        }

        public IList<Fluxo> ListarFluxo(DateTime startDate, DateTime endDate, int statusId = 0, string cpf = "", string name = "")
        {
            var fluxos = new List<Fluxo>();

            var dbContext = new SQLDbContext();

            SqlParameter[] parametros = new SqlParameter[] {
                    new SqlParameter("@StartDate", startDate.ToShortDateString()),
                    new SqlParameter("@EndDate", endDate.AddDays(1).ToShortDateString()),
                    new SqlParameter("@IdStatus", statusId),
                    new SqlParameter("@CPF", cpf?? ""),
                    new SqlParameter("@Name", name?? ""),
            };

            string SQL = "SELECT * FROM VW_Fluxo_Aprovacao WHERE DataCriacao >= @StartDate AND DataCriacao <= @EndDate";

            if (statusId > 0) SQL += " AND IdStatus = @IdStatus";

            if (!string.IsNullOrEmpty(cpf)) SQL += " AND CPF = @CPF";

            if (!string.IsNullOrEmpty(name)) SQL += " AND Cliente LIKE '%@Name%'";

            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);
            foreach (DataRow dataRow in dtResult.Rows)
            {
                var userId = Convert.ToInt32(dataRow["IdUsuario"]);
                var user = _userRepository.GetById(userId);

                var clientId = Convert.ToInt32(dataRow["IdCliente"]);
                var client = _clientRepository.GetById(clientId);

                var fluxo = new Fluxo(
                    client, 
                    user,
                    status: EnumHelper.StatusIdParaStatus(dataRow["IdStatus"].ToString()),
                    createDate: Convert.ToDateTime(dataRow["DataCriacao"])
                );

                fluxos.Add(fluxo);
            }

            return fluxos;
        }

        public IList<int> ListarIdsClientesIndisponiveis(int userId)
        {
            var ids = new List<int>();

            var dbContext = new SQLDbContext();

            SqlParameter[] parametros = new SqlParameter[] {
                    new SqlParameter("@IdUsuario", userId)
            };

            string SQL = @"SELECT * FROM FC_Listar_IdsClientes_Indisponiveis(@IdUsuario)";


            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);
            foreach (DataRow dataRow in dtResult.Rows)
            {
                ids.Add(Convert.ToInt32(dataRow["IdCliente"]));
            }

            return ids;
        }

        public void AprovarCliente(int clienteId, int userId)
        {
            ExecutarProcedureDoFluxo("SP_AprovarCliente", clienteId, userId);
        }

        public void EnviarParaAprovacao(int clienteId, int userId)
        {
            ExecutarProcedureDoFluxo("SP_EnviarParaAprovacao", clienteId, userId);
        }

        public void EnviarParaCorrecao(int clienteId, int userId)
        {
            ExecutarProcedureDoFluxo("SP_EnviarParaCorrecao", clienteId, userId);
        }

        public void EnviarParaGerencia(int clienteId, int userId)
        {
            ExecutarProcedureDoFluxo("SP_EnviarParaGerencia", clienteId, userId);
        }

        public void ReprovarCliente(int clienteId, int userId)
        {
            ExecutarProcedureDoFluxo("SP_ReprovarCliente", clienteId, userId);
        }

        private void ExecutarProcedureDoFluxo(string nomeProcedure, int clienteId, int userId)
        {
            var dbContext = new SQLDbContext();

            var parametros = new SqlParameter[] {
                new SqlParameter("@IdCliente", clienteId),
                new SqlParameter("@IdUsuarioResponsavel", userId),
            };

            dbContext.ExecutarProcedure(nomeProcedure, parametros);
        }



        public bool VerifyIfUserInFluxo(int clientId, int userId)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                new SqlParameter("@IdCliente", clientId),
                new SqlParameter("@IdUsuario", userId)
                };

                dbContext.ExecutarProcedure("SP_VerificarSeUsuarioEstaEnvolvidoNoFluxo", parametros);

                return false;
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    return true;
                throw ex;
            }
        }
    }
}

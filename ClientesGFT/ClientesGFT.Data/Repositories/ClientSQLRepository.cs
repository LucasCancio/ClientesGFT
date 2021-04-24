using ClientesGFT.Data.Contexts;
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Exceptions;
using ClientesGFT.Domain.Interfaces.Repositories;
using ClientesGFT.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ClientesGFT.Data.Repositories
{
    public class ClientSQLRepository : IClientRepository
    {
        public IList<Client> GetAllByStatus(IList<int> idsStatus)
        {
            var clientes = new List<Client>();
            var dbContext = new SQLDbContext();

            var SQL = @$"SELECT * FROM VW_Clientes 
                         WHERE IdStatusAtual IN ({String.Join(',', idsStatus)})
                         ORDER BY Id DESC";

            DataTable dtresult = dbContext.ExecutarConsulta(SQL);

            foreach (DataRow dataRow in dtresult.Rows)
            {
                var country = new Country(Convert.ToInt32(dataRow["IdPais"]), dataRow["Pais"].ToString(), dataRow["Sigla"].ToString());
                var state = new State(Convert.ToInt32(dataRow["IdEstado"]), dataRow["Estado"].ToString(), country);
                var city = new City(Convert.ToInt32(dataRow["IdCidade"]), dataRow["Cidade"].ToString(), state);

                var endereco = new Adress(
                         city,
                         dataRow["Rua"].ToString(),
                         dataRow["Bairro"].ToString(),
                         Convert.ToInt32(dataRow["Numero"]),
                         dataRow["Complemento"].ToString(),
                         dataRow["Cep"].ToString()
                  );

                var statusEnum = EnumHelper.StatusIdParaStatus(dataRow["IdStatusAtual"].ToString());

                var cliente = new Client(
                         Convert.ToInt32(dataRow["Id"]),
                         dataRow["Nome"].ToString(),
                         dataRow["CPF"].ToString(),
                         dataRow["RG"].ToString(),
                         Convert.ToDateTime(dataRow["DataNasc"]),
                         dataRow["Email"].ToString(),
                         new Status(statusEnum),
                         endereco,
                         modifiedDate: Convert.ToDateTime(dataRow["DataAlteracao"])
                 );

                clientes.Add(cliente);
            }

            return clientes;
        }

        public Client GetById(int id, bool withPhones = false)
        {
            Client cliente = null;

            var dbContext = new SQLDbContext();

            string SQL = @"SELECT * FROM VW_Clientes WHERE Id = @Id";

            var parametros = new SqlParameter[] {
                new SqlParameter("@Id",id)
            };

            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);

            if (dtResult.Rows.Count > 0)
            {

                DataRow dataRow = dtResult.Rows[0];

                var country = new Country(Convert.ToInt32(dataRow["IdPais"]), dataRow["Pais"].ToString(), dataRow["Sigla"].ToString());
                var state = new State(Convert.ToInt32(dataRow["IdEstado"]), dataRow["Estado"].ToString(), country);
                var city = new City(Convert.ToInt32(dataRow["IdCidade"]), dataRow["Cidade"].ToString(), state);

                var endereco = new Adress(
                         city,
                         dataRow["Rua"].ToString(),
                         dataRow["Bairro"].ToString(),
                         Convert.ToInt32(dataRow["Numero"]),
                         dataRow["Complemento"].ToString(),
                         dataRow["Cep"].ToString()
                  );

                var statusEnum = EnumHelper.StatusIdParaStatus(dataRow["IdStatusAtual"].ToString());

                var phones = new List<Phone>();

                if (withPhones)
                {
                    SQL = "SELECT Id, Telefone FROM Clientes_Telefones WHERE IdCliente = @IdCliente";
                    parametros = new SqlParameter[] {
                        new SqlParameter("@IdCliente",id)
                    };

                    DataTable dtResultPhone = dbContext.ExecutarConsulta(SQL, parametros);
                    foreach (DataRow phoneDataRow in dtResultPhone.Rows)
                    {
                        var phone = new Phone(
                                            Convert.ToInt32(phoneDataRow["Id"]),
                                            Convert.ToString(phoneDataRow["Telefone"])
                                        );
                        phones.Add(phone);
                    }
                }


                cliente = new Client(
                     Convert.ToInt32(dataRow["Id"]),
                     dataRow["Nome"].ToString(),
                     dataRow["CPF"].ToString(),
                     dataRow["RG"].ToString(),
                     Convert.ToDateTime(dataRow["DataNasc"]),
                     dataRow["Email"].ToString(),
                     new Status(statusEnum),
                     endereco,
                     phones,
                     Convert.ToDateTime(dataRow["DataAlteracao"])
                );
            }

            return cliente;
        }

        public void Update(Client client)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                new SqlParameter("@Id", client.Id),
                new SqlParameter("@Nome",client.Name),
                new SqlParameter("@CPF",client.CPF),
                new SqlParameter("@DataNasc",client.BirthDate),
                new SqlParameter("@Email",client.Email),

                new SqlParameter("@IdCidade",client.Adress?.City.Id),
                new SqlParameter("@Rua",client.Adress?.Street),
                new SqlParameter("@Bairro",client.Adress?.District),
                new SqlParameter("@Numero",client.Adress?.Number),
                new SqlParameter("@Complemento",client.Adress?.Complement),
                new SqlParameter("@Cep",client.Adress?.Cep),

                new SqlParameter("@RG",client.RG),
                };

                dbContext.ExecutarProcedure("SP_AtualizarCliente", parametros);
                EditPhones(client.Id, client.Phones);
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    throw new InvalidClientException(ex.Message);
                throw ex;
            }
        }

        public void Insert(Client client, User user)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                new SqlParameter("@IdUsuarioResponsavel", user.Id),
                new SqlParameter("@Nome",client.Name),
                new SqlParameter("@CPF",client.CPF),
                new SqlParameter("@DataNasc",client.BirthDate),
                new SqlParameter("@Email",client.Email),

                new SqlParameter("@IdCidade",client.Adress?.City.Id),
                new SqlParameter("@Rua",client.Adress?.Street),
                new SqlParameter("@Bairro",client.Adress?.District),
                new SqlParameter("@Numero",client.Adress?.Number),
                new SqlParameter("@Complemento",client.Adress?.Complement),
                new SqlParameter("@Cep",client.Adress?.Cep),

                new SqlParameter("@RG",client.RG),
                };

                dbContext.ExecutarProcedure("SP_InserirCliente", parametros);
                InsertPhones(client.Phones);
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    throw new InvalidClientException(ex.Message);
                throw ex;
            }

        }

        public void Delete(int clientId, int userId)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                    new SqlParameter("@IdUsuarioResponsavel", userId),
                    new SqlParameter("@IdCliente", clientId),
                };

                dbContext.ExecutarProcedure("SP_DeletarCliente", parametros);
            }
            catch (SqlException ex)
            {
                if (ex.Number > 50000)
                    throw new InvalidClientException(ex.Message);
                throw ex;
            }
        }

        private void InsertPhones(ICollection<Phone> phones)
        {
            var dbContext = new SQLDbContext();

            var parametros = new List<SqlParameter>();

            using (var table = new DataTable())
            {
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Telefone", typeof(string));

                foreach (var phone in phones)
                {
                    table.Rows.Add(phone.Id, phone.Number);
                }

                var pList = new SqlParameter("@Telefones", SqlDbType.Structured)
                {
                    TypeName = "dbo.TelefoneList",
                    Value = table
                };

                parametros.Add(pList);
            }

            dbContext.ExecutarProcedure("SP_InserirTelefones", parametros.ToArray());
        }

        private void EditPhones(int clientId, ICollection<Phone> phones)
        {
            var dbContext = new SQLDbContext();

            var parametros = new List<SqlParameter> {
                new SqlParameter("@IdCliente", clientId),
            };

            using (var table = new DataTable())
            {
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Telefone", typeof(string));

                foreach (var phone in phones)
                {
                    table.Rows.Add(phone.Id, phone.Number);
                }

                var pList = new SqlParameter("@Telefones", SqlDbType.Structured)
                {
                    TypeName = "dbo.TelefoneList",
                    Value = table
                };

                parametros.Add(pList);
            }

            dbContext.ExecutarProcedure("SP_AtualizarTelefones", parametros.ToArray());
        }




        public bool VerifyIfHasSameData(Client client)
        {
            try
            {
                var dbContext = new SQLDbContext();

                var parametros = new SqlParameter[] {
                new SqlParameter("@IdCliente", client.Id),
                new SqlParameter("@CPF",client.CPF)
                };

                dbContext.ExecutarProcedure("SP_VerificarSeExisteDadosDoCliente", parametros);

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

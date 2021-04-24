using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ClientesGFT.Data.Contexts
{
    public class SQLDbContext
    {
        public SqlConnection sqlconnection = new SqlConnection();


        public SqlConnection GetConnection()
        {
            try
            {
                string dadosConexao = @"Data Source=BRPC003339\SQLEXPRESS;Initial Catalog=ClientesGFT;Integrated Security=True";

                sqlconnection = new SqlConnection(dadosConexao);

                if (sqlconnection.State == ConnectionState.Closed)
                {
                    sqlconnection.Open();
                }

                return sqlconnection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataTable ExecutarConsulta(string sql, SqlParameter[] parametros = null)
        {
            try
            {
                DataTable dtresult = new DataTable();
                var comando = new SqlCommand
                {
                    Connection = GetConnection(),
                    CommandText = sql
                };

                if (parametros != null) comando.Parameters.AddRange(parametros);

                comando.ExecuteScalar();
                IDataReader dtreader = comando.ExecuteReader();

                dtresult.Load(dtreader);
                return dtresult;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }
        }

        public int ExecutarAtualizacao(string sql, SqlParameter[] parametros = null)
        {
            try
            {
                var comando = new SqlCommand
                {
                    Connection = GetConnection(),
                    CommandText = sql
                };

                if (parametros != null) comando.Parameters.AddRange(parametros);

                int result = comando.ExecuteNonQuery();

                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }
        }

        public DataTable ExecutarProcedure(string procedure, SqlParameter[] parametros = null)
        {
            try
            {
                DataTable dtresult = new DataTable();
                var comando = new SqlCommand
                {
                    Connection = GetConnection(),
                    CommandText = procedure,
                    CommandType = CommandType.StoredProcedure
                };

                if (parametros != null) comando.Parameters.AddRange(parametros);

                IDataReader dtreader = comando.ExecuteReader();

                dtresult.Load(dtreader);
                return dtresult;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }
        }
    }
}

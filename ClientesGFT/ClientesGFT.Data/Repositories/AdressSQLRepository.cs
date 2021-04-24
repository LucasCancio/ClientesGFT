using ClientesGFT.Data.Contexts;
using ClientesGFT.Domain.Entities.AdressEntities;
using ClientesGFT.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ClientesGFT.Data.Repositories
{
    public class AdressSQLRepository : IAdressRepository
    {

        public IList<Country> GetCountries()
        {
            var countries = new List<Country>();

            var dbContext = new SQLDbContext();

            string SQL = @"SELECT * FROM Paises";

            DataTable dtResult = dbContext.ExecutarConsulta(SQL);
            foreach (DataRow dataRow in dtResult.Rows)
            {
                var country = new Country(
                                     Convert.ToInt32(dataRow["Id"]),
                                     Convert.ToString(dataRow["Pais"]),
                                     Convert.ToString(dataRow["Sigla"])
                                    );

                countries.Add(country);
            }


            return countries;
        }


        public IList<State> GetStates(int countryId)
        {
            var states = new List<State>();

            var dbContext = new SQLDbContext();

            string SQL = @"SELECT * FROM Estados WHERE IdPais = @IdPais";

            var parametros = new SqlParameter[] {
                new SqlParameter("@IdPais",countryId)
            };

            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);
            foreach (DataRow dataRow in dtResult.Rows)
            {
                var state = new State(
                                     Convert.ToInt32(dataRow["Id"]),
                                     Convert.ToString(dataRow["Estado"]),
                                     Convert.ToInt32(dataRow["IdPais"])
                                    );

                states.Add(state);
            }


            return states;
        }


        public IList<City> GetCities(int stateId)
        {
            var cities = new List<City>();

            var dbContext = new SQLDbContext();

            string SQL = @"SELECT * FROM Cidades WHERE IdEstado = @IdEstado";

            var parametros = new SqlParameter[] {
                new SqlParameter("@IdEstado",stateId)
            };

            DataTable dtResult = dbContext.ExecutarConsulta(SQL, parametros);
            foreach (DataRow dataRow in dtResult.Rows)
            {
                var city = new City(
                                     Convert.ToInt32(dataRow["Id"]),
                                     Convert.ToString(dataRow["Cidade"]),
                                     Convert.ToInt32(dataRow["IdEstado"])
                                    );

                cities.Add(city);
            }


            return cities;
        }

       

        
    }
}

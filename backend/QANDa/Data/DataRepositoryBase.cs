using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QANDa.Model;
using System.Collections.Generic;

namespace QANDa.Data
{
    public abstract class DataRepositoryBase
    {
        private readonly string _connectionString;

        public DataRepositoryBase(IConfiguration configuration)
        {
            this._connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public SqlConnection StartConnection()
        {
            return new SqlConnection(this._connectionString);
        }

        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        public IEnumerable<T> ExecuteQueryForEnumerable<T>(string query,object queryParam)
        {
            var connection = StartConnection();
            IEnumerable<T> result;
            if (queryParam != null)
            {
                result = connection.Query<T>(query,queryParam);
            }
            else
                result = connection.Query<T>(query);
            CloseConnection(connection);
            return result;
        }
        public T ExecuteQueryWithDefault<T>(string query, object queryParam)
        {
            var connection = StartConnection();
            T result;
            if (queryParam != null)
            {
                result = connection.QueryFirstOrDefault<T>(query,queryParam);
            }
            else
                result = connection.QueryFirstOrDefault<T>(query);
            CloseConnection(connection);
            return result;
        }
        public T ExecuteQueryFirst<T>(string query, object queryParam)
        {
            var connection = StartConnection();
            var response = connection.QueryFirst<T>(query, queryParam);
            CloseConnection(connection);
            return response;
        }

        public void Execute(string query,object queryParam)
        {
            var connection = StartConnection();
            connection.Execute(query, queryParam);
            CloseConnection(connection);
        }
    }
}

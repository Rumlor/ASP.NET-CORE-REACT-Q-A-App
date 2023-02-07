using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QANDa.Model;
using System.Collections.Generic;
using System.Linq;

namespace QANDa.Data
{
    public abstract class DataRepositoryBase
    {
        private readonly string _connectionString;

        protected DataRepositoryBase(IConfiguration configuration)
        {
            this._connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        private SqlConnection StartConnection()
        {
            return new SqlConnection(this._connectionString);
        }

        private static void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        protected virtual void ExecuteQueryWithRelationship<T,U>(string query,IEnumerable<T> list,string childField,string idField)
        {
            using var con = StartConnection();
            foreach (var item in list)
            {
                var id = item.GetType().GetProperty(idField).GetValue(item, null);
                var queryParam = new Dictionary<string, object> { { idField, id } };
                IEnumerable<U> childs = con.Query<U>(query, queryParam);
                item.GetType().GetProperty(childField).SetValue(item, childs != null ? childs : new List<T> { });
            }

        }
        protected virtual IEnumerable<T> ExecuteQueryForEnumerable<T>(string query,object queryParam)
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
        protected virtual T ExecuteQueryWithDefault<T>(string query, object queryParam)
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
        protected virtual T ExecuteQueryFirst<T>(string query, object queryParam)
        {
            var connection = StartConnection();
            var response = connection.QueryFirst<T>(query, queryParam);
            CloseConnection(connection);
            return response;
        }

        protected virtual void Execute(string query,object queryParam)
        {
            var connection = StartConnection();
            connection.Execute(query, queryParam);
            CloseConnection(connection);
        }
    }
}

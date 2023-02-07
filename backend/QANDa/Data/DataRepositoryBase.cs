using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QANDa.Model;
using System;
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
            foreach (T item in list)
            {
                var id = item.GetType().GetProperty(idField).GetValue(item, null);
                var queryParam = new Dictionary<string, object> { { idField, id } };
                IEnumerable<U> childs = con.Query<U>(query, queryParam);
                item.GetType().GetProperty(childField).SetValue(item, childs);
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
        protected virtual IEnumerable<T> ExecuteMappedQuery<T,U>(string query,object queryParam,string childField, string idField,string childIdField)
        {
            using var connection = StartConnection();
            var parentDictionary = new Dictionary<int, T>();
           return  connection.Query<T, U, T>(query, map:(parent, child) =>
                {
                    T parentValue;
                    var parentId = ((int)parent.GetType().GetProperty(idField).GetValue(parent, null));
                    if (!parentDictionary.TryGetValue(parentId, out parentValue))
                    {
                        parentValue = parent;
                        parentDictionary.Add(parentId, parentValue);
                    }
                    var childs = parentValue.GetType().GetProperty(childField).GetValue(parentValue, null) as List<U>;
                    
                    if(child.GetType().GetProperty(childIdField).GetValue(child,null) != null)
                        childs.Add(child);
                    
                    parentValue.GetType().GetProperty(childField).SetValue(parent, childs);
                    return parentValue;
                },splitOn: idField).Distinct();
        }
    }
}

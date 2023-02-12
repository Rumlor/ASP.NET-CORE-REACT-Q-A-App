using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        protected virtual async Task<IEnumerable<T>> ExecuteQueryForEnumerable<T>(string query,object queryParam)
        {
            var connection = StartConnection();
            await connection.OpenAsync();
            IEnumerable<T> result;
            if (queryParam != null)
            {
                result = await connection.QueryAsync<T>(query,queryParam);
            }
            else
                result = await connection.QueryAsync<T>(query);
            connection.CloseAsync().Wait();
            return result;
        }
        protected virtual async Task<T> ExecuteQueryForMultipleAsync<T,U>(string query,string childField,object param)
        {
            var con = StartConnection();
            await con.OpenAsync();
            using var results = await con.QueryMultipleAsync(query,param);
            var question = results.Read<T>().FirstOrDefault();
            if(question != null)
            {
                var childs = (await results.ReadAsync<U>()).ToList();
                question.GetType().GetProperty(childField).SetValue(question,childs);
            }
            return question;
        }
        protected virtual async  Task<T> ExecuteQueryWithDefault<T>(string query, object queryParam)
        {
            var connection = StartConnection();
            await connection.OpenAsync();
            T result;
            if (queryParam != null)
            {
                result = await connection.QueryFirstOrDefaultAsync<T>(query,queryParam);
            }
            else
                result =  await connection.QueryFirstOrDefaultAsync<T>(query);
            await connection.CloseAsync();
            return result;
        }
        protected virtual async Task<T> ExecuteQueryFirst<T>(string query, object queryParam)
        {
            var connection = StartConnection();
            var response = await connection.QueryFirstAsync<T>(query, queryParam);
            await connection.CloseAsync();
            return response;
        }
        protected virtual async Task Execute(string query,object queryParam)
        {
            var connection = StartConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(query, queryParam);
            await connection.CloseAsync();
            return;
        }
        protected async Task<IEnumerable<T>> ExecuteQueryFromEnumerableAsync<T>(string query,object queryParam)
        {
            var connection = StartConnection();
            await connection.OpenAsync();
            return await (queryParam == null ? connection.QueryAsync<T>(query) : connection.QueryAsync<T>(query, queryParam));
        }
        protected virtual async Task<IEnumerable<T>> ExecuteMappedQuery<T,U>(string query,object queryParam,string childField, string idField,string childIdField)
        {
            using var connection = StartConnection();
            var parentDictionary = new Dictionary<int, T>();
            var result = await connection.QueryAsync<T, U, T>(query, map:(parent, child) =>
                {
                    var parentId = ((int)parent.GetType().GetProperty(idField).GetValue(parent, null));
                    //out T parentValue inlined parameter syntax !!
                    if (!parentDictionary.TryGetValue(parentId, out T parentValue))
                    {
                        parentValue = parent;
                        parentDictionary.Add(parentId, parentValue);
                    }
                    var childs = parentValue.GetType().GetProperty(childField).GetValue(parentValue, null) as List<U>;
                    
                    if(child!= null && child.GetType().GetProperty(childIdField).GetValue(child,null) != null)
                        childs.Add(child);
                    
                    parentValue.GetType().GetProperty(childField).SetValue(parentValue, childs);
                    return parentValue;
                }, queryParam, splitOn:childIdField);
            return result.Distinct();
        }
    }
}

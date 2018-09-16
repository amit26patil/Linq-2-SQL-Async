using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSqlAsync
{
    public class AsyncDataContext : DataContext
    {
        private string connectionString;
        public AsyncDataContext(string connectionString):base(connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<List<T>> ExecuteQueryAsync<T>(IQueryable<T> query)
        {
            using (var sqlCommand = GetCommand(query))
            {
                sqlCommand.Connection = Connection;
                if (sqlCommand.Connection.State == ConnectionState.Closed)
                {
                    await sqlCommand.Connection.OpenAsync();
                }
                var result = await sqlCommand.ExecuteReaderAsync();
                return Translate<T>(result).ToList();
            }
        }
    }
}

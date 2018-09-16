using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSqlAsync
{
    public static class Extensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query, DataContext context)
        {
            return await internalToListAsync(query, context);
        }

        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            var context = GetContext(query);
            if (context == null)
            {
                return null;
            }
            return await internalToListAsync(query, context);
        }

        private static async Task<List<T>> internalToListAsync<T>(IQueryable<T> query, DataContext context)
        {
            using (var sqlCommand = context.GetCommand(query))
            {
                sqlCommand.Connection = context.Connection;
                if (sqlCommand.Connection.State == ConnectionState.Closed)
                {
                    await sqlCommand.Connection.OpenAsync();
                }
                var result = await sqlCommand.ExecuteReaderAsync();
                return context.Translate<T>(result).ToList();
            }
        }
        private static DataContext GetContext(IQueryable query)
        {
            var queryType = query.GetType();
            if (!queryType.FullName.StartsWith("System.Data.Linq.DataQuery`1"))
            {
                return null;
            }
            var field = queryType.GetField("context", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                return null;
            }
            return field.GetValue(query) as DataContext;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
namespace LinqToSqlAsync
{
    [Table(Name ="Students")]
    public class Students
    {
        [Column(Name ="id",DbType ="BigInt")]
        public int Id { get; set; }
    }
    class Program
    {
        private static string connectionString = @"<<Your ConnectionString>>";

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
            AsyncDataContext asyncDataContext = new AsyncDataContext(connectionString);
            var asyncQuery = asyncDataContext.GetTable<Students>().Where(s => s.Id > 0);
            
            var result = await asyncDataContext.ExecuteQueryAsync(asyncQuery);//Using AsyncDataContext

            DataContext context = new DataContext(connectionString);
            var query = context.GetTable<Students>().Where(s => s.Id > 0);
            var resultExtension = await query.ToListAsync(context);//Using Extension
            
            var resultExtension1 = await query.ToListAsync();//Using Extension Without DataContext=> NOT RECOMMENDED
        }
    }
}

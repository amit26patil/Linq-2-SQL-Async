# Linq-2-SQL-Async
Async Methods for Linq 2 SQL

# Usage
AsyncDataContext asyncDataContext = new AsyncDataContext(connectionString);

var asyncQuery = asyncDataContext.GetTable<Students>().Where(s => s.Id > 0);

var result = await asyncDataContext.ExecuteQueryAsync(asyncQuery);//Using AsyncDataContext

DataContext context = new DataContext(connectionString);

var query = context.GetTable<Students>().Where(s => s.Id > 0);
var resultExtension = await query.ToListAsync(context);//Using Extension

var resultExtension1 = await query.ToListAsync();# //Using Extension Without DataContext=> NOT RECOMMENDED

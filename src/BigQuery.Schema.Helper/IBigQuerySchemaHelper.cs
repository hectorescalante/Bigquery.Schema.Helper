using Google.Cloud.BigQuery.V2;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigQuery.Schema.Helper
{
  public interface IBigQuerySchemaHelper
  {
    Task<BigQueryTable> GetOrCreateTableAsync<T>(string optionsName = "");
    Task InsertRowsAsync<T>(IEnumerable<object> entityList, string optionsName = "");
    Task InsertRowAsync<T>(object entity, string optionsName = "");
    Task<IEnumerable<BigQueryRow>> QueryAsync(string query, BigQueryParameter[] parameters, string optionsName = "");
    Task<int> ExecuteAsync(string query, BigQueryParameter[] parameters, string optionsName = "");
    string GetTableName<T>(string optionsName = "");
  }
}

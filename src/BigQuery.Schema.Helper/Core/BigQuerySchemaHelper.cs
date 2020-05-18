using BigQuery.Schema.Helper.Core.Abstractions;
using Google.Cloud.BigQuery.V2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BigQuery.Schema.Helper.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace BigQuery.Schema.Helper.Core
{
  internal class BigQuerySchemaHelper : IBigQuerySchemaHelper
  {
    private readonly IOptionsSnapshot<BigQuerySchemaHelperOptions> _options;
    private readonly ILogger _logger;
    private readonly IBigQueryClientFactory _bigQueryClientFactory;

    public BigQuerySchemaHelper(IOptionsSnapshot<BigQuerySchemaHelperOptions> options, ILogger<BigQuerySchemaHelper> logger, IBigQueryClientFactory bigQueryClientFactory)
    {
      _logger = logger;
      _options = options;
      _bigQueryClientFactory = bigQueryClientFactory;
    }

    public string GetTableName<T>(string optionsName = "") => _options.Get(optionsName).GetFullTableName(typeof(T).Name);

    public async Task<BigQueryTable> GetOrCreateTableAsync<T>(string optionsName = "")
    {
      var currentOptions = _options.Get(optionsName);
      var client = _bigQueryClientFactory.GetOrCreateClient(currentOptions.ProjectId);

      var type = typeof(T);

      _logger.LogInformation($"Building schema from Type: {type.Name}");
      var schemaBuilder = BigQuerySchemaBuilder.GetSchemaBuilder(type, "> ");
      var createTableOptions = new CreateTableOptions() { TimePartitioning = TimePartition.CreateDailyPartitioning(expiration: null) };

      _logger.LogInformation($"Get or Create Dataset: {currentOptions.DatasetName}");
      _ = await client.GetOrCreateDatasetAsync(currentOptions.DatasetName);

      _logger.LogInformation($"Get or Create Table: {currentOptions.GetTableName(type.Name)}");
      return await client.GetOrCreateTableAsync(currentOptions.DatasetName, currentOptions.GetTableName(type.Name), schemaBuilder.Build(), createOptions: createTableOptions);
    }

    public async Task InsertRowAsync<T>(object entity, string optionsName = "")
    {
      await InsertRowsAsync<T>(new List<object>() { { entity } }, optionsName);
    }

    public async Task InsertRowsAsync<T>(IEnumerable<object> entityList, string optionsName = "")
    {
      _logger.LogInformation($"Creating rows...");
      var bigQueryRows = GetRowsFromEntity(entityList, typeof(T), "+ ");

      _logger.LogInformation($"Inserting Rows...");
      var currentOptions = _options.Get(optionsName);
      var client = _bigQueryClientFactory.GetOrCreateClient(currentOptions.ProjectId);
      await client.InsertRowsAsync(currentOptions.DatasetName, currentOptions.GetTableName(typeof(T).Name), bigQueryRows, new InsertOptions() { AllowUnknownFields = true });

      _logger.LogInformation($"Successfully inserted!!");
    }

    public async Task<IEnumerable<BigQueryRow>> QueryAsync(string query, BigQueryParameter[] parameters, string optionsName = "")
    {
      var client = _bigQueryClientFactory.GetOrCreateClient(_options.Get(optionsName).ProjectId);
      return await client.ExecuteQueryAsync(query, parameters);
    }

    public async Task<int> ExecuteAsync(string query, BigQueryParameter[] parameters, string optionsName = "")
    {
      var client = _bigQueryClientFactory.GetOrCreateClient(_options.Get(optionsName).ProjectId);
      var result = await client.ExecuteQueryAsync(query, parameters);
      return (int)result.NumDmlAffectedRows;
    }

    private IEnumerable<BigQueryInsertRow> GetRowsFromEntity(IEnumerable<object> entityList, Type schemaType, string indent)
    {
      foreach (var entity in entityList)
      {
        yield return BigQueryRowAdapter.GetRowFromEntity(entity, schemaType, indent);
      }
    }
  }
}

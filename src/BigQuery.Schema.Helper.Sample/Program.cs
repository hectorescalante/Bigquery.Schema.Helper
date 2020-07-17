using BigQuery.Schema.Helper.Core;
using BigQuery.Schema.Helper.Core.Abstractions;
using BigQuery.Schema.Helper.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace BigQuery.Schema.Helper.Sample
{
  class Program
  {
    static void Main(string[] args)
    {
      var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

      var helperOptions = new BigQuerySchemaHelperOptions();
      configuration.GetSection("BIGQUERYSINK").Bind(helperOptions);

      var serviceProvider = new ServiceCollection()
        .AddLogging(configure => configure.AddConsole())
        .AddBigQuerySchemaHelper(helperOptions)
        .BuildServiceProvider();

      var bigQueryShemaHelper = serviceProvider.GetService<IBigQuerySchemaHelper>();

      var table = bigQueryShemaHelper.GetOrCreateTableAsync<BusinessTraceData>().GetAwaiter().GetResult();

      var timestamp = DateTime.Now;
      var trace = new BusinessTraceData(timestamp);

      bigQueryShemaHelper.InsertRowAsync<BusinessTraceData>(trace).Wait();

    }
  }
}

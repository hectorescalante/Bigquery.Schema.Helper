using BigQuery.Schema.Helper.Core;
using BigQuery.Schema.Helper.Core.Abstractions;
using BigQuery.Schema.Helper.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BigQuery.Schema.Helper.Core
{
  public static class BigQuerySchemaHelperExtensions
  {
    public static IServiceCollection AddBigQuerySchemaHelper(this IServiceCollection services, IConfigurationSection configurationSection)
    {
      return services.AddBigQuerySchemaHelper(configurationSection, "");
    }
    public static IServiceCollection AddBigQuerySchemaHelper(this IServiceCollection services, IConfigurationSection configurationSection, string optionsName)
    {
      var helperOptions = new BigQuerySchemaHelperOptions();
      configurationSection.Bind(helperOptions);
      return services.AddBigQuerySchemaHelper(helperOptions, optionsName);
    }
    public static IServiceCollection AddBigQuerySchemaHelper(this IServiceCollection services, BigQuerySchemaHelperOptions helperOptions)
    {
      return services.AddBigQuerySchemaHelper(helperOptions, "");
    }
    public static IServiceCollection AddBigQuerySchemaHelper(this IServiceCollection services, BigQuerySchemaHelperOptions helperOptions, string optionsName)
    {
      services.Configure<BigQuerySchemaHelperOptions>(optionsName, options =>
      {
        options.ProjectId = helperOptions.ProjectId;
        options.DatasetName = helperOptions.DatasetName;
        options.TeamId = helperOptions.TeamId;
        options.AppId = helperOptions.AppId;
      });
      services.AddSingleton<IBigQueryClientFactory, BigQueryClientFactory>();
      services.AddScoped<IBigQuerySchemaHelper, BigQuerySchemaHelper>();
      return services;
    }
  }
}

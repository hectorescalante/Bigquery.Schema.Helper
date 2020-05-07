namespace BigQuery.Schema.Helper
{
  public class BigQuerySchemaHelperOptions
  {
    public string ProjectId { get; set; }
    public string DatasetName { get; set; } = "";
    public string TeamId { get; set; } = "";
    public string AppId { get; set; } = "";
    public string Environment { get; set; } = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    public bool PrintDebugInfo { get; set; } = false;
    public bool IsEnabled { get; set; } = true;

    public string GetTableName(string typeName) => $"{Environment}_{TeamId}_{AppId}_{typeName}";
    public string GetFullTableName(string typeName) => $"{ProjectId}.{DatasetName}.{GetTableName(typeName)}";
  }
}

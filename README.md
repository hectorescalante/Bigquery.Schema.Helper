# dotnet-bigquery-schema-helper
BigQuery table creation and data insertion helper

## Configuration
```
"BigQueryTraces": {
    "ProjectId": "my-google-cloud-project",
    "DatasetName": "MyDataset",
    "TeamId": "MyTeam",
    "AppId": "MyApp"
  }
```
## Startup
```
services.AddBigQuerySchemaHelper(_configuration.GetSection("MyBigQuerySection"));
```

## Table Creation
```
_ = await _bigQuerySchemaHelper.GetOrCreateTableAsync<MyLogClass>();
```

## Rows Insertion
```
await _bigQuerySchemaHelper.InsertRowsAsync<MyLogClass>(MyLogClassList);
```

## Google Credential

Required Roles/Permissions:

- BigQuery Data Editor
- BigQuery User

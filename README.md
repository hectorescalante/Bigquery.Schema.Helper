# dotnet-bigquery-schema-helper
BigQuery table creation and data insertion helper
## Configuration
```
"MyBigQuerySection": {
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
### Class and IEnumerable Property Types
Class type properties are converted to nested columns and IEnumerable types are converted to nested and repeated columns.
## Rows Insertion
```
await _bigQuerySchemaHelper.InsertRowsAsync<MyLogClass>(MyLogClassList);
```
## Google Credential
Required Roles/Permissions:
- BigQuery Data Editor
- BigQuery User

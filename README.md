# BigQuery.Schema.Helper
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

## Dependency Injection
You can define an IBigQuerySchemaHelper dependency in your class.
```
public class MySampleClass()
{
    private readonly IBigQuerySchemaHelper _bigQuerySchemaHelper;
    public MySampleClass(IBigQuerySchemaHelper bigQuerySchemaHelper)
    {
        _bigQuerySchemaHelper = bigQuerySchemaHelper;
    }
}
```

## Table Creation
```
_ = await _bigQuerySchemaHelper.GetOrCreateTableAsync<MyLogClass>();
```

### Class and IEnumerable Property Types
Class type properties are converted to nested columns and IEnumerable types are converted to nested and repeated columns.

### Schema Attribute
Use the *BigQuerySchema* attribute in class properties to:
- Ignore a property: [BigQuerySchema(**ignore:true**)]

## Rows Insertion
```
# Single row
await _bigQuerySchemaHelper.InsertRowAsync<MyLogClass>(MyLogClassInstance);

# Multiple rows
await _bigQuerySchemaHelper.InsertRowsAsync<MyLogClass>(MyLogClassList);
```

## Google Credential
Required Roles/Permissions:
- BigQuery Data Editor
- BigQuery User

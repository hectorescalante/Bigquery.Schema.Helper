using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using System;
using System.Linq;
using System.Reflection;

namespace BigQuery.Schema.Helper.Core
{
  public class BigQuerySchemaBuilder : BigQueryCommon
  {
    public static TableSchemaBuilder GetSchemaBuilder(Type schemaType, string indent = "")
    {
      var schemaBuilder = new TableSchemaBuilder();

      foreach (var propertyItem in schemaType.GetProperties())
      {
        if (IgnoreProperty(propertyItem.GetCustomAttribute<BigQuerySchemaAttribute>()))
          continue;

        if (IsEnumerable(propertyItem.PropertyType))
        {
          var elementType = GetElementsType(propertyItem);

          if (IsClass(elementType))
          {
            WritePropertyMessage(propertyItem, indent, "IEnumerableClass");
            var propertySchemaBuilder = GetSchemaBuilder(propertyItem.PropertyType.GetGenericArguments().FirstOrDefault(), indent.Insert(0, _spaces));
            schemaBuilder.Add(NewTableFieldSchema(propertyItem.Name, "RECORD", BigQueryFieldMode.Repeated.ToString(), propertySchemaBuilder.Build()));
          }
          else
          {
            WritePropertyMessage(propertyItem, indent, "IEnumerableType");
            schemaBuilder.Add(NewTableFieldSchema(propertyItem.Name, elementType.ToBigQueryDbType(), BigQueryFieldMode.Repeated.ToString()));
          }
        }
        else if (IsClass(propertyItem.PropertyType))
        {
          WritePropertyMessage(propertyItem, indent, "Class");
          var propertySchemaBuilder = GetSchemaBuilder(propertyItem.PropertyType, indent.Insert(0, _spaces));
          schemaBuilder.Add(NewTableFieldSchema(propertyItem.Name, "RECORD", BigQueryFieldMode.Nullable.ToString(), propertySchemaBuilder.Build()));
        }
        else
        {
          WritePropertyMessage(propertyItem, indent, "Type");
          schemaBuilder.Add(NewTableFieldSchema(propertyItem.Name, propertyItem.PropertyType.ToBigQueryDbType(), BigQueryFieldMode.Nullable.ToString()));
        }
      }
      return schemaBuilder;
    }


    private static TableFieldSchema NewTableFieldSchema(string name, string type, string mode, TableSchema schema = null)
    {
      return new TableFieldSchema()
      {
        Name = name,
        Type = type,
        Mode = mode,
        Fields = schema?.Fields
      };
    }

  }
}

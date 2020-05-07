using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;

namespace BigQuery.Schema.Helper.Core
{
  internal static class BigQueryExtensions
  {

    public static string ToBigQueryDbType(this Type propertyType)
    {
      if (propertyType == typeof(string))
        return BigQueryDbType.String.ToString();
      if (propertyType == typeof(int))
        return BigQueryDbType.Int64.ToString();
      if (propertyType == typeof(bool))
        return BigQueryDbType.Bool.ToString();
      if (propertyType == typeof(DateTime))
        return BigQueryDbType.DateTime.ToString();
      if (propertyType == typeof(float))
        return BigQueryDbType.Float64.ToString();
      if (propertyType == typeof(decimal))
        return BigQueryDbType.Numeric.ToString();
      if (propertyType == typeof(Array))
        return BigQueryDbType.Array.ToString();

      return BigQueryDbType.String.ToString();
    }
    public static Type GetEnumeratedType(this IEnumerable<object> _) => typeof(object);

    public static T GetValue<T>(this BigQueryRow bigQueryRow, string key)
    {
      if (!key.Contains("."))
        return (T)Convert.ChangeType(bigQueryRow[key], typeof(T));

      var root = key.Substring(0, key.IndexOf("."));
      return GetNestedValue<T>(key.Remove(0, root.Length + 1), bigQueryRow[root] as Dictionary<string, object>);
    }

    private static T GetNestedValue<T>(string key, Dictionary<string, object> row)
    {
      if (!key.Contains("."))
        return (T)Convert.ChangeType(row[key], typeof(T));

      var root = key.Substring(0, key.IndexOf("."));
      return GetNestedValue<T>(key.Remove(0, root.Length + 1), row[root] as Dictionary<string, object>);
    }

  }
}

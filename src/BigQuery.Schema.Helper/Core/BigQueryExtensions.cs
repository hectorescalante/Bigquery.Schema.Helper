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
  }
}

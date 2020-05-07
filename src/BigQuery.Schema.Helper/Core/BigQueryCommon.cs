using System;
using System.Linq;
using System.Reflection;

namespace BigQuery.Schema.Helper.Core
{
  internal class BigQueryCommon
  {
    protected static readonly string _spaces = "  ";

    protected static void WritePropertyMessage(PropertyInfo property, string indent, string category) =>
      Console.WriteLine($"{indent}{property.Name} => {category}: {property.PropertyType}");

    protected static bool IsEnumerable(Type propertyType) =>
      propertyType != typeof(string) && (
        propertyType.Name.Contains("IEnumerable") ||
        propertyType.GetInterfaces().Any(i => i.IsGenericType && i.Name.Contains("IEnumerable"))
      );

    protected static bool IsClass(Type propertyType) =>
      propertyType.IsClass && propertyType != typeof(string);

    protected static Type GetElementsType(PropertyInfo property) =>
      property.PropertyType.GetGenericArguments().FirstOrDefault();
  }
}

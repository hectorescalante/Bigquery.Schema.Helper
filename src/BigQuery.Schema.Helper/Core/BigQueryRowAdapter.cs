﻿using Google.Cloud.BigQuery.V2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BigQuery.Schema.Helper.Core
{
  internal class BigQueryRowAdapter : BigQueryCommon
  {
    public static BigQueryInsertRow GetRowFromEntity(object entity, Type schemaType, string indent = "")
    {
      //Nothing to do with nulls
      if (entity == null) { return null; }

      indent = indent.Insert(0, _spaces);

      var newRow = new BigQueryInsertRow();
      foreach (var propertyItem in schemaType.GetProperties())
      {
        if (IsEnumerable(propertyItem.PropertyType))
        {
          var elementType = GetElementsType(propertyItem);

          if (IsClass(elementType))
          {
            //WritePropertyMessage(propertyItem, indent, "IEnumerableOfClass");
            newRow.Add(propertyItem.Name, GetClassRows((IEnumerable)propertyItem.GetValue(entity), elementType, indent));
          }
          else
          {
            //WritePropertyMessage(propertyItem, indent, "IEnumerableOfType");
            newRow.Add(propertyItem.Name, GetValueByListType(elementType, propertyItem.GetValue(entity)));
          }
        }
        else if (IsClass(propertyItem.PropertyType))
        {
          //WritePropertyMessage(propertyItem, indent, "Class");
          var classRow = GetRowFromEntity(propertyItem.GetValue(entity), propertyItem.PropertyType, indent);
          newRow.Add(propertyItem.Name, classRow);
        }
        else
        {
          //WritePropertyMessage(propertyItem, indent, "Type");
          newRow.Add(propertyItem.Name, GetValueByType(propertyItem.PropertyType, propertyItem.GetValue(entity)));
        }
      }
      return newRow;
    }

    private static List<BigQueryInsertRow> GetClassRows(IEnumerable classList, Type elementType, string indent)
    {
      var rowList = new List<BigQueryInsertRow>();
      if (classList != null)
      {
        foreach (var classItem in classList)
        {
          rowList.Add(GetRowFromEntity(classItem, elementType, indent));
        }
      }
      return rowList;
    }

    private static object GetValueByListType(Type elementType, object propertyValue)
    {
      var stringList = new List<string>();
      if (elementType == typeof(decimal))
      {
        var decimalList = (List<decimal>)propertyValue;
        if (decimalList != null)
        {
          decimalList.ForEach(value => stringList.Add(FormatDecimal(value)));
        }
        return stringList;
      }
      else if (elementType == typeof(DateTime))
      {
        var dateTimeList = (List<DateTime>)propertyValue;
        if (dateTimeList != null)
        {
          dateTimeList.Where(date => date != DateTime.MinValue).ToList().ForEach(value => stringList.Add(FormatDateTime(value)));
        }
        return stringList;
      }

      return propertyValue ?? stringList;
    }

    private static object GetValueByType(Type propertyType, object propertyValue)
    {
      if (propertyType == typeof(decimal))
      {
        propertyValue = FormatDecimal(decimal.Parse(propertyValue.ToString()));
      }
      else if (propertyType == typeof(DateTime))
      {
        var dateValue = DateTime.Parse(propertyValue.ToString());
        propertyValue = dateValue != DateTime.MinValue ? FormatDateTime(dateValue) : null;
      }
      return propertyValue;
    }

    private static string FormatDecimal(decimal number) =>
      number.ToString("N", CultureInfo.CurrentCulture);
    private static string FormatDateTime(DateTime date) =>
      date.ToString("u", CultureInfo.InvariantCulture);
  }
}

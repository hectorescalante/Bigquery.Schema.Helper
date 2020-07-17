using System;
using System.Collections.Generic;
using System.Text;

namespace BigQuery.Schema.Helper.Core
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public class BigQuerySchemaAttribute : Attribute
  {
    public BigQuerySchemaAttribute(bool ignore)
    {
      Ignore = ignore;
    }

    public bool Ignore { get; private set; }
  }
}

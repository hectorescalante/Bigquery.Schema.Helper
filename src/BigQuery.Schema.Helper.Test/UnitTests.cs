using BigQuery.Schema.Helper.Core;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using Xunit;

namespace BigQuery.Schema.Helper.Test
{
  public class UnitTests
  {
    [Fact]
    public void TestGetSchemaBuilder_WithoutInstance_ShouldReturnAnInstanceOfSchemaBuilder()
    {
      //Arrange
      var schemaBuilder = BigQuerySchemaBuilder.GetSchemaBuilder(typeof(BusinessTraceData));

      //Act
      var schema = schemaBuilder.Build();

      //Assert
      Assert.True(schema.Fields.Count > 0);
      Assert.Contains(schema.Fields, f => f.Type.Equals("int64", StringComparison.InvariantCultureIgnoreCase));
      Assert.Contains(schema.Fields, f => f.Type.Equals("string", StringComparison.InvariantCultureIgnoreCase));
      Assert.Contains(schema.Fields, f => f.Type.Equals("numeric", StringComparison.InvariantCultureIgnoreCase));
      Assert.Contains(schema.Fields, f => f.Type.Equals("bool", StringComparison.InvariantCultureIgnoreCase));
      Assert.Contains(schema.Fields, f => f.Type.Equals("DateTime", StringComparison.InvariantCultureIgnoreCase));
      Assert.Contains(schema.Fields, f => f.Type.Equals("Record", StringComparison.InvariantCultureIgnoreCase));
      Assert.Contains(schema.Fields, f => f.Mode.Equals("Repeated", StringComparison.InvariantCultureIgnoreCase));
    }

    [Fact]
    public void TestGetRowFromEntity_WithInstance_ShouldReturnBigQueryInsertRow()
    {
      //Arrange
      var timestamp = DateTime.Now;
      var trace = new BusinessTraceData(timestamp);

      //Act
      var row = BigQueryRowAdapter.GetRowFromEntity(trace, trace.GetType());

      //Assert
      Assert.Equal(typeof(BigQueryInsertRow), row.GetType());
      Assert.NotEmpty(row);
      Assert.Equal(100, row["Id"]);
      Assert.Equal("Test title", row["Title"]);
      Assert.Equal(timestamp.ToString("s"), row["CreatedAt"]);
      Assert.Equal(true, row["IsActive"]);
      Assert.Equal("300.00", row["Amount"]);
      Assert.Collection((List<int>)row["ChildTraces"],
        item1 => { Assert.Equal(1, item1); },
        item2 => { Assert.Equal(2, item2); },
        item3 => { Assert.Equal(3, item3); },
        item4 => { Assert.Equal(4, item4); },
        item5 => { Assert.Equal(5, item5); }
      );
      Assert.Collection((List<string>)row["Tags"],
        item1 => { Assert.Equal("Value1", item1); },
        item2 => { Assert.Equal("Value2", item2); },
        item3 => { Assert.Equal("Value3", item3); }
      );
      Assert.Collection((List<string>)row["ModifiedAt"],
        item1 => { Assert.Equal(timestamp.AddDays(-1).ToString("s"), item1); },
        item2 => { Assert.Equal(timestamp.AddDays(-2).ToString("s"), item2); }
      );
      Assert.Collection((List<string>)row["Billing"],
        item1 => { Assert.Equal("1200.00", item1); },
        item2 => { Assert.Equal("2400.00", item2); },
        item3 => { Assert.Equal("4800.00", item3); },
        item4 => { Assert.Equal("9600.00", item4); }
      );
      Assert.Equal(typeof(BigQueryInsertRow), row["Detail"].GetType());
      Assert.NotEmpty((BigQueryInsertRow)row["Detail"]);
      Assert.Equal(1, ((BigQueryInsertRow)row["Detail"])["Id"]);
      Assert.Equal("Test Description", ((BigQueryInsertRow)row["Detail"])["Description"]);
      Assert.Equal(true, ((BigQueryInsertRow)row["Detail"])["IsSuccesful"]);
      Assert.Equal(timestamp.ToString("s"), ((BigQueryInsertRow)row["Detail"])["Timestamp"]);

      Assert.Collection((List<BigQueryInsertRow>)row["History"],
        item1 =>
        {
          Assert.Equal(0, item1["Id"]);
          Assert.Null(item1["Description"]);
          Assert.False((bool)item1["IsSuccesful"]);
          Assert.Null(item1["Timestamp"]);
        },
        item2 =>
        {
          Assert.Equal(2, item2["Id"]);
          Assert.Null(item2["Description"]);
          Assert.False((bool)item2["IsSuccesful"]);
          Assert.Null(item2["Timestamp"]);
        },
        item3 =>
        {
          Assert.Equal(3, item3["Id"]);
          Assert.NotNull(item3["Description"]);
          Assert.False((bool)item3["IsSuccesful"]);
          Assert.Null(item3["Timestamp"]);
        },
        item4 =>
        {
          Assert.Equal(4, item4["Id"]);
          Assert.NotNull(item4["Description"]);
          Assert.True((bool)item4["IsSuccesful"]);
          Assert.Null(item4["Timestamp"]);
        },
        item5 =>
        {
          Assert.Equal(5, item5["Id"]);
          Assert.NotNull(item5["Description"]);
          Assert.False((bool)item5["IsSuccesful"]);
          Assert.Equal(timestamp.ToString("s"), item5["Timestamp"]);
        }
      );

    }

    [Theory]
    [MemberData(nameof(GetBusinessTraces))]
    public void TestGetRowFromEntity_WithInstances_ShouldReturnBigQueryInsertRow(BusinessTraceData trace)
    {
      //Arrange
      var traceType = trace.GetType();

      //Act
      var row = BigQueryRowAdapter.GetRowFromEntity(trace, traceType);

      //Assert
      Assert.Equal(typeof(BigQueryInsertRow), row.GetType());
      Assert.NotEmpty(row);
    }

    public static IEnumerable<object[]> GetBusinessTraces()
    {
      yield return new object[]
      {
        new BusinessTraceData()
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title"
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 }
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" }
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" },
          ModifiedAt = new List<DateTime>() { DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3) }
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" },
          ModifiedAt = new List<DateTime>() { DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3) },
          Results = new List<bool>() { true, false, true, false }
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" },
          ModifiedAt = new List<DateTime>() { DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3) },
          Results = new List<bool>() { true, false, true, false },
          Billing = new List<decimal>() { 1200.00M, 2400.00M, 4800.00M, 9600.00M}
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" },
          ModifiedAt = new List<DateTime>() { DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3) },
          Results = new List<bool>() { true, false, true, false },
          Billing = new List<decimal>() { 1200.00M, 2400.00M, 4800.00M, 9600.00M},
          Detail = new BusinessTraceDataDetail()
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" },
          ModifiedAt = new List<DateTime>() { DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3) },
          Results = new List<bool>() { true, false, true, false },
          Billing = new List<decimal>() { 1200.00M, 2400.00M, 4800.00M, 9600.00M},
          Detail = new BusinessTraceDataDetail(){ Id = 11, Description = "Test Description", IsSuccesful = true, Timestamp = DateTime.Now }
        }
      };
      yield return new object[]
      {
        new BusinessTraceData()
        {
          Id = 100,
          Title = "Test title",
          CreatedAt = DateTime.Now,
          IsActive = true,
          Amount = 300.00M,
          ChildTraces = new List<int>(){ 1, 2, 3, 4, 5 },
          Tags = new List<string>(){ "Value1", "Value2", "Value3" },
          ModifiedAt = new List<DateTime>() { DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-3) },
          Results = new List<bool>() { true, false, true, false },
          Billing = new List<decimal>() { 1200.00M, 2400.00M, 4800.00M, 9600.00M},
          Detail = new BusinessTraceDataDetail(){ Id = 1, Description = "Test Description", IsSuccesful = true, Timestamp = DateTime.Now },
          History = new List<BusinessTraceDataDetail>()
          {
            new BusinessTraceDataDetail(),
            new BusinessTraceDataDetail() { Id = 2 },
            new BusinessTraceDataDetail() { Id = 2, Description = "Test Description" },
            new BusinessTraceDataDetail() { Id = 2, Description = "Test Description", IsSuccesful = false },
            new BusinessTraceDataDetail() { Id = 2, Description = "Test Description", IsSuccesful = false, Timestamp = DateTime.UtcNow }
          }
        }
      };
    }
  }
}

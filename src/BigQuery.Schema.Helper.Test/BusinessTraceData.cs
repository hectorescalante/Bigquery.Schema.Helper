using BigQuery.Schema.Helper.Core;
using System;
using System.Collections.Generic;

namespace BigQuery.Schema.Helper.Test
{
  public class BusinessTraceData
  {
    public BusinessTraceData() { }

    public BusinessTraceData(DateTime timestamp)
    {
      Id = 100;
      Title = "Test title";
      CreatedAt = timestamp;
      IsActive = true;
      Amount = 300.00M;
      ChildTraces = new List<int>() { 1, 2, 3, 4, 5 };
      Tags = new List<string>() { "Value1", "Value2", "Value3" };
      ModifiedAt = new List<DateTime>() { timestamp.AddDays(-1), DateTime.MinValue, timestamp.AddDays(-2), DateTime.MinValue };
      Results = new List<bool>() { true, false, true, false };
      Billing = new List<decimal>() { 1200.00M, 2400.00M, 4800.00M, 9600.00M };
      Detail = new BusinessTraceDataDetail() { Id = 1, Description = "Test Description", IsSuccesful = true, Timestamp = timestamp, UpdatedById = "UserId" };
      History = new List<BusinessTraceDataDetail>()
      {
        new BusinessTraceDataDetail(),
        new BusinessTraceDataDetail() { Id = 2 },
        new BusinessTraceDataDetail() { Id = 3, Description = "Test Description" },
        new BusinessTraceDataDetail() { Id = 4, Description = "Test Description", IsSuccesful = true },
        new BusinessTraceDataDetail() { Id = 5, Description = "Test Description", IsSuccesful = false, Timestamp = timestamp },
        new BusinessTraceDataDetail() { Id = 6, Description = "Test Description", IsSuccesful = false, Timestamp = timestamp, UpdatedById = "UserId" }
      };
      PrivateDetail = new BusinessTraceDataDetail() { Id = 0, Description = "Private Test Description", IsSuccesful = true, Timestamp = timestamp, UpdatedById = "UserId" };
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public decimal Amount { get; set; }

    public IEnumerable<int> ChildTraces { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public IEnumerable<DateTime> ModifiedAt { get; set; }
    public IEnumerable<bool> Results { get; set; }
    public IEnumerable<decimal> Billing { get; set; }

    public BusinessTraceDataDetail Detail { get; set; }
    public IEnumerable<BusinessTraceDataDetail> History { get; set; }

    [BigQuerySchema(ignore:true)]
    public BusinessTraceDataDetail PrivateDetail { get; set; }
  }

  public class BusinessTraceDataDetail
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsSuccesful { get; set; }

    [BigQuerySchema(ignore:true)]
    public string UpdatedById { get; set; }
  }
}

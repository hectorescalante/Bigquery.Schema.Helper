using BigQuery.Schema.Helper.Core;
using BigQuery.Schema.Helper.Core.Abstractions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using System.Collections.Concurrent;

namespace BigQuery.Schema.Helper.Infrastructure
{
  internal class BigQueryClientFactory : IBigQueryClientFactory
  {
    public static ConcurrentDictionary<string, BigQueryClient> Clients { get; set; } = new ConcurrentDictionary<string, BigQueryClient>();

    public BigQueryClient GetOrCreateClient(string projectId, GoogleCredential credential = null)
    {
      if (Clients.TryGetValue(projectId, out var client))
        return client;
      else
        return Clients.GetOrAdd(projectId, BigQueryClient.Create(projectId));
    }
  }
}

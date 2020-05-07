using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;

namespace BigQuery.Schema.Helper.Core
{
  interface IBigQueryClientFactory
  {
    BigQueryClient GetOrCreateClient(string projectId, GoogleCredential credential = null);
  }
}

using AutoFixture;
using AutoFixture.AutoMoq;
using BigQuery.Schema.Helper.Core;
using BigQuery.Schema.Helper.Core.Abstractions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace BigQuery.Schema.Helper.Test
{
  public class IntegrationTests
  {
    private readonly IFixture _autoFixture = new Fixture().Customize(new AutoMoqCustomization());

    [Fact]
    public async void TestBigQuerySchemaHelper_WithMocks_ShouldSuccess()
    {
      //Arrange

      var bigQuerySchemaHelperOptions = _autoFixture.Create<BigQuerySchemaHelperOptions>();
      var optionsMock = _autoFixture.Freeze<Mock<IOptionsSnapshot<BigQuerySchemaHelperOptions>>>();
      optionsMock
        .Setup(m => m.Value)
        .Returns(bigQuerySchemaHelperOptions);
      optionsMock
        .Setup(m => m.Get(It.IsAny<string>()))
        .Returns(bigQuerySchemaHelperOptions);

      var bigQueryClientMock = new Mock<BigQueryClient>();
      bigQueryClientMock
        .Setup(mock => mock.GetOrCreateDatasetAsync(It.IsAny<string>(), It.IsAny<GetDatasetOptions>(), It.IsAny<CreateDatasetOptions>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new BigQueryDataset(bigQueryClientMock.Object, new Dataset()));
      bigQueryClientMock
        .Setup(mock => mock.GetOrCreateTableAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TableSchema>(), It.IsAny<GetTableOptions>(), It.IsAny<CreateTableOptions>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new BigQueryTable(bigQueryClientMock.Object, new Table()));
      bigQueryClientMock
        .Setup(mock => mock.InsertRowsAsync(It.IsAny<TableReference>(), It.IsAny<IEnumerable<BigQueryInsertRow>>(), It.IsAny<InsertOptions>(), It.IsAny<CancellationToken>()));

      var bigQueryClientFactoryMock = _autoFixture.Freeze<Mock<IBigQueryClientFactory>>();
      bigQueryClientFactoryMock
        .Setup(mock => mock.GetOrCreateClient(It.IsAny<string>(), It.IsAny<GoogleCredential>()))
        .Returns(bigQueryClientMock.Object);
      
      //Act
      var sut = _autoFixture.Create<BigQuerySchemaHelper>();
      var table = await sut.GetOrCreateTableAsync<BusinessTraceData>();

      var trace = new BusinessTraceData(DateTime.Now);
      await sut.InsertRowAsync<BusinessTraceData>(trace);

      var traces = new List<BusinessTraceData>()
      {
        { new BusinessTraceData(DateTime.Now) },
        { new BusinessTraceData(DateTime.Now) },
        { new BusinessTraceData(DateTime.Now) },
        { new BusinessTraceData(DateTime.Now) },
        { new BusinessTraceData(DateTime.Now) },
        { new BusinessTraceData(DateTime.Now) }
      };
      await sut.InsertRowsAsync<BusinessTraceData>(traces);

      //Assert
      Assert.NotNull(trace);
      Assert.NotNull(traces);
    }
  }
}

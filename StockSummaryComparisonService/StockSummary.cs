using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using StockSummaryComparisonService.Models;

namespace StockSummaryComparisonService
{
    public static class StockSummary
    {
        [FunctionName("StockSummary")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["stockName"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.stockName;

            if (string.IsNullOrEmpty(name)) {
                return new BadRequestResult();
            }

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=storageaccount4471b6a6;AccountKey=dX2VUCuxC0EcRnyZ7Srg+XIKLLagAO30kkpcBcsv9bq91rG+h2FomX6EHP/IByNzKSxVdRIqq6phUYDQ3PAPnw==;EndpointSuffix=core.windows.net";

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("stocksinfo");

            var summary = GetSummary(table, name);

            if (summary == null) { 
                return new NotFoundResult(); ;
            }

            return new OkObjectResult(JsonConvert.SerializeObject(summary));
        }

        public static StockSummaryInfo GetSummary(CloudTable table, string name) {
            var rowKey = name;
            var rows = Helper.RetrieveRecordAsync(table, "closing", rowKey).Result;

            if (rows == null)
            {
                return null;
            }

            var summary = new StockSummaryInfo();
            summary.StockName = name;
            rows = Helper.RetrieveRecordAsync(table, "high", rowKey).Result;
            summary.high = rows.Price;

            rows = Helper.RetrieveRecordAsync(table, "market cap", rowKey).Result;
            summary.marketCap = rows.Price;

            rows = Helper.RetrieveRecordAsync(table, "price to earn ratio", rowKey).Result;
            summary.priceToEarnRatio = rows.Price;

            return summary;
        }
    }
}

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
using System.Collections.Generic;

namespace StockSummaryComparisonService
{
    public static class StockComparison
    {
        [FunctionName("StockComparison")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            StockNamesModel data = JsonConvert.DeserializeObject<StockNamesModel>(requestBody);


            if (data.stockNames == null || data.stockNames.Count <= 0)
            {
                return new BadRequestResult();
            }

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=storageaccount4471b6a6;AccountKey=dX2VUCuxC0EcRnyZ7Srg+XIKLLagAO30kkpcBcsv9bq91rG+h2FomX6EHP/IByNzKSxVdRIqq6phUYDQ3PAPnw==;EndpointSuffix=core.windows.net";

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("stocksinfo");


            var comparisonList = new StockComparisonInfo();
            foreach (var s in data.stockNames) {
                var info = StockSummary.GetSummary(table, s);
                comparisonList.StockSummaries.Add(info);
            }
            
            return new OkObjectResult(JsonConvert.SerializeObject(comparisonList));
        }

    }
}

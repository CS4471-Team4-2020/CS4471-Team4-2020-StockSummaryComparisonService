using Microsoft.WindowsAzure.Storage.Table;
using StockSummaryComparisonService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StockSummaryComparisonService
{
    public static class Helper
    {
        public static async Task<StocksInfo> RetrieveRecordAsync(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<StocksInfo>(partitionKey, rowKey);
            TableResult tableResult = await table.ExecuteAsync(tableOperation);
            return tableResult.Result as StocksInfo;
        }
    }
}

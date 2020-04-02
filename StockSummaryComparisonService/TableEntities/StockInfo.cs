using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockSummaryComparisonService.Models
{
    public class StocksInfo : TableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}

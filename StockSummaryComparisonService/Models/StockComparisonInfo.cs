using System;
using System.Collections.Generic;
using System.Text;

namespace StockSummaryComparisonService.Models
{
    public class StockComparisonInfo
    {
        public StockComparisonInfo() {
            StockSummaries = new List<StockSummaryInfo>();
        }

        public List<StockSummaryInfo> StockSummaries { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace StockSummaryComparisonService.Models
{
    public class StockSummaryInfo
    {
        public string StockName { get; set; }
        public double high { get; set; }
        public double marketCap { get; set; }
        public double priceToEarnRatio { get; set; }
    }
}

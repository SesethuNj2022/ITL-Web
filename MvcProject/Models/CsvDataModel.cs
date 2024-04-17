using System;

namespace MvcProject.Models
{
    public class CsvDataModel
    {
        public string FileName { get; set; } 
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public double CostPerItem { get; set; }
        public DateTime DateTime { get; set; }
        public string Line { get; set; } 
        public DateTime EarliestDate { get; set; }
        public DateTime LatestDate { get; set; }
        public double TotalCost { get; set; }
        public string HighestTotalCost { get; set; }
        public double AverageQuantity { get; set; }
    }
}

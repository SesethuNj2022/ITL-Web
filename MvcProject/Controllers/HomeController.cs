using MvcProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MvcProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UploadCsv()
        {
            var file = Request.Form.Files[0]; 
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file.";
                return View("Index");
            }

            List<CsvDataModel> dataModels = new List<CsvDataModel>();
            List<string> errors = new List<string>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                int lineNumber = 1; 
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');

                    if (values.Length != 4)
                    {
                        errors.Add($"Incorrect data format (Line {lineNumber}): {line}");
                        continue;
                    }

                    if (!DateTime.TryParseExact(values[3], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                    {
                        errors.Add($"Incorrect date format (Line {lineNumber}): {line}");
                        continue;
                    }

                    if (!int.TryParse(values[1], out int quantity) || !double.TryParse(values[2].Replace("R", ""), out double costPerItem))
                    {
                        errors.Add($"Incorrect data type (Line {lineNumber}): {line}");
                        continue;
                    }

                    var productCode = values[0];
                    if (dataModels.Any(x => x.ProductCode == productCode))
                    {
                        errors.Add($"Duplicate entry (Line {lineNumber}): {line}");
                        continue;
                    }

                    dataModels.Add(new CsvDataModel
                    {
                        ProductCode = productCode,
                        Quantity = quantity,
                        CostPerItem = costPerItem,
                        DateTime = dateTime,
                        Line = line
                    });

                    lineNumber++;
                }
            }

            if (errors.Any())
            {
                ViewBag.Errors = errors;
                return View("Index");
            }

            // Calculate metrics
            DateTime earliestDate = dataModels.Min(x => x.DateTime);
            DateTime latestDate = dataModels.Max(x => x.DateTime);
            double totalCost = dataModels.Sum(x => x.Quantity * x.CostPerItem);
            var highestTotalCost = dataModels
                .GroupBy(x => x.ProductCode)
                .Select(group => new
                {
                    ProductCode = group.Key,
                    TotalCost = group.Sum(item => item.Quantity * item.CostPerItem)
                })
                .OrderByDescending(x => x.TotalCost)
                .FirstOrDefault();
            double averageQuantity = dataModels.Average(x => x.Quantity);

            // Pass metrics to view
            ViewBag.EarliestDate = earliestDate;
            ViewBag.LatestDate = latestDate;
            ViewBag.TotalCost = totalCost;
            ViewBag.HighestTotalCost = highestTotalCost != null ? $"{highestTotalCost.ProductCode} (R {highestTotalCost.TotalCost})" : "No data";
            ViewBag.AverageQuantity = averageQuantity;

            return View("Index");
        }
    }
}

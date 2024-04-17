using MvcProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvDataController : ControllerBase
    {
        private List<CsvDataModel> _uploadedData = new List<CsvDataModel>();

        [HttpGet]
        public IActionResult GetCsvData()
        {
            if (_uploadedData.Any())
            {
                var lastUploadedData = _uploadedData.Last();
                var result = new
                {
                    HasData = true,
                    FileName = lastUploadedData.FileName,
                    EarliestDate = lastUploadedData.EarliestDate,
                    LatestDate = lastUploadedData.LatestDate,
                    TotalCost = lastUploadedData.TotalCost,
                    HighestTotalCost = lastUploadedData.HighestTotalCost,
                    AverageQuantity = lastUploadedData.AverageQuantity
                };
                return Ok(result);
            }
            else
            {
                return Ok(new { HasData = false });
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadCsvData([FromBody] CsvDataModel data)
        {
            if (data == null)
            {
                return BadRequest("Invalid CSV data.");
            }
            _uploadedData.Add(data); 
            return Ok();
        }
    }
}

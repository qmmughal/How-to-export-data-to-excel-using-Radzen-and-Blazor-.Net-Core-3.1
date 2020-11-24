using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExportOperations.Data;

namespace ExportOperations
{
    public partial class ExportSampleDbController : ExportController
    {
        private readonly SampleDbContext context;

        public ExportSampleDbController(SampleDbContext context)
        {
            this.context = context;
        }
        [HttpGet("/export/SampleDb/products/csv")]
        [HttpGet("/export/SampleDb/products/csv(fileName='{fileName}')")]
        public FileStreamResult ExportProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Products, Request.Query), fileName);
        }

        [HttpGet("/export/SampleDb/products/excel")]
        [HttpGet("/export/SampleDb/products/excel(fileName='{fileName}')")]
        public FileStreamResult ExportProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Products, Request.Query), fileName);
        }
    }
}

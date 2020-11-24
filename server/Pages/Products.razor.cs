using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.JSInterop;

namespace ExportOperations.Pages
{
    public partial class ProductsComponent
    {
        public async Task Export()
        {
            byte[] response = await SampleDb.ExportProductsToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "Id,ProductName,ProductPrice,ProductPicture" }, $"Products");
            await JSRuntime.InvokeAsync<object>("saveAsFile", $"MyFile.xlsx", Convert.ToBase64String(response));
        }
    }
}

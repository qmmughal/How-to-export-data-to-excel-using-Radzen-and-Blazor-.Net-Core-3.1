using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using ExportOperations.Data;

namespace ExportOperations
{
    public partial class SampleDbService
    {
        private readonly HttpClient httpClient;
        private readonly SampleDbContext context;
        private readonly NavigationManager navigationManager;

        public SampleDbService(HttpClient httpClient, SampleDbContext context, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task<byte[]> ExportProductsToExcel(Query query = null, string fileName = null)
        {
            var url = query != null ? query.ToUrl($"api/ortSampleDb/ExportProductsToExcel/export/sampledb/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sampledb/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')";
            url = navigationManager.BaseUri + url;
            var response = await httpClient.GetAsync(url);
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return fileBytes;
        }

        public async Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sampledb/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sampledb/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductsRead(ref IQueryable<Models.SampleDb.Product> items);

        public async Task<IQueryable<Models.SampleDb.Product>> GetProducts(Query query = null)
        {
            var items = context.Products.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductCreated(Models.SampleDb.Product item);
        partial void OnAfterProductCreated(Models.SampleDb.Product item);

        public async Task<Models.SampleDb.Product> CreateProduct(Models.SampleDb.Product product)
        {
            OnProductCreated(product);

            context.Products.Add(product);
            context.SaveChanges();

            OnAfterProductCreated(product);

            return product;
        }

        partial void OnProductDeleted(Models.SampleDb.Product item);
        partial void OnAfterProductDeleted(Models.SampleDb.Product item);

        public async Task<Models.SampleDb.Product> DeleteProduct(int? id)
        {
            var itemToDelete = context.Products
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductDeleted(itemToDelete);

            context.Products.Remove(itemToDelete);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw ex;
            }

            OnAfterProductDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnProductGet(Models.SampleDb.Product item);

        public async Task<Models.SampleDb.Product> GetProductById(int? id)
        {
            var items = context.Products
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.SampleDb.Product> CancelProductChanges(Models.SampleDb.Product item)
        {
            var entityToCancel = context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnProductUpdated(Models.SampleDb.Product item);
        partial void OnAfterProductUpdated(Models.SampleDb.Product item);

        public async Task<Models.SampleDb.Product> UpdateProduct(int? id, Models.SampleDb.Product product)
        {
            OnProductUpdated(product);

            var itemToUpdate = context.Products
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
            var entryToUpdate = context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(product);
            entryToUpdate.State = EntityState.Modified;
            context.SaveChanges();

            OnAfterProductUpdated(product);

            return product;
        }
    }
}

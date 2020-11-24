using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using ExportOperations.Models.SampleDb;

namespace ExportOperations.Data
{
  public partial class SampleDbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public SampleDbContext(DbContextOptions<SampleDbContext> options):base(options)
    {
    }

    public SampleDbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);



        this.OnModelBuilding(builder);
    }


    public DbSet<ExportOperations.Models.SampleDb.Product> Products
    {
      get;
      set;
    }
  }
}

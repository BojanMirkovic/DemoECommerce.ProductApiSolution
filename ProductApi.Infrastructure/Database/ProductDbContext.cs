using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.EntityModels;

namespace ProductApi.Infrastructure.Database
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<ProductModel> Products { get; set; }
    }
}

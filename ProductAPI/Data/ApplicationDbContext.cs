using Microsoft.EntityFrameworkCore;
using ProductAPI.Model;



namespace ProductAPI.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductVariant> ProductVariants { get; set; }
		public DbSet<Inventory> Inventory { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<ProductInventorySupplier> ProductInventorySupplier { get; set; }
		public DbSet<ProductInventoryLog> productInventoryLogs { get; set; }
		public DbSet<Sale> Sales { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
	}
}

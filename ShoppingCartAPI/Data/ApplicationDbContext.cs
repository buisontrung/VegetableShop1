
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Model;


namespace ShoppingCartAPI.Data
{
	public class ApplicationDbContext: DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
		{
		}
		public DbSet<ShoppingCarts> ShoppingCarts { get; set; }
	}
}

using Microsoft.EntityFrameworkCore;
using Order.Model;

namespace Order.Data
{
	public class ApplicationDbContext : DbContext
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Order.Model.Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Promotion> PromotionDetails { get; set;}
		public DbSet<Payment> Payments { get; set; }
		public DbSet<PaymentMethod> PaymentMethods { get; set; }
	}
}

using Authentication.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public new DbSet<ApplicationUser> Users { get; set; }
		public DbSet<Address> Addresses { get; set; }
	}
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Model
{
	[Table("AspNetUsers")]
	public class ApplicationUser : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
	}
}

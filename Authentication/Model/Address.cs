using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Model
{
	public class Address
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? UserNameAddress { get; set; }
		public string? PhoneNumberAddress { get; set; }
		public string? City { get; set; }	
		public string? District { get; set; }
		public string? WardsCommunes { get; set; }
		public string? AddressName { get; set; }
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual ApplicationUser? ApplicationUser { get; set; }
		public bool IsPrimary { get; set; }
	}
}

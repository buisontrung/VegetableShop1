using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{
	public class Review
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public string? UserId { get; set; }	
		public string? Content { get; set; }	
		public int Rating { get; set; }
		public DateTime CreateAt { get; set; } = DateTime.Now;
		public DateTime UpdateAt { get; set; }
		public Product? product { get; set; }

	}
}

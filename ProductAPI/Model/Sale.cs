using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{
	public class Sale
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? SaleName { get; set; }
		public decimal DiscountPercentage { get; set; } // % giảm giá
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
	}
}

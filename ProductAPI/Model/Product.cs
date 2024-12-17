
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ProductAPI.Model
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string? ProductName { get; set; }

		public string? Description { get; set; }

		[Range(0, double.MaxValue)]
		public decimal? Price { get; set; }

		public string? ImageUrl { get; set; } 

		public bool IsActive { get; set; } = true;  

		public int? ProductCategoryId { get; set; }  


		public virtual ProductCategory? ProductCategory { get; set; } 
	
		public virtual ICollection<Review>? Reviews { get; set; }	
		public virtual ICollection<ProductImage>? Images { get; set; }
		public virtual ICollection<ProductVariant>? Variants { get; set; }

	}
}

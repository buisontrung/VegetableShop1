
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Model
{
	public class ProductCategory
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? ProductCategoryName { get; set; }
		public string? Description { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public string? ImageUrl { get; set; }
		public bool IsActive { get; set; } = true;
		public virtual ICollection<Product>? Products { get; set; }
		public ProductCategory() {
			Products = new HashSet<Product>();
		}

	}
}

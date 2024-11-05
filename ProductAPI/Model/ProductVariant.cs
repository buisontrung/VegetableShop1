using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{

	public class ProductVariant
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public string? VariantName { get; set; } // Ví dụ: "Hộp 250g", "Hộp 400g"
		public decimal UnitPrice { get; set; }

		public Product? Product { get; set; }
		public ICollection<ProductInventorySupplier>? ProductInventorySuppliers { get; set; }
	}
}

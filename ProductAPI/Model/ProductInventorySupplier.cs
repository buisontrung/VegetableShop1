using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Model
{
	[Table("ProductInventorySupplier")]
	public class ProductInventorySupplier
	{
		public int Id { get; set; }
		public int ProductVariantId { get; set; }
		public ProductVariant? ProductVariant { get; set; }

		public int SupplierId { get; set; }
		public Supplier? Supplier { get; set; }
		public int InventoryId { get; set; }
		public Inventory? Inventory { get; set; }
		public int Quantity { get; set; }

	} 
}

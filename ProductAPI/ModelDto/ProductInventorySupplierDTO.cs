using ProductAPI.Model;

namespace ProductAPI.ModelDto
{
	public class ProductInventorySupplierDTO
	{
		public int Id { get; set; }
		public int ProductVariantId { get; set; }
	

		public int SupplierId { get; set; }

		public int InventoryId { get; set; }

		public int Quantity { get; set; }
	}
}

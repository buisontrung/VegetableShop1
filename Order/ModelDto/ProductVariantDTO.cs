

namespace Order.ModelDto
{
	public class ProductVariantDTO
	{
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public string? VariantName { get; set; } // Ví dụ: "Hộp 250g", "Hộp 400g"
		public decimal UnitPrice { get; set; }

		public ICollection<ProductInventorySupplierDTO>? ProductInventorySuppliers { get; set; }
	}
}

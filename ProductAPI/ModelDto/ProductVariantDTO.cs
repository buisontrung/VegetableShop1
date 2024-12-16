using ProductAPI.Model;

namespace ProductAPI.ModelDto
{
	public class ProductVariantDTO
	{
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public string? VariantName { get; set; } // Ví dụ: "Hộp 250g", "Hộp 400g"
		public decimal UnitPrice { get; set; }
		public int? SaleId { get; set; }
		public decimal? PriceSale { get; set; }
		public SaleDTO Sale { get; set; }
		public ICollection<ProductInventorySupplierDTO>? ProductInventorySuppliers { get; set; }
	}
}

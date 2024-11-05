namespace ProductAPI.ModelDto
{
	public class ProductDetail
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public bool IsActive { get; set; }
		public string MeasurementUnit { get; set; }
		public decimal Price { get; set; }
		public decimal PriceSale { get; set; }
		public int ProductCategoryId { get; set; }
		public string ProductCategoryName { get; set; }
		public int VariantId { get; set; }
		public int ProductId { get; set; }
		public decimal UnitPrice { get; set; }
		public string VariantName { get; set; }
		public int Quantity { get; set; }
	}
}

namespace ProductAPI.ModelDto
{
	public class SupplierDTO
	{
		public int SupplierId { get; set; }
		public string? SupplierName { get; set; }
		public string? ContactInfo { get; set; }
		public string? PhoneNumber { get; set; }
		public ICollection<ProductInventorySupplierDTO>? Suppliers { get; set; }
	}
}

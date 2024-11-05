namespace ProductAPI.Model
{
	public class Supplier
	{
		public int SupplierId { get; set; }
		public string? SupplierName { get; set; }
		public string? ContactInfo { get; set; }
		public string? PhoneNumber { get; set; }	
		public virtual ICollection<ProductInventorySupplier>? ProductInventorySuppliers { get; set; }	
	}
}

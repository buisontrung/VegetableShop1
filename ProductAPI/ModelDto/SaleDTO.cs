namespace ProductAPI.ModelDto
{
	public class SaleDTO
	{
		public int Id { get; set; }
		public string? SaleName { get; set; }
		public decimal DiscountPercentage { get; set; } // % giảm giá
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}

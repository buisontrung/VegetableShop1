namespace ProductAPI.ModelDto
{
	public class ProductCategoryDTO
	{
		public int Id { get; set; }
		public string? ProductCategoryName { get; set; } = string.Empty;
		public string? Description { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public string? ImageUrl { get; set; }
		public bool IsActive { get; set; } = true;
	}
}

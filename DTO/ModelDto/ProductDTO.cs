namespace DTO.ModelDto
{
	public class ProductDTO
	{
		public int? Id { get; set; }
		public string? ProductName { get; set; }
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public IFormFile? ImageFile { get; set; }

		public string? ImageUrl { get; set; }
		public bool IsActive { get; set; } = true;
		public int? ProductCategoryId { get; set; }
		public double? AverageRating { get; set; }
		public ICollection<ReviewsDTO>? Reviews { get; set; }
		public ICollection<ProductImageDTO>? productImageDTOs { get; set; }
		public ProductCategoryDTO? productCategoryDTO { get; set; }
		public ICollection<ProductVariantDTO>? productVariantDTOs { get; set; }

	}
}

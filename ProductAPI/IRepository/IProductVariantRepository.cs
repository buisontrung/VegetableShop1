using ProductAPI.ModelDto;

namespace ProductAPI.IRepository
{
	public interface IProductVariantRepository
	{
		Task<List<ProductVariantDTO>> GetAllProductVariantsAsync();
		Task<ProductVariantDTO?> GetProductVariantByIdAsync(int id);
		Task<ProductVariantDTO> CreateProductVariantAsync(ProductVariantDTO productVariantDto);
		Task<bool> UpdateProductVariantAsync(ProductVariantDTO productVariantDto);
		Task<bool> DeleteProductVariantAsync(int id);
	}
}

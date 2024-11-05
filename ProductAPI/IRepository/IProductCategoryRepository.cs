
using ProductAPI.ModelDto;

namespace ProductAPI.IRepository
{
	public interface IProductCategoryRepository
	{
		Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategoriesAsync();
		Task<ProductCategoryDTO?> GetProductCategoryByIdAsync(int id);
		Task AddProductCategoryAsync(ProductCategoryDTO category);
		Task UpdateProductCategoryAsync(ProductCategoryDTO category);
		Task DeleteProductCategoryAsync(int id);
	}
}

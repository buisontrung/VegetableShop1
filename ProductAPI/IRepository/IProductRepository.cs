
using ProductAPI.Model;
using ProductAPI.ModelDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAPI.IRepository
{
	public interface IProductRepository
	{
		Task<ProductVariantDTO> GetAllProductVariantName(string productName, string variantName);
		Task<IEnumerable<ProductVariantDTO>> GetAllProductVariantsName(string productName);
		Task<IEnumerable<ResponseProductDialog>> GetAllProductsName(string categoryName);
		Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
		Task<ProductDTO?> GetProductByIdAsync(int id);
		Task<ProductDTO?> GetProductByVarianIdAsync(int id);
		Task<IEnumerable<ProductDTO?>> GetProductByCategoryIdAsync(int categoryId);
		Task <ProductDTO> AddProductAsync(ProductDTO productDto,ICollection<IFormFile> images);
		Task UpdateProductAsync(ProductDTO productDto);
		Task DeleteProductAsync(int id);
		Task<IEnumerable<ProductDTO>> GetProductsByOrder(string orderType,int pageSize ,int pageIndex,int categoryId);
		Task <int> CountProductByCategoryId(int categoryId);
		Task<IEnumerable<ProductDTO?>> GetProductsByVariantIdsAsync(List<int> varianIds);
	}
}

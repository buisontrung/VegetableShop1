


using DTO.ModelDto;

namespace ShoppingCartAPI.IServices
{
	public interface IProductService
	{
		public Task<ProductDTO?> GetProductByIdAsync(int productId);
	}
}

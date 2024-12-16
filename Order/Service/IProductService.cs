
using Order.ModelDto;

namespace Order.IServices
{
	public interface IProductService
	{
		public Task<ProductDTO?> GetProductByIdAsync(int productId);
	}
}

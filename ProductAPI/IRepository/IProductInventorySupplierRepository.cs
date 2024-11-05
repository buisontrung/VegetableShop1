using ProductAPI.ModelDto;

namespace ProductAPI.IRepository
{
	public interface IProductInventorySupplierRepository
	{
		Task<IEnumerable<ProductInventorySupplierDTO>> GetAllAsync();
		Task<ProductInventorySupplierDTO?> GetByIdAsync(int id);
		Task<ProductInventorySupplierDTO> AddAsync(ProductInventorySupplierDTO productInventorySupplierDto);
		Task<ProductInventorySupplierDTO?> UpdateAsync(int id, ProductInventorySupplierDTO productInventorySupplierDto);
		Task<bool> DeleteAsync(int id);
	}
}

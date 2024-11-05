using ProductAPI.ModelDto;

namespace ProductAPI.IRepository
{
	public interface ISupplierRepository
	{
		Task<List<SupplierDTO>> GetAllSuppliersAsync();
		Task<SupplierDTO?> GetSupplierByIdAsync(int id);
		Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplierDto);
		Task<bool> UpdateSupplierAsync(SupplierDTO supplierDto);
		Task<bool> DeleteSupplierAsync(int id);
	}
}

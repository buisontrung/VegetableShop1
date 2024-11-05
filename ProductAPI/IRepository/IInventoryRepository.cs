using ProductAPI.ModelDto;

namespace ProductAPI.IRepository
{
	public interface IInventoryRepository
	{
		Task<List<InventoryDTO>> GetAllInventoriesAsync();
		Task<InventoryDTO?> GetInventoryByIdAsync(int id);
		Task<InventoryDTO> CreateInventoryAsync(InventoryDTO inventoryDto);
		Task<bool> UpdateInventoryAsync(InventoryDTO inventoryDto);
		Task<bool> DeleteInventoryAsync(int id);
	}
}

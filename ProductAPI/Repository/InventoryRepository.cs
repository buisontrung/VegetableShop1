using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.IRepository;
using ProductAPI.Model;
using ProductAPI.ModelDto;

namespace ProductAPI.Repository
{
	public class InventoryRepository : IInventoryRepository
	{
		private readonly ApplicationDbContext _context; // Đổi DbContext thành tên cụ thể của bạn, ví dụ: ProductDbContext

		public InventoryRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		// Lấy tất cả Inventories
		public async Task<List<InventoryDTO>> GetAllInventoriesAsync()
		{
			return await _context.Inventory
				.Select(i => new InventoryDTO
				{
					Id = i.Id,
					Location = i.Location,
					InventoryName = i.InventoryName
				})
				.ToListAsync();
		}

		// Lấy Inventory theo Id
		public async Task<InventoryDTO?> GetInventoryByIdAsync(int id)
		{
			return await _context.Inventory
				.Where(i => i.Id == id)
				.Select(i => new InventoryDTO
				{
					Id = i.Id,
					Location = i.Location,
					InventoryName = i.InventoryName
				})
				.FirstOrDefaultAsync();
		}

		// Tạo mới Inventory
		public async Task<InventoryDTO> CreateInventoryAsync(InventoryDTO inventoryDto)
		{
			var inventory = new Inventory
			{
				Location = inventoryDto.Location,
				InventoryName = inventoryDto.InventoryName
			};

			_context.Inventory.Add(inventory);
			await _context.SaveChangesAsync();

			// Cập nhật Id của DTO sau khi lưu để trả về bản ghi vừa tạo
			inventoryDto.Id = inventory.Id;
			return inventoryDto;
		}

		// Cập nhật Inventory
		public async Task<bool> UpdateInventoryAsync(InventoryDTO inventoryDto)
		{
			var inventory = await _context.Set<Inventory>().FindAsync(inventoryDto.Id);

			if (inventory == null)
			{
				return false;
			}

			inventory.Location = inventoryDto.Location;
			inventory.InventoryName = inventoryDto.InventoryName;

			_context.Inventory.Update(inventory);
			await _context.SaveChangesAsync();
			return true;
		}

		// Xóa Inventory
		public async Task<bool> DeleteInventoryAsync(int id)
		{
			var inventory = await _context.Set<Inventory>().FindAsync(id);

			if (inventory == null)
			{
				return false;
			}

			_context.Set<Inventory>().Remove(inventory);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}

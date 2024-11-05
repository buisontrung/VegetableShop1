using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.IRepository;
using ProductAPI.ModelDto;

namespace ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryRepository _inventoryRepository;

		public InventoryController(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;
		}

		// Lấy tất cả Inventories
		[HttpGet]
		public async Task<ActionResult<List<InventoryDTO>>> GetAllInventories()
		{
			var inventories = await _inventoryRepository.GetAllInventoriesAsync();
			return Ok(inventories);
		}

		// Lấy Inventory theo Id
		[HttpGet("{id}")]
		public async Task<ActionResult<InventoryDTO>> GetInventoryById(int id)
		{
			var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
			if (inventory == null)
			{
				return NotFound();
			}
			return Ok(inventory);
		}

		// Tạo mới Inventory
		[HttpPost]
		public async Task<ActionResult<InventoryDTO>> CreateInventory([FromBody] InventoryDTO inventoryDto)
		{
			if (inventoryDto == null)
			{
				return BadRequest("Invalid inventory data.");
			}

			var createdInventory = await _inventoryRepository.CreateInventoryAsync(inventoryDto);
			return CreatedAtAction(nameof(GetInventoryById), new { id = createdInventory.Id }, createdInventory);
		}

		// Cập nhật Inventory
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateInventory(int id, [FromBody] InventoryDTO inventoryDto)
		{
			if (inventoryDto == null || inventoryDto.Id != id)
			{
				return BadRequest("Inventory ID mismatch.");
			}

			var updated = await _inventoryRepository.UpdateInventoryAsync(inventoryDto);
			if (!updated)
			{
				return NotFound();
			}

			return NoContent();
		}

		// Xóa Inventory
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteInventory(int id)
		{
			var deleted = await _inventoryRepository.DeleteInventoryAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}

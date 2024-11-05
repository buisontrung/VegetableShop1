using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.IRepository;
using ProductAPI.ModelDto;

namespace ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductSupplierInventoryController : ControllerBase
	{
		private readonly IProductInventorySupplierRepository _repository;

		public ProductSupplierInventoryController(IProductInventorySupplierRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductInventorySupplierDTO>>> GetAll()
		{
			var result = await _repository.GetAllAsync();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductInventorySupplierDTO>> GetById(int id)
		{
			var result = await _repository.GetByIdAsync(id);
			if (result == null)
				return NotFound();

			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ProductInventorySupplierDTO>> Add(ProductInventorySupplierDTO dto)
		{
			var result = await _repository.AddAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, ProductInventorySupplierDTO dto)
		{
			var result = await _repository.UpdateAsync(id, dto);
			if (result == null)
				return NotFound();

			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var success = await _repository.DeleteAsync(id);
			if (!success)
				return NotFound();

			return NoContent();
		}
	}
}

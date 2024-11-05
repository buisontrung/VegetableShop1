using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ProductAPI.IRepository;
using ProductAPI.ModelDto;

namespace ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductCategoryController : ControllerBase
	{
		private readonly IProductCategoryRepository _repository;

		public ProductCategoryController(IProductCategoryRepository repository)
		{
			_repository = repository;
		}

		// GET: api/ProductCategory

		[HttpGet("getall")]
		public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductCategories()
		{
			var categories = await _repository.GetAllProductCategoriesAsync();
			return Ok(categories);
		}

		// GET: api/ProductCategory/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductCategoryDTO>> GetProductCategory(int id)
		{
			var category = await _repository.GetProductCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return Ok(category);
		}

		// POST: api/ProductCategory
		[HttpPost("create")]
		public async Task<ActionResult> PostProductCategory(ProductCategoryDTO dto)
		{
			if (dto == null)
			{
				return BadRequest("ProductCategoryDTO cannot be null.");
			}

			await _repository.AddProductCategoryAsync(dto);
			return CreatedAtAction(nameof(GetProductCategory), new { id = dto.Id }, dto);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> PutProductCategory(int id, ProductCategoryDTO dto)
		{
			if (id != dto.Id)
			{
				return BadRequest("ID mismatch.");
			}

			var category = await _repository.GetProductCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			await _repository.UpdateProductCategoryAsync(dto);
			return NoContent();
		}

		// DELETE: api/ProductCategory/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProductCategory(int id)
		{
			var category = await _repository.GetProductCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			await _repository.DeleteProductCategoryAsync(id);
			return NoContent();
		}
	}
}

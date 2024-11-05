using Microsoft.AspNetCore.Mvc;
using ProductAPI.IRepository;
using ProductAPI.ModelDto;
using ProductAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductVariantController : ControllerBase
	{
		private readonly IProductVariantRepository _productVariantRepository;

		public ProductVariantController(IProductVariantRepository productVariantRepository)
		{
			_productVariantRepository = productVariantRepository;
		}

		// Lấy tất cả ProductVariants
		[HttpGet]
		public async Task<ActionResult<List<ProductVariantDTO>>> GetAllProductVariants()
		{
			var productVariants = await _productVariantRepository.GetAllProductVariantsAsync();
			return Ok(productVariants);
		}

		// Lấy ProductVariant theo Id
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductVariantDTO>> GetProductVariantById(int id)
		{
			var productVariant = await _productVariantRepository.GetProductVariantByIdAsync(id);
			if (productVariant == null)
			{
				return NotFound();
			}
			return Ok(productVariant);
		}

		// Tạo mới ProductVariant
		[HttpPost]
		public async Task<ActionResult<ProductVariantDTO>> CreateProductVariant([FromBody] ProductVariantDTO productVariantDto)
		{
			if (productVariantDto == null)
			{
				return BadRequest("Invalid product variant data.");
			}

			var createdProductVariant = await _productVariantRepository.CreateProductVariantAsync(productVariantDto);
			return CreatedAtAction(nameof(GetProductVariantById), new { id = createdProductVariant.Id }, createdProductVariant);
		}

		// Cập nhật ProductVariant
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateProductVariant(int id, [FromBody] ProductVariantDTO productVariantDto)
		{
			if (productVariantDto == null || productVariantDto.Id != id)
			{
				return BadRequest("Product variant ID mismatch.");
			}

			var updated = await _productVariantRepository.UpdateProductVariantAsync(productVariantDto);
			if (!updated)
			{
				return NotFound();
			}

			return NoContent();
		}

		// Xóa ProductVariant
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProductVariant(int id)
		{
			var deleted = await _productVariantRepository.DeleteProductVariantAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}

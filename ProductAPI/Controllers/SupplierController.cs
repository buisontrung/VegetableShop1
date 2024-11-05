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
	public class SupplierController : ControllerBase
	{
		private readonly ISupplierRepository _supplierRepository;

		public SupplierController(ISupplierRepository supplierRepository)
		{
			_supplierRepository = supplierRepository;
		}

		// Lấy tất cả Suppliers
		[HttpGet]
		public async Task<ActionResult<List<SupplierDTO>>> GetAllSuppliers()
		{
			var suppliers = await _supplierRepository.GetAllSuppliersAsync();
			return Ok(suppliers);
		}

		// Lấy Supplier theo Id
		[HttpGet("{id}")]
		public async Task<ActionResult<SupplierDTO>> GetSupplierById(int id)
		{
			var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
			if (supplier == null)
			{
				return NotFound();
			}
			return Ok(supplier);
		}

		// Tạo mới Supplier
		[HttpPost]
		public async Task<ActionResult<SupplierDTO>> CreateSupplier([FromBody] SupplierDTO supplierDto)
		{
			if (supplierDto == null)
			{
				return BadRequest("Invalid supplier data.");
			}

			var createdSupplier = await _supplierRepository.CreateSupplierAsync(supplierDto);
			return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.SupplierId }, createdSupplier);
		}

		// Cập nhật Supplier
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO supplierDto)
		{
			if (supplierDto == null || supplierDto.SupplierId != id)
			{
				return BadRequest("Supplier ID mismatch.");
			}

			var updated = await _supplierRepository.UpdateSupplierAsync(supplierDto);
			if (!updated)
			{
				return NotFound();
			}

			return NoContent();
		}

		// Xóa Supplier
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteSupplier(int id)
		{
			var deleted = await _supplierRepository.DeleteSupplierAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}

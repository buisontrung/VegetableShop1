using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.IRepository;
using ProductAPI.Model;
using ProductAPI.ModelDto;

namespace ProductAPI.Repository
{
	public class ProductInventorySupplierRepository : IProductInventorySupplierRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductInventorySupplierRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProductInventorySupplierDTO>> GetAllAsync()
		{
			return await _context.ProductInventorySupplier
				.Select(pis => new ProductInventorySupplierDTO
				{
					Id = pis.Id,
					ProductVariantId = pis.ProductVariantId,
					InventoryId = pis.InventoryId,
					Quantity = pis.Quantity
				})
				.ToListAsync();
		}

		public async Task<ProductInventorySupplierDTO?> GetByIdAsync(int id)
		{
			var entity = await _context.ProductInventorySupplier.FindAsync(id);
			if (entity == null) return null;

			return new ProductInventorySupplierDTO
			{
				Id = entity.Id,
				ProductVariantId = entity.ProductVariantId,
				InventoryId = entity.InventoryId,
				Quantity = entity.Quantity
			};
		}

		public async Task<ProductInventorySupplierDTO> AddAsync(ProductInventorySupplierDTO productInventorySupplierDto)
		{
			var entity = new ProductInventorySupplier
			{
				ProductVariantId = productInventorySupplierDto.ProductVariantId,
				InventoryId = productInventorySupplierDto.InventoryId,
				Quantity = productInventorySupplierDto.Quantity
			};

			_context.ProductInventorySupplier.Add(entity);
			await _context.SaveChangesAsync();

			productInventorySupplierDto.Id = entity.Id;
			return productInventorySupplierDto;
		}

		public async Task<ProductInventorySupplierDTO?> UpdateAsync(int id, ProductInventorySupplierDTO productInventorySupplierDto)
		{
			var entity = await _context.ProductInventorySupplier.FindAsync(id);
			if (entity == null) return null;

			entity.ProductVariantId = productInventorySupplierDto.ProductVariantId;

			entity.InventoryId = productInventorySupplierDto.InventoryId;
			entity.Quantity = productInventorySupplierDto.Quantity;

			_context.ProductInventorySupplier.Update(entity);
			await _context.SaveChangesAsync();

			return productInventorySupplierDto;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var entity = await _context.ProductInventorySupplier.FindAsync(id);
			if (entity == null) return false;

			_context.ProductInventorySupplier.Remove(entity);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}

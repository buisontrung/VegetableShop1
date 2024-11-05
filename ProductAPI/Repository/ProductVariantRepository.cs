using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.IRepository;
using ProductAPI.Model; // Namespace chứa các entity của bạn
using ProductAPI.ModelDto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Repository
{
	public class ProductVariantRepository : IProductVariantRepository
	{
		private readonly ApplicationDbContext _context; // Thay DbContext bằng tên cụ thể của bạn, ví dụ: ProductDbContext

		public ProductVariantRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		// Lấy tất cả ProductVariants
		public async Task<List<ProductVariantDTO>> GetAllProductVariantsAsync()
		{
			return await _context.ProductVariants
				.Include(pv => pv.Product)
				.Include(pv => pv.ProductInventorySuppliers)
				.Select(pv => new ProductVariantDTO
				{
					Id = pv.Id,
					ProductId = pv.ProductId,
					VariantName = pv.VariantName,
					UnitPrice = pv.UnitPrice,
					
					ProductInventorySuppliers = pv.ProductInventorySuppliers.Select(pis => new ProductInventorySupplierDTO
					{
						Id = pis.Id,
						Quantity = pis.Quantity,
						// Add other necessary fields for ProductInventorySupplierDTO
					}).ToList()
				})
				.ToListAsync();
		}

		// Lấy ProductVariant theo Id
		public async Task<ProductVariantDTO?> GetProductVariantByIdAsync(int id)
		{
			return await _context.ProductVariants
				.Where(pv => pv.Id == id)
				.Include(pv => pv.Product)
				.Include(pv => pv.ProductInventorySuppliers)
				.Select(pv => new ProductVariantDTO
				{
					Id = pv.Id,
					ProductId = pv.ProductId,
					VariantName = pv.VariantName,
					UnitPrice = pv.UnitPrice,
					
					ProductInventorySuppliers = pv.ProductInventorySuppliers.Select(pis => new ProductInventorySupplierDTO
					{
						Id = pis.Id,
						Quantity = pis.Quantity,
						// Add other necessary fields for ProductInventorySupplierDTO
					}).ToList()
				})
				.FirstOrDefaultAsync();
		}

		// Tạo mới ProductVariant
		public async Task<ProductVariantDTO> CreateProductVariantAsync(ProductVariantDTO productVariantDto)
		{
			var productVariant = new ProductVariant
			{
				ProductId = productVariantDto.ProductId,
				VariantName = productVariantDto.VariantName,
				UnitPrice = productVariantDto.UnitPrice
			};

			_context.ProductVariants.Add(productVariant);
			await _context.SaveChangesAsync();

			productVariantDto.Id = productVariant.Id;
			return productVariantDto;
		}

		// Cập nhật ProductVariant
		public async Task<bool> UpdateProductVariantAsync(ProductVariantDTO productVariantDto)
		{
			var productVariant = await _context.Set<ProductVariant>().FindAsync(productVariantDto.Id);

			if (productVariant == null)
			{
				return false;
			}

			productVariant.VariantName = productVariantDto.VariantName;
			productVariant.UnitPrice = productVariantDto.UnitPrice;
			productVariant.ProductId = productVariantDto.ProductId;

			_context.ProductVariants.Update(productVariant);
			await _context.SaveChangesAsync();
			return true;
		}

		// Xóa ProductVariant
		public async Task<bool> DeleteProductVariantAsync(int id)
		{
			var productVariant = await _context.Set<ProductVariant>().FindAsync(id);

			if (productVariant == null)
			{
				return false;
			}

			_context.ProductVariants.Remove(productVariant);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}

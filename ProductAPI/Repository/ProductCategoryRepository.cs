using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.IRepository;
using ProductAPI.Model;
using ProductAPI.ModelDto;

namespace ProductAPI.Repository
{
	public class ProductCategoryRepository : IProductCategoryRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductCategoryRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategoriesAsync()
		{
			return await _context.ProductCategories
				.Select(c => new ProductCategoryDTO
				{
					Id = c.Id,
					ProductCategoryName = c.ProductCategoryName,
					Description = c.Description,
					CreatedDate = c.CreatedDate,
					ImageUrl = c.ImageUrl,
					IsActive = c.IsActive
				})
				.ToListAsync();
		}

		public async Task<ProductCategoryDTO?> GetProductCategoryByIdAsync(int id)
		{
			return await _context.ProductCategories
				.Where(c => c.Id == id)
				.Select(c => new ProductCategoryDTO
				{
					Id = c.Id,
					ProductCategoryName = c.ProductCategoryName,
					Description = c.Description,
					CreatedDate = c.CreatedDate,
					ImageUrl = c.ImageUrl,
					IsActive = c.IsActive
				})
				.FirstOrDefaultAsync();
		}

		public async Task AddProductCategoryAsync(ProductCategoryDTO dto)
		{
			var category = new ProductCategory
			{
				ProductCategoryName = dto.ProductCategoryName,
				Description = dto.Description,
				CreatedDate = dto.CreatedDate,
				ImageUrl = dto.ImageUrl,
				IsActive = dto.IsActive
			};

			_context.ProductCategories.Add(category);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductCategoryAsync(ProductCategoryDTO dto)
		{
			var category = await _context.ProductCategories.FindAsync(dto.Id);
			if (category != null)
			{
				category.ProductCategoryName = dto.ProductCategoryName;
				category.Description = dto.Description;
				category.CreatedDate = dto.CreatedDate;
				category.ImageUrl = dto.ImageUrl;
				category.IsActive = dto.IsActive;

				_context.ProductCategories.Update(category);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteProductCategoryAsync(int id)
		{
			var category = await _context.ProductCategories.FindAsync(id);
			if (category != null)
			{
				_context.ProductCategories.Remove(category);
				await _context.SaveChangesAsync();
			}
		}
	}

}

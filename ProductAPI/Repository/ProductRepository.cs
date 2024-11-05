using Microsoft.EntityFrameworkCore;

using ProductAPI.Model;
using ProductAPI.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductAPI.Data;
using ProductAPI.ModelDto;
using ProductAPI.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;

namespace ProductAPI.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IBlobStorageService _blobStorageService;

		public ProductRepository(ApplicationDbContext context,IBlobStorageService blobStorageService)
		{
			_context = context;
			_blobStorageService = blobStorageService;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
		{
			var products = await _context.Products.Include(p => p.Images).Include(p=>p.ProductCategory).ToListAsync();

			return products.Select(p => new ProductDTO
			{
				Id = p.Id,
				ProductName = p.ProductName,
				Description = p.Description,
				Price = p.Price,
				ImageUrl = p.ImageUrl, // Primary image URL if available
				IsActive = p.IsActive,
				ProductCategoryId = p.ProductCategoryId,
				productCategoryDTO = new ProductCategoryDTO // Change to single mapping
				{
					Id = p.ProductCategory.Id,
					ProductCategoryName = p.ProductCategory.ProductCategoryName,
				},
				productImageDTOs = p.Images?.Select(img => new ProductImageDTO
				{
					Id = img.Id,
					ImageUrl = img.ImageUrl,
					ProductId = img.ProductId
				}).ToList()
			}) ;
		}


		public async Task<ProductDTO?> GetProductByIdAsync(int id)
		{
			var product = await _context.Products.Include(p=>p.Images)
				.Include(p => p.ProductCategory)
				.Include(p=>p.Variants)
					.ThenInclude(p=>p.ProductInventorySuppliers)
				.FirstOrDefaultAsync(p=>p.Id ==id);
			if (product == null) return null;

			return new ProductDTO
			{
				Id = product.Id,
				ProductName = product.ProductName,
				Description = product.Description,
				Price = product.Price,

				ImageUrl = product.ImageUrl,
				IsActive = product.IsActive,
				productVariantDTOs = product.Variants?.Select(a => new ProductVariantDTO
				{
					Id = a.Id,
					ProductId = a.ProductId,
					UnitPrice = a.UnitPrice,
					VariantName = a.VariantName,
					ProductInventorySuppliers= a.ProductInventorySuppliers.Select(a => new ProductInventorySupplierDTO{
						Quantity = a.Quantity,
					}).ToList()
				}).ToList(),
				ProductCategoryId = product.ProductCategoryId,
				productCategoryDTO = new ProductCategoryDTO // Change to single mapping
				{
					Id = product.ProductCategory.Id,
					ProductCategoryName = product.ProductCategory.ProductCategoryName,
				},
				productImageDTOs = product.Images?.Select(img => new ProductImageDTO
				{
					Id = img.Id,
					ImageUrl = img.ImageUrl,
					ProductId = img.ProductId
				}).ToList()
			};
		}
		public async Task<ProductDTO?> GetProductByVarianIdAsync(int varianId)
		{
			var varian = await _context.ProductVariants.Include(v => v.ProductInventorySuppliers).FirstOrDefaultAsync(x => x.Id == varianId);
			if (varian == null)
			{
				return null;
			}
			var product = await _context.Products.Include(p => p.Images)
				.Include(p => p.ProductCategory)
				.FirstOrDefaultAsync(p => p.Id == varian.ProductId);
			if (product == null) return null;
			var variantDTO = new ProductVariantDTO
			{
				Id = varian.Id,
				ProductId = varian.ProductId,
				UnitPrice = varian.UnitPrice,
				VariantName = varian.VariantName,
				ProductInventorySuppliers = varian.ProductInventorySuppliers.Select(supplier => new ProductInventorySupplierDTO
				{
					Quantity = supplier.Quantity,
				}).ToList()
			};
			return new ProductDTO
			{
				Id = product.Id,
				ProductName = product.ProductName,
				Description = product.Description,
				Price = product.Price,

				ImageUrl = product.ImageUrl,
				IsActive = product.IsActive,
				productVariantDTOs = product.Variants?.Select(a => new ProductVariantDTO
				{
					Id = a.Id,
					ProductId = a.ProductId,
					UnitPrice = a.UnitPrice,
					VariantName = a.VariantName,
					ProductInventorySuppliers = a.ProductInventorySuppliers.Select(a => new ProductInventorySupplierDTO
					{
						Quantity = a.Quantity,
					}).ToList()
				}).ToList(),
				ProductCategoryId = product.ProductCategoryId,
				productCategoryDTO = new ProductCategoryDTO // Change to single mapping
				{
					Id = product.ProductCategory.Id,
					ProductCategoryName = product.ProductCategory.ProductCategoryName,
				},
				productImageDTOs = product.Images?.Select(img => new ProductImageDTO
				{
					Id = img.Id,
					ImageUrl = img.ImageUrl,
					ProductId = img.ProductId
				}).ToList()
			};
		}


		public async Task<IEnumerable<ProductDTO?>> GetProductByCategoryIdAsync(int categoryId)
		{
			var products = await _context.Products
				.Where(p => p.ProductCategoryId == categoryId)
				.Select(p => new ProductDTO
				{
					Id = p.Id,
					ProductName = p.ProductName,
					Description = p.Description,
					Price = p.Price,

					ImageUrl = p.ImageUrl,
					IsActive = p.IsActive,
					ProductCategoryId = p.ProductCategoryId
				})
				.ToListAsync();

			return products;
		}

		public async Task<ProductDTO> AddProductAsync(ProductDTO productDto, ICollection<IFormFile> images)
		{
			CloudBlobContainer container = await _blobStorageService.GetCloudBlogContainer();

			// Tạo đối tượng Product
			var product = new Product
			{
				ProductName = productDto.ProductName,
				Description = productDto.Description,
				Price = productDto.Price,
				ImageUrl = productDto.ImageUrl,
				IsActive = productDto.IsActive,
				ProductCategoryId = productDto.ProductCategoryId
			};

			// Thêm sản phẩm vào ngữ cảnh
			_context.Products.Add(product);
			await _context.SaveChangesAsync(); // Lưu để có Id cho sản phẩm

			// Duyệt qua từng hình ảnh
			foreach (var image in images)
			{
				// Lấy tên file và tạo CloudBlockBlob
				string fileName = Path.GetFileName(image.FileName);
				CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
				string contentType = image.ContentType;
				blockBlob.Properties.ContentType = contentType;
				// Tải lên hình ảnh
				using (var stream = image.OpenReadStream())
				{
					await blockBlob.UploadFromStreamAsync(stream);
				}

				// Tạo đối tượng ProductImage
				var productImage = new ProductImage
				{
					ProductId = product.Id, // Sử dụng Id của sản phẩm vừa lưu
					ImageUrl = blockBlob.Uri.ToString(),
				};

				// Thêm hình ảnh vào ngữ cảnh
				_context.ProductImages.Add(productImage);
			}

			// Lưu tất cả thay đổi một lần nữa
			await _context.SaveChangesAsync();

			return productDto;
		}


		public async Task UpdateProductAsync(ProductDTO productDto)
		{
			var product = await _context.Products.FindAsync(productDto.Id);
			if (product == null) return;

			product.ProductName = productDto.ProductName;
			product.Description = productDto.Description;
			product.Price = productDto.Price;
			product.ImageUrl = productDto.ImageUrl;
			product.IsActive = productDto.IsActive;
			product.ProductCategoryId = productDto.ProductCategoryId;

			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(int id)
		{
			// Lấy sản phẩm cùng với hình ảnh liên quan trong một truy vấn
			var product = await _context.Products
				.Include(p => p.Images) // Bao gồm hình ảnh liên quan
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product != null)
			{
				// Lấy blob container từ Azure
				CloudBlobContainer container = await _blobStorageService.GetCloudBlogContainer();

				// Xóa từng hình ảnh khỏi Blob Storage

				if (product.Images.Any())
				{
					foreach (var productImage in product.Images)
					{
						// Lấy tên blob từ ImageUrl
						string blobName = Path.GetFileName(productImage.ImageUrl);
						CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

						// Xóa blob khỏi Azure
						await blockBlob.DeleteIfExistsAsync(); // Xóa nếu blob tồn tại
					}
				}

				// Xóa hình ảnh khỏi cơ sở dữ liệu
				_context.ProductImages.RemoveRange(product.Images);

				// Xóa sản phẩm khỏi cơ sở dữ liệu
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
		}


		public async Task<IEnumerable<ProductDTO>> GetProductsByOrder(string orderType, int pageSize, int pageIndex, int categoryId)
		{
			var query = from product in _context.Products
						where product.ProductCategoryId == categoryId && product.IsActive
						join review in _context.Reviews on product.Id equals review.ProductId into productReviews
						from review in productReviews.DefaultIfEmpty()  // Join nhưng cho phép sản phẩm không có đánh giá
						group new { product, review } by new
						{
							product.Id,
							product.ProductName,
							product.Description,
							product.Price,
							product.ImageUrl,
							product.IsActive,
							product.ProductCategoryId
						} into productGroup
						select new ProductDTO
						{
							Id = productGroup.Key.Id,
							ProductName = productGroup.Key.ProductName,
							Description = productGroup.Key.Description,
							Price = productGroup.Key.Price,
							ImageUrl = productGroup.Key.ImageUrl,
							IsActive = productGroup.Key.IsActive,
							ProductCategoryId = productGroup.Key.ProductCategoryId,
							AverageRating = productGroup.Average(g => g.review != null ? g.review.Rating : 0)  // Tính trung bình đánh giá
						};

			// Sắp xếp theo yêu cầu
			query = orderType switch
			{
				"default" => query,
				"price" => query.OrderBy(p => p.Price),
				"price-desc" => query.OrderByDescending(p => p.Price),
				"rating" => query.OrderByDescending(p => p.AverageRating),  // Sắp xếp theo điểm đánh giá
				_ => query.OrderBy(p => p.ProductName)  // Mặc định sắp xếp theo tên sản phẩm
			};

			// Phân trang
			query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

			// Thực thi truy vấn và trả về kết quả
			return await query.ToListAsync();
		}
		public async Task<int> CountProductByCategoryId(int categoryId)
		{
			var count = await _context.Products
							  .Where(product => product.ProductCategoryId == categoryId)
							  .CountAsync();

			return count; 
		}

	}
}

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
using Microsoft.Extensions.Caching.Memory;

namespace ProductAPI.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IBlobStorageService _blobStorageService;
		private readonly IMemoryCache _memoryCache;
		public ProductRepository(ApplicationDbContext context,IBlobStorageService blobStorageService,IMemoryCache memoryCache)
		{
			_context = context;
			_blobStorageService = blobStorageService;
			_memoryCache = memoryCache;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
		{
			const string cacheKey = "all_products"; // Đặt tên khóa cho bộ nhớ cache

			// Kiểm tra xem dữ liệu đã có trong bộ nhớ cache chưa
			if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<ProductDTO> cachedProducts))
			{
				// Nếu không có trong bộ nhớ cache, thực hiện truy vấn cơ sở dữ liệu
				var products = await _context.Products.Include(p => p.Images).Include(p => p.ProductCategory).ToListAsync();

				// Chuyển đổi dữ liệu thành ProductDTO
				cachedProducts = products.Select(p => new ProductDTO
				{
					Id = p.Id,
					ProductName = p.ProductName,
					Description = p.Description,
					Price = p.Price,
					ImageUrl = p.ImageUrl, // Primary image URL if available
					IsActive = p.IsActive,
					ProductCategoryId = p.ProductCategoryId,
					PriceSale = p.PriceSale,
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
				}).ToList();

				// Lưu dữ liệu vào bộ nhớ cache với thời gian hết hạn là 1 giờ
				_memoryCache.Set(cacheKey, cachedProducts, TimeSpan.FromMinutes(15));
			}

			// Trả về dữ liệu từ bộ nhớ cache hoặc cơ sở dữ liệu
			return cachedProducts;
		}
		public async Task<IEnumerable<ResponseProductDialog>> GetAllProductsName(string categoryName)
		{
			// Kiểm tra đầu vào, tránh trường hợp categoryName null hoặc rỗng
			if (string.IsNullOrEmpty(categoryName))
			{
				return new List<ResponseProductDialog>(); // Trả về danh sách rỗng
			}

			// Tìm danh mục theo tên
			var category = await _context.ProductCategories
				.Where(x => x.ProductCategoryName != null && x.ProductCategoryName.Contains(categoryName))
				.FirstOrDefaultAsync();

			if (category == null)
			{
				return new List<ResponseProductDialog>(); // Trả về danh sách rỗng nếu không tìm thấy danh mục
			}

			// Lấy danh sách sản phẩm theo danh mục
			var products = await _context.Products
				.Where(x => x.ProductCategoryId == category.Id)
				.Select(p => new ResponseProductDialog
				{
					ProductName = p.ProductName // Giả sử 'ProductName' là trường tên sản phẩm
				})
				.ToListAsync();

			// Trả về danh sách sản phẩm dưới dạng ResponseProductDialog
			return products;
		}
		public async Task<ProductVariantDTO> GetAllProductVariantName(string productName,string variantName)
		{
			// Kiểm tra đầu vào, tránh trường hợp categoryName null hoặc rỗng
			if (string.IsNullOrEmpty(productName))
			{
				return new ProductVariantDTO(); // Trả về danh sách rỗng
			}

			// Tìm danh mục theo tên
			var product = await _context.Products
				.Where(x => x.ProductName != null && x.ProductName.Contains(productName))
				.FirstOrDefaultAsync();

			if (product == null)
			{
				return new ProductVariantDTO(); // Trả về danh sách rỗng nếu không tìm thấy danh mục
			}

			// Lấy danh sách sản phẩm theo danh mục
			var products = await _context.ProductVariants
				.Where(x => x.ProductId == product.Id && x.VariantName == variantName)
				.Select(p => new ProductVariantDTO
				{
					VariantName = p.VariantName,
					UnitPrice = p.UnitPrice,
					Id =p.Id,

				})
				.FirstOrDefaultAsync();

			// Trả về danh sách sản phẩm dưới dạng ResponseProductDialog
			return products;
		}

		public async Task<IEnumerable<ProductVariantDTO>> GetAllProductVariantsName(string productName)
		{
			// Kiểm tra đầu vào, tránh trường hợp categoryName null hoặc rỗng
			if (string.IsNullOrEmpty(productName))
			{
				return new List<ProductVariantDTO>(); // Trả về danh sách rỗng
			}

			// Tìm danh mục theo tên
			var product = await _context.Products
				.Where(x => x.ProductName != null && x.ProductName.Contains(productName))
				.FirstOrDefaultAsync();

			if (product == null)
			{
				return new List<ProductVariantDTO>(); // Trả về danh sách rỗng nếu không tìm thấy danh mục
			}

			// Lấy danh sách sản phẩm theo danh mục
			var products = await _context.ProductVariants
				.Where(x => x.ProductId == product.Id)
				.Select(p => new ProductVariantDTO
				{
					VariantName = p.VariantName,
					UnitPrice = p.UnitPrice
				})
				.ToListAsync();

			// Trả về danh sách sản phẩm dưới dạng ResponseProductDialog
			return products;
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
			var product = await _context.Products
				.Include(p => p.Images)
				.Include(p=>p.Variants.Where(p=>p.Id==varianId))
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
		public async Task<IEnumerable<ProductDTO?>> GetProductsByVariantIdsAsync(List<int> varianIds)
		{
			// Fetch all product variants and associated products in one query
			var variants = await _context.ProductVariants
				.Where(v => varianIds.Contains(v.Id))
				.Include(v => v.ProductInventorySuppliers)
				.Include(v => v.Product)
				.ThenInclude(p => p.Images)
				.Include(v => v.Product)
				.ThenInclude(p => p.ProductCategory)
				.ToListAsync();

			// Map results to ProductDTO
			var productDTOs = variants.Select(variant => new ProductDTO
			{
				Id = variant.Product.Id,
				ProductName = variant.Product.ProductName,
				Description = variant.Product.Description,
				Price = variant.Product.Price,
				ImageUrl = variant.Product.ImageUrl,
				IsActive = variant.Product.IsActive,
				ProductCategoryId = variant.Product.ProductCategoryId,
				productCategoryDTO = new ProductCategoryDTO
				{
					Id = variant.Product.ProductCategory.Id,
					ProductCategoryName = variant.Product.ProductCategory.ProductCategoryName,
				},
				productImageDTOs = variant.Product.Images.Select(img => new ProductImageDTO
				{
					Id = img.Id,
					ImageUrl = img.ImageUrl,
					ProductId = img.ProductId
				}).ToList(),
				productVariantDTOs = variant.Product.Variants?.Select(v => new ProductVariantDTO
				{
					Id = v.Id,
					ProductId = v.ProductId,
					UnitPrice = v.UnitPrice,
					VariantName = v.VariantName,
					ProductInventorySuppliers = v.ProductInventorySuppliers.Select(supplier => new ProductInventorySupplierDTO
					{
						Quantity = supplier.Quantity,
					}).ToList()
				}).ToList()
			}).ToList();

			return productDTOs;
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
					PriceSale =p.PriceSale,
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
			// Tạo khóa bộ nhớ cache duy nhất cho các tham số
			string cacheKey = $"products_order_{orderType}_size_{pageSize}_index_{pageIndex}_category_{categoryId}";

			// Kiểm tra bộ nhớ cache trước
			if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<ProductDTO> cachedProducts))
			{
				// Truy vấn cơ sở dữ liệu
				var query = _context.Products
					.Where(p => (categoryId == 0 || p.ProductCategoryId == categoryId) && p.IsActive)
					.GroupJoin(
						_context.Reviews,
						product => product.Id,
						review => review.ProductId,
						(product, reviews) => new { product, reviews }
					)
					.SelectMany(
						x => x.reviews.DefaultIfEmpty(),
						(x, review) => new { x.product, review }
					)
					.GroupJoin(
						_context.ProductVariants, // Join thêm bảng biến thể
						x => x.product.Id,
						variant => variant.ProductId,
						(x, variants) => new { x.product, x.review, variants }
					)
					.SelectMany(
						x => x.variants.DefaultIfEmpty(),
						(x, variant) => new { x.product, x.review, variant }
					)
					.GroupBy(
						x => new
						{
							x.product.Id,
							x.product.ProductName,
							x.product.Description,
							x.product.ImageUrl,
							x.product.IsActive,
							x.product.ProductCategoryId,
							x.product.PriceSale
						},
						x => new { x.review, x.variant.UnitPrice }
					)
					.Select(g => new ProductDTO
					{
						Id = g.Key.Id,
						ProductName = g.Key.ProductName,
						Description = g.Key.Description,
						ImageUrl = g.Key.ImageUrl,
						IsActive = g.Key.IsActive,
						PriceSale = g.Key.PriceSale,
						ProductCategoryId = g.Key.ProductCategoryId,
						AverageRating = g.Average(r => r.review != null ? r.review.Rating : 0), // Điểm đánh giá trung bình
						MinPrice = g.Min(r => r.UnitPrice), // Giá thấp nhất từ các biến thể
						MaxPrice = g.Max(r => r.UnitPrice),
					});

				// Sắp xếp theo yêu cầu
				query = orderType switch
				{
					"default" => query,
					"price" => query.OrderBy(p => p.MinPrice), // Sắp xếp theo giá thấp nhất
					"price-desc" => query.OrderByDescending(p => p.MinPrice), // Sắp xếp theo giá cao nhất
					"rating" => query.OrderByDescending(p => p.AverageRating), // Sắp xếp theo điểm đánh giá
					_ => query.OrderBy(p => p.ProductName) // Mặc định sắp xếp theo tên sản phẩm
				};

				// Phân trang
				var pagedQuery = query
					.Skip((pageIndex - 1) * pageSize)
					.Take(pageSize);

				// Thực thi truy vấn và lưu vào bộ nhớ cache
				cachedProducts = await pagedQuery.AsNoTracking().ToListAsync();

				// Lưu vào bộ nhớ cache với thời gian hết hạn 1 giờ
				_memoryCache.Set(cacheKey, cachedProducts, TimeSpan.FromHours(1));
			}

			// Trả về kết quả
			return cachedProducts;
		}



		public async Task<int> CountProductByCategoryId(int categoryId)
		{
			
			if (categoryId ==0)
			{
				return await _context.Products
							 .CountAsync();
			}
			var count = await _context.Products
							  .Where(product => product.ProductCategoryId == categoryId)
							  .CountAsync();

			return count; 
		}

	}
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.IRepository;
using ShoppingCartAPI.IServices;
using ShoppingCartAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.API.Repository
{
	public class ShoppingCartRepository : IShoppingCartRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IProductService _productService;
		private readonly IMemoryCache _memoryCache;
		public ShoppingCartRepository(ApplicationDbContext context,IProductService productService,IMemoryCache memoryCache)
		{
			_context = context;
			_productService = productService;	
			_memoryCache = memoryCache;
		}

		public async Task<ICollection<object?>> GetCartItemsbyUserIdAsync(string userId)
		{
			// Kiểm tra cache trước
			if (_memoryCache.TryGetValue($"CartItems_{userId}", out ICollection<object?> cachedCartItems))
			{
				// Nếu dữ liệu có trong cache, trả về dữ liệu từ cache
				return cachedCartItems;
			}

			// Truy vấn từ database nếu không có trong cache
			var cart = await _context.ShoppingCarts
									 .Where(cart => cart.UserId == userId)
		
									 .ToListAsync();

			ICollection<object?> cartList = new List<object?>();

			foreach (var cartItem in cart)
			{
				var productInfo = await _productService.GetProductByIdAsync(cartItem.ProductVarianId);
				cartList.Add(new
				{
					ShoppingCart = cartItem,
					Product = productInfo
				});
			}

			// Lưu dữ liệu vào cache với thời gian hết hạn (ví dụ: 5 phút)
			var cacheOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15), // Hết hạn sau 5 phút
				SlidingExpiration = TimeSpan.FromMinutes(2) // Làm mới cache nếu được truy cập
			};

			_memoryCache.Set($"CartItems_{userId}", cartList, cacheOptions);

			// Trả về danh sách dữ liệu
			return cartList;
		}

		public async Task<object?> GetCartItemByIdAsync(int cartItemId)
		{
			// Lấy thông tin giỏ hàng theo cartItemId
			var shoppingCart = await _context.ShoppingCarts.FindAsync(cartItemId);
			
			if (shoppingCart == null)
			{
				return null; // Không tìm thấy giỏ hàng
			}

			// Lấy thông tin sản phẩm từ dịch vụ
			dynamic productInfo = await _productService.GetProductByIdAsync(shoppingCart.ProductVarianId);

			
			// Trả về cả thông tin giỏ hàng và sản phẩm
			return new
			{
				ShoppingCart = shoppingCart,
				Product = productInfo
			};
		}
		public async Task<ShoppingCarts?> GetCartItemByVarianIdUserIdAsync(int productVarianId, string userId)
		{
			var cart = await _context.ShoppingCarts.Where(x=>x.UserId.Contains(userId)&& x.ProductVarianId==productVarianId).FirstOrDefaultAsync();
			var a = 1;
			return cart;


		}
		public async Task<ShoppingCarts?> AddToCartAsync(ShoppingCarts cartItem)
		{
			var checkItem = await _context.ShoppingCarts
				.Where(c => c.UserId == cartItem.UserId  && c.ProductVarianId == cartItem.ProductVarianId)
				.FirstOrDefaultAsync();
			if (checkItem != null)
			{
				checkItem.Quantity += cartItem.Quantity;
				_context.ShoppingCarts.Update(checkItem);
				await _context.SaveChangesAsync();
				return checkItem;
			}
			_context.ShoppingCarts.Add(cartItem);
			await _context.SaveChangesAsync();
			return cartItem;
		}

		public async Task UpdateCartItemAsync(ShoppingCarts cartItem)
		{
			var cacheKey = $"CartItems_{cartItem.UserId}";

			// Remove the cache from memory
			_memoryCache.Remove(cacheKey);
			// Update the cart item in the database
			_context.ShoppingCarts.Update(cartItem);
			await _context.SaveChangesAsync();
			await GetCartItemsbyUserIdAsync(cartItem.UserId);
			// Invalidate or update the cache
			// Remove existing cache

			// Optionally, re-fetch data and set it back to the cache

		}

		public async Task RemoveCartItemAsync(List<int> Ids)
		{
			var cartItemsToRemove = new List<ShoppingCarts>();

			// Lấy tất cả các mục cần xóa một lần
			foreach (var Id in Ids)
			{
				var cartItem = await _context.ShoppingCarts.FindAsync(Id);
				if (cartItem != null)
				{
					cartItemsToRemove.Add(cartItem);
				}
			}

			// Xóa tất cả các mục đã tìm thấy và lưu thay đổi một lần
			if (cartItemsToRemove.Any())
			{
				_context.ShoppingCarts.RemoveRange(cartItemsToRemove);
				await _context.SaveChangesAsync();
			}
		}


		public async Task ClearCartAsync(string userId)
		{
			var cartItems = _context.ShoppingCarts.Where(cart => cart.UserId == userId);
			_context.ShoppingCarts.RemoveRange(cartItems);
			await _context.SaveChangesAsync();
		}

		public async Task<int> GetCartTotalAsync(string userId)
		{
			return await _context.ShoppingCarts
				.Where(cart => cart.UserId == userId)
				.SumAsync(cart => cart.Quantity * cart.Price);
		}
	}
}

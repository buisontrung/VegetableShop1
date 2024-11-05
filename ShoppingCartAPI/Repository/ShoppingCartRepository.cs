using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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

		public ShoppingCartRepository(ApplicationDbContext context,IProductService productService)
		{
			_context = context;
			_productService = productService;	
		}

		public async Task<ICollection<object?>> GetCartItemsbyUserIdAsync(string userId)
		{
			// Retrieve the cart items for the specified user
			var cart = await _context.ShoppingCarts
									 .Where(cart => cart.UserId == userId)
									 .ToListAsync();

			// Initialize a list to store the product information
			ICollection<object?> cartList = new List<object?>();
			
			// Loop through each cart item to retrieve product information
			foreach (var cartItem in cart)
			{
				// Retrieve product information by product variant ID
				var productInfo = await _productService.GetProductByIdAsync(cartItem.ProductVarianId);
				cartList.Add(new
				{
					ShoppingCart = cartItem,
					Product = productInfo
				}); // Add product info to the list
			}

			// Return the list of product information
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
				.Where(c => c.UserId == cartItem.UserId  && c.ProductVarianId == c.ProductVarianId)
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
			_context.ShoppingCarts.Update(cartItem);
			await _context.SaveChangesAsync();
		}

		public async Task RemoveCartItemAsync(int cartItemId)
		{
			var cartItem = await _context.ShoppingCarts.FindAsync(cartItemId);
			if (cartItem != null)
			{
				_context.ShoppingCarts.Remove(cartItem);
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

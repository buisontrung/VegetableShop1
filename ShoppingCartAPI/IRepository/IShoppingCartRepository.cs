using ShoppingCartAPI.Model;

namespace ShoppingCartAPI.IRepository
{
	public interface IShoppingCartRepository
	{
		Task<ICollection<object?>> GetCartItemsbyUserIdAsync(string userId);
		Task<object?> GetCartItemByIdAsync(int cartItemId);
		Task<ShoppingCarts?> GetCartItemByVarianIdUserIdAsync(int productVarianId, string userId);
		Task<ShoppingCarts?> AddToCartAsync(ShoppingCarts cartItem);
		Task UpdateCartItemAsync(ShoppingCarts cartItem);
		Task RemoveCartItemAsync(int cartItemId);
		Task ClearCartAsync(string userId);
		Task<int> GetCartTotalAsync(string userId);
	}
}

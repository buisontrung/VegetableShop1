using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Model;
namespace ShoppingCartAPI.hub
{
	public class CartHub : Hub
	{
		private readonly ApplicationDbContext _context;
		public CartHub(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task JoinGroup(string userId)
		{
			// Tham gia nhóm có tên là Id
			await Groups.AddToGroupAsync(Context.ConnectionId, userId);

			await Clients.Caller.SendAsync("GroupJoined", userId);
		}
		// Hàm gửi thông báo đến client
		public async Task NotifyCartUpdate(string userId)
		{
			var cartItems = await _context.ShoppingCarts.Where(x => x.UserId == userId).ToListAsync();
			Console.WriteLine($"UserId: {userId}, CartItems: {cartItems.Count}");
			if (cartItems.Any())	
			{
				await Clients.Group(userId).SendAsync("CartUpdated", cartItems);
			}
			else
			{
				await Clients.Group(userId).SendAsync("CartUpdated", new List<ShoppingCarts>());
			}
		}
	}
}

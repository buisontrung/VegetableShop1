using Microsoft.Data.SqlClient;
using Order.Model;
using Order.ModelDto;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Order.IRepository
{
	public interface IOrderRepository
	{
		Task<bool> CheckOrderStatusCompleted(ClaimsPrincipal user, IEnumerable<int> VariantIds);
		Task<List<decimal>> GetOrdersTotalPriceMonth();
		Task<IEnumerable<OrderRequestDTO>> GetOrdersByMonth(int month, int year);
		Task<IEnumerable<IGrouping<string, OrderRequestDTO>>> GetOrdersGroupedByMonthAndYear();
		Task<IEnumerable<OrderRequestDTO?>> GetAll();
		Task UpdateStatusAsync(int Id, int Status);
		Task<OrderRequestDTO?> GetById(int Id);
		Task<IEnumerable<OrderRequestDTO>> GetByUserId(string userId);
		Task<OrderRequestDTO> CreateOrderAsync(OrderRequestDTO order);
		Task<string> ThanhToanVNPayAsync(OrderRequestDTO order);
		Task SaveChangesAsync();
		Task<List<OrderDetailDTO>> GetOrderDetailsWithApiAsync(int id);
	}
}

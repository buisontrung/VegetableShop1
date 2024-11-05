using Microsoft.Data.SqlClient;
using Order.Model;
using Order.ModelDto;

namespace Order.IRepository
{
	public interface IOrderRepository
	{
		Task<OrderRequestDTO> CreateOrderAsync(OrderRequestDTO order);
		Task SaveChangesAsync();
	}
}

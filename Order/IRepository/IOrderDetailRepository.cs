using Order.ModelDto;

namespace Order.IRepository
{
	public interface IOrderDetailRepository
	{
		Task<IEnumerable<OrderDetailDTO>> GetAllAsync();
		Task<OrderDetailDTO> GetByIdAsync(int id);
		Task<IEnumerable<OrderDetailDTO>> GetByOrderIdAsync(int orderId); // Thêm phương thức này
		Task AddAsync(OrderDetailDTO orderDetail);
		Task UpdateAsync(OrderDetailDTO orderDetail);
		Task DeleteAsync(int id);
	}
}

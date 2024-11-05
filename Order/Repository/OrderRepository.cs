using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.IRepository;
using Order.Model;
using Order.ModelDto;

namespace Order.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _dbContext;


		public OrderRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

		}

		// Tạo mới đơn hàng
		public async Task<OrderRequestDTO> CreateOrderAsync(OrderRequestDTO orderRequest)
		{
			// Kiểm tra danh sách sản phẩm
			if (orderRequest.Items == null || !orderRequest.Items.Any())
			{
				throw new ArgumentException("Giỏ hàng không có sản phẩm nào.");
			}

			// Tạo mới đối tượng Order
			var order = new Order.Model.Order
			{
				CustomerName = orderRequest.CustomerName,
				Address = orderRequest.Address,
				Phone = orderRequest.Phone,
				OrderDetails = new List<OrderDetail>(),
				TypePayment = orderRequest.TypePayment,
				PromotionId = orderRequest.PromotionId
			};

			// Thêm chi tiết đơn hàng từ danh sách sản phẩm
			foreach (var item in orderRequest.Items)
			{
				order.OrderDetails.Add(new OrderDetail
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Price = item.Price
				});
			}

			// Tính tổng số tiền đơn hàng
			order.TotalAmount = order.OrderDetails.Sum(x => x.Quantity * x.Price);

			// Sinh mã đơn hàng ngẫu nhiên
			Random random = new Random();
			order.Code = "DH" + random.Next(1000, 9999);

			// Lưu đơn hàng vào cơ sở dữ liệu
			_dbContext.Orders.Add(order);
			foreach(var item in order.OrderDetails)
			{
				_dbContext.OrderDetails.Add(item);
			}
			await _dbContext.SaveChangesAsync();

			// Gửi email xác nhận đơn hàng cho khách hàng
			


			return orderRequest;
		}

		// Lưu thay đổi vào cơ sở dữ liệu
		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}
	}
}

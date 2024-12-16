
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Order.Data;
using Order.IRepository;
using Order.IServices;
using Order.Model;
using Order.ModelDto;
using Order.Service;
using Order.Services;
using System.Security.Claims;

namespace Order.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly ICurrentUserService currentUserService;
		private readonly IProductService productService;
		private readonly IEmailService emailService;

		public OrderRepository(ApplicationDbContext dbContext, IConfiguration configuration, ICurrentUserService currentUserService,IProductService productService,IEmailService emailService)
		{
			_dbContext = dbContext;
			_configuration = configuration;
			this.currentUserService = currentUserService;
			this.productService = productService;
			this.emailService = emailService;
		}
		public async Task<IEnumerable<OrderRequestDTO>> GetByUserId(string userId)
		{
			try
			{
				// Lấy danh sách đơn hàng của người dùng từ cơ sở dữ liệu
				var orders = await _dbContext.Orders
											 .Where(x => x.UserId == userId)
											 .ToListAsync();

				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (orders == null || !orders.Any())
				{
					Console.WriteLine($"No orders found for user {userId}");
					return Enumerable.Empty<OrderRequestDTO>(); // Trả về danh sách rỗng nếu không tìm thấy đơn hàng
				}

				// Chuyển đổi danh sách đơn hàng thành danh sách OrderRequestDTO
				var orderRequestDTOs = orders.Select(order => new OrderRequestDTO
				{
					Id = order.Id,
					UserId = order.UserId,
					PaymentMethodId = order.PaymentMethodId,
					PromotionId = order.PromotionId,
					IsPayment = order.IsPayment,
					Address = order.Address,
					Code = order.Code,
					CustomerName = order.CustomerName,
					DiscountAmount = order.DiscountAmount,
					Phone = order.Phone,
					Quantity = order.Quantity,
					TotalAmount = order.TotalAmount,
					Status = order.Status,
					CreateDate = order.CreateDate,
					Email = order.Email,
				});

				return orderRequestDTOs; // Trả về danh sách DTO
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có lỗi xảy ra trong quá trình truy vấn hoặc chuyển đổi dữ liệu
				Console.WriteLine($"An error occurred while fetching orders for user {userId}: {ex.Message}");
				// Có thể trả về danh sách rỗng hoặc null tùy theo yêu cầu
				return Enumerable.Empty<OrderRequestDTO>();
			}
		}
		public async Task<IEnumerable<OrderRequestDTO?>> GetAll()
		{
			try
			{
				// Lấy danh sách đơn hàng của người dùng từ cơ sở dữ liệu
				var order = await _dbContext.Orders.ToListAsync();

				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (order == null)
				{

					return null;
				}

				// Chuyển đổi danh sách đơn hàng thành danh sách OrderRequestDTO
				

				return order.Select(x=> new OrderRequestDTO{
				
					Address = x.Address,
					Code=x.Code,
					CreateDate =x.CreateDate,
					CustomerName=x.CustomerName,
					DiscountAmount=x.DiscountAmount,
					Id	= x.Id,
					IsPayment=x.IsPayment,
					Items = [],
					PaymentMethodId = x.PaymentMethodId,
					Phone = x.Phone,
					Status = x.Status,
					PromotionId = x.PromotionId,
					TotalAmount=x.TotalAmount,
					Quantity	=x.Quantity,
					UserId = x.UserId,
					Email = x.Email,
				}); // Trả về danh sách DTO
			}

			catch (Exception ex)
			{

				return null;
			}
		}
		public async Task<OrderRequestDTO?> GetById(int Id)
		{
			try
			{
				// Lấy danh sách đơn hàng của người dùng từ cơ sở dữ liệu
				var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);

				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (order == null)
				{

					return null; 
				}

				// Chuyển đổi danh sách đơn hàng thành danh sách OrderRequestDTO
				var orderRequestDTO = new OrderRequestDTO
				{
					Id = order.Id,
					UserId = order.UserId,
					PaymentMethodId = order.PaymentMethodId,
					PromotionId = order.PromotionId,
					IsPayment = order.IsPayment,
					Address = order.Address,
					Code = order.Code,
					CustomerName = order.CustomerName,
					DiscountAmount = order.DiscountAmount,
					Phone = order.Phone,
					Quantity = order.Quantity,
					TotalAmount = order.TotalAmount,
					Status = order.Status,
					CreateDate = order.CreateDate,
					Email = order.Email,
				};

				return orderRequestDTO; // Trả về danh sách DTO
			}
			catch (Exception ex)
			{

				return null;
			}
		}
		public async Task<bool> CheckOrderStatusCompleted(ClaimsPrincipal user, IEnumerable<int> VariantIds)
		{
			try
			{
				// Lấy danh sách đơn hàng của người dùng từ cơ sở dữ liệu
				var UserId = user.FindFirst("Id")?.Value;
				var order = await _dbContext.Orders
									.Include(x => x.OrderDetails)
									.AnyAsync(x => x.UserId == UserId && x.OrderDetails.Any(od => VariantIds.Contains(od.ProductId)));

				


				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (!order)
				{

					return false;
				}


				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}
		public async Task<List<OrderDetailDTO>> GetOrderDetailsWithApiAsync(int id)
		{
			try
			{
				// Lấy danh sách đơn hàng dựa trên Id
				var orderDetailsList = await _dbContext.OrderDetails
					.Where(x => x.OrderId == id)
					.ToListAsync();

				if (orderDetailsList == null || !orderDetailsList.Any())
				{
					return new List<OrderDetailDTO>(); // Trả về danh sách rỗng nếu không có đơn hàng
				}
	
				// Danh sách các task để gọi API
				var tasks = new List<Task<ProductDTO?>>(); // Sửa kiểu thành Task<ProductDTO?>

				using var httpClient = new HttpClient();
				List<OrderDetailDTO> orderDetailDTOList = orderDetailsList.Select(x => new OrderDetailDTO
				{
					// Giả sử `x` là một đối tượng có các thuộc tính như OrderId, ProductName, Quantity, Price
					OrderId = x.OrderId,
					ProductId = x.ProductId,
					Quantity = x.Quantity,
					Price = x.Price
				}).ToList();
				foreach (var orderDetail in orderDetailsList)
				{
					var productId = orderDetail.ProductId;
					// Gọi API để lấy thông tin sản phẩm
					var res = productService.GetProductByIdAsync(productId); // res là Task<ProductDTO?>
					tasks.Add(res); // Thêm vào danh sách tasks
				}

				// Chờ tất cả các Task hoàn thành
				var responses = await Task.WhenAll(tasks);

				var results = new List<object>();
				foreach (var orderDetail in orderDetailDTOList)
				{
					// Tìm kiếm sản phẩm tương ứng với ProductId trong responses
					var productInfo = responses.FirstOrDefault(p => p?.productVariantDTOs?.FirstOrDefault()?.Id == orderDetail.ProductId);
					orderDetail.product = productInfo;

				}

				return orderDetailDTOList;
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ (log nếu cần)
				Console.WriteLine($"Error: {ex.Message}");
				return new List<OrderDetailDTO>(); // Trả về danh sách rỗng nếu có lỗi
			}
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
				PaymentMethodId = orderRequest.PaymentMethodId,
				PromotionId = orderRequest.PromotionId,
				Code = orderRequest.Code,
				IsPayment= orderRequest.IsPayment,
				UserId = orderRequest.UserId,
				TotalAmount = orderRequest.TotalAmount,
				DiscountAmount = orderRequest.DiscountAmount,
				Quantity = orderRequest.Quantity,
				Status = orderRequest.Status,
				Email = orderRequest.Email
				
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
			
			_dbContext.Orders.Add(order);
			foreach(var item in order.OrderDetails)
			{
				_dbContext.OrderDetails.Add(item);
			}
			
			await _dbContext.SaveChangesAsync();

			// Gửi email xác nhận đơn hàng cho khách hàng
			var order1 = await GetOrderDetailsWithApiAsync(order.Id);
			var emailbody = StaticEmail.GenerateEmailBody(order.CustomerName, order1, order.Code);
			var Otp = new Otp()
			{
				Email = order.Email,
				Subject = "Cảm ơn vì đã sử dụng dịch vụ của chúng tôi",
				Emailbody = emailbody
			};
			await emailService.SendEmail(Otp);
			return orderRequest;
		}
		public async Task<string> ThanhToanVNPayAsync(OrderRequestDTO orderRequest)
		{
			var vnpUrl = _configuration["VNPay:Url"];
			var vnpReturnUrl = _configuration["VNPay:ReturnUrl"];
			var vnpTmnCode = _configuration["VNPay:TmnCode"];
			var vnpHashSecret = _configuration["VNPay:HashSecret"];

			var txnRef = orderRequest.Code ?? Guid.NewGuid().ToString();

			var vnpayPayRequest = new VnpayPayRequest(
				"2.1.0", vnpTmnCode, DateTime.Now, currentUserService.IpAddress ?? string.Empty,
				(int)orderRequest.DiscountAmount, "VND", "other", "Thanh toan hoa don",
				vnpReturnUrl, txnRef);

			
			return await Task.FromResult(vnpayPayRequest.GetLink(vnpUrl, vnpHashSecret)); 
		}
		public async Task<IEnumerable<OrderRequestDTO>> GetOrdersByMonth(int month, int year)
		{
			try
			{
				// Lấy danh sách đơn hàng trong tháng và năm cụ thể từ cơ sở dữ liệu
				var orders = await _dbContext.Orders
							 .Where(x => x.CreateDate.HasValue && x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year)
							 .ToListAsync();

				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (orders == null || !orders.Any())
				{
					Console.WriteLine($"No orders found for {month}/{year}");
					return Enumerable.Empty<OrderRequestDTO>(); // Trả về danh sách rỗng nếu không tìm thấy đơn hàng
				}

				// Chuyển đổi danh sách đơn hàng thành danh sách OrderRequestDTO
				var orderRequestDTOs = orders.Select(order => new OrderRequestDTO
				{
					Id = order.Id,
					UserId = order.UserId,
					PaymentMethodId = order.PaymentMethodId,
					PromotionId = order.PromotionId,
					IsPayment = order.IsPayment,
					Address = order.Address,
					Code = order.Code,
					CustomerName = order.CustomerName,
					DiscountAmount = order.DiscountAmount,
					Phone = order.Phone,
					Quantity = order.Quantity,
					TotalAmount = order.TotalAmount,
					Status = order.Status,
					CreateDate = order.CreateDate,
				});

				return orderRequestDTOs; // Trả về danh sách DTO
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có lỗi xảy ra trong quá trình truy vấn
				Console.WriteLine($"An error occurred while fetching orders for {month}/{year}: {ex.Message}");
				return Enumerable.Empty<OrderRequestDTO>(); // Trả về danh sách rỗng nếu có lỗi
			}
		}
		public async Task<IEnumerable<IGrouping<string, OrderRequestDTO>>> GetOrdersGroupedByMonthAndYear()
		{
			try
			{
				// Lấy danh sách đơn hàng từ cơ sở dữ liệu
				var orders = await _dbContext.Orders
											  .Where(x => x.CreateDate.HasValue)  // Chỉ lấy các đơn hàng có ngày tạo
											  .ToListAsync();

				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (orders == null || !orders.Any())
				{
					return Enumerable.Empty<IGrouping<string, OrderRequestDTO>>(); // Trả về danh sách rỗng nếu không có đơn hàng
				}

				// Chuyển đổi danh sách đơn hàng thành danh sách OrderRequestDTO
				var orderRequestDTOs = orders.Select(order => new OrderRequestDTO
				{
					Id = order.Id,
					UserId = order.UserId,
					PaymentMethodId = order.PaymentMethodId,
					PromotionId = order.PromotionId,
					IsPayment = order.IsPayment,
					Address = order.Address,
					Code = order.Code,
					CustomerName = order.CustomerName,
					DiscountAmount = order.DiscountAmount,
					Phone = order.Phone,
					Quantity = order.Quantity,
					TotalAmount = order.TotalAmount,
					Status = order.Status,
					CreateDate = order.CreateDate,
				});

				// Nhóm các đơn hàng theo tháng và năm, và sắp xếp theo tháng trong năm
				var groupedOrders = orderRequestDTOs
									.GroupBy(x => $"{x.CreateDate.Value.Year}-{x.CreateDate.Value.Month:D2}")  // Nhóm theo Year-Month
									.OrderBy(g => g.Key)  // Sắp xếp theo Year-Month
									.ToList();

				return groupedOrders; // Trả về kết quả đã nhóm và sắp xếp
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có lỗi xảy ra
				Console.WriteLine($"An error occurred while fetching and grouping orders: {ex.Message}");
				return Enumerable.Empty<IGrouping<string, OrderRequestDTO>>(); // Trả về danh sách rỗng nếu có lỗi
			}
		}
		public async Task UpdateStatusAsync(int Id, int Status)
		{
			// Lấy entity từ cơ sở dữ liệu
			var order = await _dbContext.Orders.FindAsync(Id);
			if (order == null) return;
			if (Status == 4) return;
			order.Status = Status;

			_dbContext.Orders.Update(order);
			await _dbContext.SaveChangesAsync();
		}
		public async Task<List<decimal>> GetOrdersTotalPriceMonth()
		{
			try
			{
				// Lấy danh sách đơn hàng với các trường cần thiết từ cơ sở dữ liệu
				var orders = await _dbContext.Orders
											  .Where(x => x.CreateDate.HasValue)  // Chỉ lấy các đơn hàng có ngày tạo
											  .Select(order => new
											  {
												  order.CreateDate,
												  order.DiscountAmount
											  })
											  .ToListAsync();

				// Kiểm tra nếu không có đơn hàng nào được tìm thấy
				if (orders == null || !orders.Any())
				{
					return Enumerable.Repeat(0m, 12).ToList(); // Trả về mảng rỗng với 12 phần tử 0 nếu không có đơn hàng
				}

				// Tạo mảng tổng cho từng tháng (12 tháng)
				var monthlyTotals = new decimal[12];

				// Duyệt qua các đơn hàng để tính tổng theo tháng
				foreach (var order in orders)
				{
					int month = order.CreateDate.Value.Month - 1; // Lấy tháng (0-based index, tháng 1 -> 0, tháng 2 -> 1, ...)
					monthlyTotals[month] += order.DiscountAmount ?? 0;  // Cộng tổng số tiền của đơn hàng vào tháng tương ứng
				}

				return monthlyTotals.ToList(); // Trả về mảng tổng cho từng tháng
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có lỗi xảy ra
				Console.WriteLine($"An error occurred while fetching and grouping orders: {ex.Message}");
				return Enumerable.Repeat(0m, 12).ToList(); // Trả về mảng rỗng với 12 phần tử 0 nếu có lỗi
			}
		}

		// Lưu thay đổi vào cơ sở dữ liệu
		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}
	}

}

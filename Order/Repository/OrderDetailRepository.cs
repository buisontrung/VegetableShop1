
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
using Newtonsoft.Json;
using Order.Data;
using Order.IRepository;
using Order.Model;
using Order.ModelDto;
using System.Net.Http;

namespace Order.Repository
{
	public class OrderDetailRepository : IOrderDetailRepository
	{
		private readonly HttpClient _httpClient;
		private readonly ApplicationDbContext _context;

		public OrderDetailRepository(HttpClient httpClient, ApplicationDbContext context)
		{
			_httpClient = httpClient;
			_context = context;
		}

		public async Task<IEnumerable<OrderDetailDTO>> GetAllAsync()
		{
			// Truy vấn từ bảng OrderDetails (entity)
			var orderDetails = await _context.OrderDetails.ToListAsync();

			// Chuyển entity thành DTO
			var orderDetailDTOs = orderDetails.Select(orderDetail => new OrderDetailDTO
			{
				Id = orderDetail.Id,
				OrderId = orderDetail.OrderId,
				ProductId = orderDetail.ProductId,
				Price = orderDetail.Price,
				Quantity = orderDetail.Quantity
			});

			return orderDetailDTOs;
		}

		public async Task<OrderDetailDTO> GetByIdAsync(int id)
		{
			// Lấy orderDetail theo id từ cơ sở dữ liệu
			var orderDetail = await _context.OrderDetails.FindAsync(id);
			if (orderDetail == null) return null;

			// Chuyển entity thành DTO
			var orderDetailDTO = new OrderDetailDTO
			{
				Id = orderDetail.Id,
				OrderId = orderDetail.OrderId,
				ProductId = orderDetail.ProductId,
				Price = orderDetail.Price,
				Quantity = orderDetail.Quantity
			};

			return orderDetailDTO;
		}

		public async Task<IEnumerable<OrderDetailDTO>> GetByOrderIdAsync(int orderId)
		{
			// Lấy danh sách OrderDetails từ cơ sở dữ liệu
			var orderDetails = await _context.OrderDetails
											  .Where(detail => detail.OrderId == orderId)
											  .ToListAsync();

			// Chuyển đổi OrderDetails thành OrderDetailDTOs
			var orderDetailDTOs = orderDetails.Select(orderDetail => new OrderDetailDTO
			{
				Id = orderDetail.Id,
				OrderId = orderDetail.OrderId,
				ProductId = orderDetail.ProductId,
				Price = orderDetail.Price,
				Quantity = orderDetail.Quantity,
				product = null // Đảm bảo rằng bạn đặt `Product` là null ban đầu
			}).ToList(); // Convert to List to allow modifications

			// Tạo danh sách các tác vụ để gọi API
			var tasks = orderDetailDTOs.Select(async orderDetail =>
			{
				try
				{
					var url = $"https://localhost:7001/api/Product/varianid={orderDetail.ProductId}";
					var response = await _httpClient.GetAsync(url);

					if (response.IsSuccessStatusCode)
					{
						var content = await response.Content.ReadAsStringAsync();
						var product = JsonConvert.DeserializeObject<ProductDTO>(content);

						// Cập nhật thông tin sản phẩm vào DTO
						if (product != null)
						{
							orderDetail.product = product; // Cập nhật `Product` trong DTO
						}
					}
					else
					{
						Console.WriteLine($"Failed to fetch product {orderDetail.ProductId}, Status: {response.StatusCode}");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error fetching product {orderDetail.ProductId}: {ex.Message}");
				}
			});

			// Đợi tất cả các tác vụ HTTP hoàn thành
			await Task.WhenAll(tasks);

			// Trả về danh sách OrderDetailDTOs với thông tin sản phẩm
			return orderDetailDTOs;
		}


		public async Task AddAsync(OrderDetailDTO orderDetailDTO)
		{
			// Chuyển DTO thành entity trước khi thêm vào cơ sở dữ liệu
			var orderDetail = new OrderDetail
			{
				OrderId = orderDetailDTO.OrderId,
				ProductId = orderDetailDTO.ProductId,
				Price = orderDetailDTO.Price,
				Quantity = orderDetailDTO.Quantity
			};

			await _context.OrderDetails.AddAsync(orderDetail);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(OrderDetailDTO orderDetailDTO)
		{
			// Lấy entity từ cơ sở dữ liệu
			var orderDetail = await _context.OrderDetails.FindAsync(orderDetailDTO.Id);
			if (orderDetail == null) return;

			// Cập nhật các thuộc tính từ DTO vào entity
			orderDetail.OrderId = orderDetailDTO.OrderId;
			orderDetail.ProductId = orderDetailDTO.ProductId;
			orderDetail.Price = orderDetailDTO.Price;
			orderDetail.Quantity = orderDetailDTO.Quantity;

			_context.OrderDetails.Update(orderDetail);
			await _context.SaveChangesAsync();
		}
		

		public async Task DeleteAsync(int id)
		{
			// Lấy entity từ cơ sở dữ liệu
			var orderDetail = await _context.OrderDetails.FindAsync(id);
			if (orderDetail != null)
			{
				_context.OrderDetails.Remove(orderDetail);
				await _context.SaveChangesAsync();
			}
		}
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Data;
using Order.Model;
using Order.ModelDto;

using Order.IRepository;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace Order.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IMemoryCache _memoryCache;

		public OrderController(IOrderRepository orderRepository, IMemoryCache memoryCache)
		{
			_orderRepository = orderRepository;
			_memoryCache = memoryCache;
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> CreateOrderAsync(int Id)
		{


			var order = await _orderRepository.GetById(Id);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
		[Authorize]
		[HttpGet("checkOrder")]
		public async Task<IActionResult> CheckOrderAsync([FromQuery]IEnumerable<int> VariantIds)
		{


			var isOrder = await _orderRepository.CheckOrderStatusCompleted(User,VariantIds);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(isOrder);
		}
		[HttpGet("getall")]
		public async Task<IActionResult> GetAll()
		{


			var order = await _orderRepository.GetAll();


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
		[HttpGet("getmonthall")]
		public async Task<IActionResult> GetAllYear()
		{


			var order = await _orderRepository.GetOrdersGroupedByMonthAndYear();


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
		[HttpGet("getmonth")]
		public async Task<IActionResult> GetAllYear([FromQuery] int month, [FromQuery] int year)
		{


			var order = await _orderRepository.GetOrdersByMonth(month, year);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}

		[HttpGet("gettotalmonth")]
		public async Task<IActionResult> GetTotalMonth()
		{


			var order = await _orderRepository.GetOrdersTotalPriceMonth();


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
		[HttpGet("getOrderDetail/{Id}")]
		public async Task<IActionResult> GetOrderDetailsWithApiAsync(int Id)
		{


			var order = await _orderRepository.GetOrderDetailsWithApiAsync(Id);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}

		[HttpGet("userId={userId}")]
		public async Task<IActionResult> CreateOrderAsync(string userId)
		{


			var order = await _orderRepository.GetByUserId(userId);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
		[Authorize(Roles = "admin")]
		[HttpPut("UpdateStatus")]
		public async Task<IActionResult> UpdateStatusAsync([FromBody] UpdateStatusRequest order)
		{


			await _orderRepository.UpdateStatusAsync(order.Id,order.Status);
			

			// Trả về thông tin đơn hàng đã được tạo
			return Ok("Success");
		}
		[HttpPost("thanh-toan-nhan-hang")]
		public async Task<IActionResult> CreateOrderAsync(OrderRequestDTO orderRequest)
		{
			if (orderRequest == null)
			{
				return BadRequest("Yêu cầu không hợp lệ.");
			}

			// Tạo đơn hàng
			orderRequest.CalculateTotals();

			var order = await _orderRepository.CreateOrderAsync(orderRequest);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
		[HttpPost("vnpay")]
		public async Task<IActionResult> CreateOrderVnPayAsync(OrderRequestDTO orderRequest)
		{
			if (orderRequest == null)
			{
				return BadRequest("Yêu cầu không hợp lệ.");
			}
			orderRequest.CalculateTotals();

			_memoryCache.Set($"order:{orderRequest.Code}", JsonConvert.SerializeObject(orderRequest), TimeSpan.FromMinutes(15));
			string vnpayurl = await _orderRepository.ThanhToanVNPayAsync(orderRequest);
			return Ok(new { url= vnpayurl,code= orderRequest.Code });
		}
		[HttpGet("vnpay-return")]
		public async Task<IActionResult> PaymentReturn([FromQuery] string vnp_ResponseCode, [FromQuery] string vnp_TxnRef, [FromQuery] string vnp_SecureHash, [FromQuery] string vnp_Amount, [FromQuery] string vnp_OrderInfo)
		{
			if (vnp_ResponseCode != "00")
			{
				return BadRequest("Thanh toán thất bại.");
			}
			var orderJson = _memoryCache.Get<string>($"order:{vnp_TxnRef}");
			if (orderJson == null)
			{
				return BadRequest("Không tìm thấy thông tin giao dịch.");
			}
			try
			{
				var orderRequest = JsonConvert.DeserializeObject<OrderRequestDTO>(orderJson);
				await _orderRepository.CreateOrderAsync(orderRequest);
				_memoryCache.Remove($"order:{vnp_TxnRef}");
				return Redirect("http://localhost:5173/#/tai-khoan/don-hang");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Deserialization error: {ex.Message}");
				Console.WriteLine($"Stack Trace: {ex.StackTrace}");
				return BadRequest("Dữ liệu không hợp lệ.");
			}
			
			
		}
	}
}

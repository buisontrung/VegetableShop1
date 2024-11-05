using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Data;
using Order.Model;
using Order.ModelDto;
using Order.IRepository;

namespace Order.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;


		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
	
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrderAsync(OrderRequestDTO orderRequest)
		{
			if (orderRequest == null)
			{
				return BadRequest("Yêu cầu không hợp lệ.");
			}

			// Tạo đơn hàng

	
				var order = await _orderRepository.CreateOrderAsync(orderRequest);


			// Trả về thông tin đơn hàng đã được tạo
			return Ok(order);
		}
	}
}

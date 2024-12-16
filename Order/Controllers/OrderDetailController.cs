using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.IRepository;
using Order.Repository;

namespace Order.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderDetailController : ControllerBase
	{
		private readonly IOrderDetailRepository orderDetailRepository;
		public OrderDetailController(IOrderDetailRepository orderDetailRepository)
		{
			this.orderDetailRepository = orderDetailRepository;
		}

		[HttpGet("orderId={id}")]
		public async Task<IActionResult> GetOrderDetailsByOrderId(int id)
		{
			
				var orderDetails = await orderDetailRepository.GetByOrderIdAsync(id);

				
				return Ok(orderDetails);
			
			
		}
	}
}

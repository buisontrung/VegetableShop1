
using System.ComponentModel.DataAnnotations;

namespace Order.ModelDto
{
	public class OrderRequestDTO
	{
		public string? Code { get; set; }
		[Required(ErrorMessage = "Tên người nhận không được để trống")]
		public string? CustomerName { get; set; }
		[Required(ErrorMessage = "Số điện thoại không được để trống")]
		public string? Phone { get; set; }
		[Required(ErrorMessage = "Địa chỉ không được để trống")]
		public string? Address { get; set; }
		public int? PromotionId { get; set; }

		public decimal TotalAmount { get; set; }
		public decimal? DiscountAmount { get; set; }
		public int Quantity { get; set; }
		public string? TypePayment { get; set; }
		public List<CartItemDto>? Items { get; set; }
	}
}

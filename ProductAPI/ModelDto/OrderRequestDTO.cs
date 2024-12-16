
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.ModelDto
{
	public class OrderRequestDTO
	{
		public int? Id { get; set; }
		public string? Code { get; set; }

		public string? Email { get; set; }
		public string? CustomerName { get; set; }

		public string? Phone { get; set; }
		
		public string? Address { get; set; }
		public int? PromotionId { get; set; }
		public string? UserId { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal? DiscountAmount { get; set; }
		public int Quantity { get; set; }
		public int? PaymentMethodId { get; set; }
		public DateTime? CreateDate { get; set; }
		public  int? Status { get; set; }
		public bool IsPayment { get; set; }
		public List<CartItemDto>? Items { get; set; }
		public PromotionDTO? Promotion { get; set; }
		public void CalculateTotals()
		{
			Code = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
			TotalAmount = Items?.Sum(item => item.Quantity * item.Price) + 20000 ?? 0;
			Quantity = Items?.Sum(item => item.Quantity) ?? 0;
			DiscountAmount = TotalAmount * (1 - (Promotion !=null ? Promotion.DiscountPercentage : 0));
		}
	}
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.ModelDto
{
	public class PromotionDTO
	{

		public int Id { get; set; }
		public string? PromotionCode { get; set; }  // Mã khuyến mãi
		public string? Description { get; set; }  // Mô tả chi tiết về khuyến mãi
		public decimal DiscountPercentage { get; set; }  // Phần trăm giảm giá

		public int Quantity { get; set; }
		public DateTime ValidFrom { get; set; }  // Ngày bắt đầu khuyến mãi
		public DateTime ValidTo { get; set; }  // Ngày kết thúc khuyến mãi
		public bool IsActive { get; set; }  // Khuyến mãi có đang hoạt động không

	}
}

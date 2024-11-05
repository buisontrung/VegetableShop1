using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Model
{
	public class Promotion	
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? PromotionCode { get; set; }  // Mã khuyến mãi
		public string? Description { get; set; }  // Mô tả chi tiết về khuyến mãi
		public decimal DiscountPercentage { get; set; }  // Phần trăm giảm giá

		public int Quantity { get; set; }
		public DateTime ValidFrom { get; set; }  // Ngày bắt đầu khuyến mãi
		public DateTime ValidTo { get; set; }  // Ngày kết thúc khuyến mãi
		public bool IsActive { get; set; }  // Khuyến mãi có đang hoạt động không
		public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
	}
}

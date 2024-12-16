using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Model
{
	public class Order
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? Code { get; set; }
		public string? Email { get; set; }
		public string? CustomerName { get; set; }

		public string? Phone { get; set; }

		public string? UserId { get; set; }
		public string? Address { get; set; }
		public int? PromotionId { get; set; }
		
		public decimal TotalAmount { get; set; }
		public decimal? DiscountAmount { get; set; }
		public int Quantity { get; set; }
		public int? PaymentMethodId { get; set; }
		public bool IsPayment { get; set; } =false;
		public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
		public int? Status { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
		
		public virtual Promotion? Promotion { get; set; }
		[ForeignKey("PaymentMethodId")]
		public virtual PaymentMethod? PaymentMethod { get; set; }
		public Order()
		{
			this.OrderDetails = new HashSet<OrderDetail>();
		}
		
	}
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Model
{
	public class Payment
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } 
		public int OrderId { get; set; } // Khóa ngoại tới bảng Order
		public int PaymentMethodId { get; set; } // Phương thức thanh toán
		public DateTime PaymentDate { get; set; } // Ngày thanh toán
		public bool IsPayment { get; set; } // Trạng thái thanh toán
		public virtual PaymentMethod? PaymentMethod { get; set; }
		public virtual Order? Order { get; set; } // Liên kết tới bảng Order
	}
}

namespace Order.Model
{
	public class Payment
	{
		public int Id { get; set; } 
		public int OrderId { get; set; } // Khóa ngoại tới bảng Order
		public decimal Amount { get; set; } // Số tiền thanh toán
		public int PaymentMethodId { get; set; } // Phương thức thanh toán
		public DateTime PaymentDate { get; set; } // Ngày thanh toán
		public bool IsPayment { get; set; } // Trạng thái thanh toán
		public virtual PaymentMethod? PaymentMethod { get; set; }
		public virtual Order? Order { get; set; } // Liên kết tới bảng Order
	}
}

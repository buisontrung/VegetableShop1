namespace Order.Model
{
	public class PaymentMethod
	{
		public int PaymentMethodId { get; set; } 
		public string? PaymentName { get; set; } 

		public virtual ICollection<Payment>? Payments { get; set; } 
	}
}

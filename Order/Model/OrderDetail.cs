using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Order.Model
{
	public class OrderDetail
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int OrderId { get; set; }


		public int ProductId { get; set; }

		public decimal Price { get; set; }
		public int Quantity { get; set; }

		public virtual Order? Order { get; set; }


	}
}

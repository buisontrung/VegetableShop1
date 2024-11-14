using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Model
{
	public class ShoppingCarts
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? UserId { get; set; }
		public int ProductVarianId { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }	
		public DateTime Created { get; set; }

		public bool? IsChecked { get; set; }
	}
}

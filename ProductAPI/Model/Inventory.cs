using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{
	public class Inventory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? Location { get; set; }
		public string? InventoryName { get; set; }
		public virtual ICollection<ProductInventorySupplier>? Suppliers { get; set; }
	}
}

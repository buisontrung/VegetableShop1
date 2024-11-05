using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ProductAPI.Model
{
	public class ProductImage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string? ImageUrl { get; set; }
		public int? ProductId { get; set; }
		public virtual Product? Product { get; set; }

	}
}

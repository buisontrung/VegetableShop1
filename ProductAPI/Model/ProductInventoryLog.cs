using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{
	public class ProductInventoryLog
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }  // Khóa chính cho bảng lịch sử nhập hàng

		public int? ProductInventorySupplierId { get; set; } // Khóa ngoại tới ProductInventorySupplier
		public ProductInventorySupplier? ProductInventorySupplier { get; set; } // Đối tượng liên kết với ProductInventorySupplier

		public int Quantity { get; set; } // Số lượng sản phẩm được nhập
		public DateTime EntryDate { get; set; } = DateTime.Now; // Ngày nhập hàng, mặc định là ngày hiện tại
	}
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Model
{
	public class RefreshToken
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } // Khóa chính

		public string UserId { get; set; } // Khóa ngoại đến AspNetUsers

		[ForeignKey("UserId")]
		public ApplicationUser? User { get; set; } // Điều hướng đến ApplicationUser

		public string Token { get; set; } // Giá trị token

		public string? ClientId { get; set; } // ClientId để xác định khách hàng nào yêu cầu token

		public DateTime Expiration { get; set; } // Thời gian hết hạn

		public bool IsRevoked { get; set; } // Trạng thái token (có bị thu hồi không)
	}
}

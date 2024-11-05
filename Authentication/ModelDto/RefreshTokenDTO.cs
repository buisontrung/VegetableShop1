using Authentication.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.ModelDto
{
	public class RefreshTokenDTO
	{
	

		public string? UserId { get; set; } // Khóa ngoại đến AspNetUsers



		public string? Token { get; set; } // Giá trị token

		public string? ClientId { get; set; } // ClientId để xác định khách hàng nào yêu cầu token

		public DateTime Expiration { get; set; } // Thời gian hết hạn

		public bool IsRevoked { get; set; }
	}
}

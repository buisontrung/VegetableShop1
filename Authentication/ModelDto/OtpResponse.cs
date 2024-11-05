using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.ModelDto
{
	public class OtpResponse
	{
		public string? Otptext { get; set; }

		public DateTime Expiration { get; set; }

	}
}

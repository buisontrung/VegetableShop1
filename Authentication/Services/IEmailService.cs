using Authentication.ModelDto;

namespace Authentication.Services
{
	public interface IEmailService
	{
		Task SendEmail(Otp mailrequest);
	}
}

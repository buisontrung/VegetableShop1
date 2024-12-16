

using Order.ModelDto;

namespace Order.Services
{
	public interface IEmailService
	{
		Task SendEmail(Otp mailrequest);
	}
}

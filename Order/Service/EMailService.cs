
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

using Order.Settings;
using Order.ModelDto;



namespace Order.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSetting emailSettings;
		public EmailService(IOptions<EmailSetting> options)
		{
			this.emailSettings = options.Value;
		}
		public async Task SendEmail(Otp mailrequest)
		{
			var email = new MimeMessage();
			email.Sender = MailboxAddress.Parse(emailSettings.Email);
			email.To.Add(MailboxAddress.Parse(mailrequest.Email));
			email.Subject = mailrequest.Subject;
			var builder = new BodyBuilder();
			builder.HtmlBody = mailrequest.Emailbody;
			email.Body = builder.ToMessageBody();

			using var smptp = new SmtpClient();
			smptp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
			smptp.Authenticate(emailSettings.Email, emailSettings.Password);
			await smptp.SendAsync(email);
			smptp.Disconnect(true);
		}
	}

}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace MlSuite.Api.Services
{
	public class EmailService
	{
		public EmailService()
		{

		}
		public async Task SendAsync(string to, string subject, string html, string? from = null)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(from ?? Secrets.EmailFrom));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;
			email.Body = new TextPart(TextFormat.Html) { Text = html };

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(Secrets.SmtpHost, Secrets.SmtpPort, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(Secrets.SmtpUser, Secrets.SmtpPassword);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}

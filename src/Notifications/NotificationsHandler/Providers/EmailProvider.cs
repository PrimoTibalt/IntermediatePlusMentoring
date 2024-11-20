using Resend;

namespace NotificationsHandler.Providers
{
	internal class EmailProvider(IResend emailClient) : INotificationProvider
	{
		const string FromEmail = "primotibalt <onbording@resend.dev>";

		private readonly IResend _emailClient = emailClient;

		public async Task Send(Message message)
		{
			var entity = new EmailMessage
			{
				From = FromEmail,
				To = message.To,
				Subject = message.Subject,
				HtmlBody = message.Body
			};

			await _emailClient.EmailSendAsync(entity);
		}
	}
}

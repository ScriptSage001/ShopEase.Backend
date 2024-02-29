using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Infrastructure.MailService
{
    /// <summary>
    /// Email ServiceS
    /// </summary>
    public class EmailService : IEmailService
    {
        #region Variables

        /// <summary>
        /// Instance of EmailSettings
        /// </summary>
        private readonly EmailSettings _emailSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for EmailService
        /// </summary>
        /// <param name="options"></param>
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To Send Mail
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        public async Task SendMailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = PrepareMail(mailRequest);

                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.SenderEmail, _emailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To Prepare Mail
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        private MimeMessage PrepareMail(MailRequest mailRequest)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_emailSettings.SenderEmail),
                Subject = mailRequest.Subject,
                Body = new BodyBuilder() { HtmlBody = mailRequest.Body }.ToMessageBody()
            };

            foreach (var recipient in mailRequest.Recipients)
            {
                email.To.Add(MailboxAddress.Parse(recipient));
            }

            return email;
        }

        #endregion
    }
}

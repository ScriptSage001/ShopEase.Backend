using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;
using System.Text;
using static ShopEase.Backend.AuthService.Core.CommonConstants.EmailConstants;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    /// <summary>
    /// SendWelcomeMailCommand Handler
    /// </summary>
    internal sealed class SendWelcomeMailCommandHandler : ICommandHandler<SendWelcomeMailCommand>
    {
        #region Variables

        /// <summary>
        /// Instance of EmailService
        /// </summary>
        private readonly IEmailService _emailService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SendWelcomeMailCommandHandler
        /// </summary>
        /// <param name="emailService"></param>
        public SendWelcomeMailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Method to Handle SendWelcomeMailCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> Handle(SendWelcomeMailCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var firstName = GetFirstName(command.Name);

                MailRequest mailRequest = new()
                {
                    Recipients = [command.Email],
                    Subject = Subject.Welcome,
                    Body = GenerateEmailBody(firstName)
                };

                await _emailService.SendMailAsync(mailRequest);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new("SendWelcomMailFailed", ex.Message));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To get the First name form the Full Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetFirstName(string name)
        {
            var nameArray = name.Split(" ");

            return nameArray?[0] ?? string.Empty;
        }

        /// <summary>
        /// TO Prepare the Email Body
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GenerateEmailBody(string name)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<html lang='en'><head><style>body{font-family:Arial,sans-serif;line-height:1.6;background-color:#f4f4f4;margin:0;padding:20px;}");
            stringBuilder.Append(".container{max-width:600px;margin:auto;background:#fff;padding:20px;border-radius:5px;box-shadow:0 0 10px rgba(0,0,0,0.1);}</style>");
            stringBuilder.Append("<body><div class='container'><h2>Welcome to ShopEase, {userName}!</h2>");
            stringBuilder.Append("<p>Thank you for choosing ShopEase for your online shopping needs. We're excited to have you on board.</p>");
            stringBuilder.Append("<p>If you have any questions or need assistance, feel free to contact our support team.</p>");
            stringBuilder.Append("<br/><br/><p>Happy shopping!<br>ShopEase Team</p></div></body></html>");

            var body = stringBuilder.ToString().Replace("{userName}", name);

            return body;
        }

        #endregion
    }
}

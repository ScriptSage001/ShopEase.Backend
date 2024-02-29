using System.Text;
using Microsoft.Extensions.Options;
using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Entities;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CommonConstants.EmailConstants;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    /// <summary>
    /// SendOtpCommand Handler
    /// </summary>
    internal sealed class SendOtpCommandHandler : ICommandHandler<SendOtpCommand>
    {
        #region Variables

        /// <summary>
        /// Instance of EmailService
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Instance of AuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        /// <summary>
        /// Instance of EmailSettings
        /// </summary>
        private readonly int _otpLifeSpan;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SendOtpCommandHandler
        /// </summary>
        /// <param name="emailService"></param>
        /// <param name="authServiceRepository"></param>
        /// <param name="options"></param>
        public SendOtpCommandHandler(IEmailService emailService, IAuthServiceRepository authServiceRepository, IOptions<EmailSettings> options)
        {
            _emailService = emailService;
            _authServiceRepository = authServiceRepository;
            _otpLifeSpan = options.Value.OtpLifeSpanInMinutes;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle Method For SendOtpCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> Handle(SendOtpCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var otp = GenerateOtp();

                MailRequest mailRequest = new()
                {
                    Recipients = [command.Email],
                    Subject = Subject.Otp,
                    Body = GenerateEmailBody(otp)
                };

                await _emailService.SendMailAsync(mailRequest);

                await SaveOtpDetails(command.Email, otp);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new("SendOtpFailed", ex.Message));
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Generates Random 6 Digit Otp
        /// </summary>
        /// <returns></returns>
        private static string GenerateOtp()
        {
            Random random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

        /// <summary>
        /// TO Prepare the Email Body
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        private string GenerateEmailBody(string otp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.Append("<html lang='en'><head><style>body{font-family:Arial,sans-serif;line-height:1.6;background-color:#f4f4f4;margin:0;padding:20px;}");
            stringBuilder.Append(".container{max-width:600px;margin:auto;background:#fff;padding:20px;border-radius:5px;box-shadow:0 0 10px rgba(0,0,0,0.1);}");
            stringBuilder.Append(".btn{display:inline-block;background:#007bff;color:#fff;text-decoration:none;padding:10px 20px;border-radius:5px;}</style></head>");
            stringBuilder.Append("<body><div class='container'><h2>Email Verification</h2>");
            stringBuilder.Append("<p>Thank you for signing up with ShopEase! To verify your email address, please use the following OTP: </p><h3> {otp} </h3>");
            stringBuilder.Append("<br/><p>This OTP is valid for only {otpLifeSpan} minutes.</p>");
            stringBuilder.Append("<p>If you didn't sign up for an account with us, you can safely ignore this email.</p>");
            stringBuilder.Append("<br/><br/><p>Best Regards,<br>ShopEase Team</p></div></body></html>");

            var body = stringBuilder.ToString();
            body = body.Replace("{otp}", otp);
            body = body.Replace("{otpLifeSpan}", _otpLifeSpan.ToString());

            return body;
        }

        /// <summary>
        /// To Save OTP Details in DB
        /// </summary>
        /// <param name="email"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        private async Task SaveOtpDetails(string email, string otp)
        {
            UserOtpDetails newUserOtpDetails = new()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Otp = otp,
                OtpExpiresOn = DateTime.Now.AddMinutes(_otpLifeSpan),
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _authServiceRepository.CreateUserOtpDetailAsync(newUserOtpDetails);
        }

        #endregion
    }
}

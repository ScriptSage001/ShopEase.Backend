using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command for Sending Otp
    /// </summary>
    /// <param name="Email"></param>
    public sealed record SendOtpCommand(string Email) : ICommand
    {
        /// <summary>
        /// Recipient Email for the OTP
        /// </summary>
        public string Email { get; set; } = Email;
    }
}

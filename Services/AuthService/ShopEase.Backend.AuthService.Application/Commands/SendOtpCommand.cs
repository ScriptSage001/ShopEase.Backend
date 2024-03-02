using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using static ShopEase.Backend.AuthService.Core.CommonConstants.EmailConstants;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command for Sending Otp
    /// </summary>
    /// <param name="Email"></param>
    /// <param name="OtpType"></param>
    public sealed record SendOtpCommand(string Email, OTPType OtpType) : ICommand
    {
        /// <summary>
        /// Recipient Email for the OTP
        /// </summary>
        public string Email { get; set; } = Email;

        /// <summary>
        /// Otp Type to Send
        /// </summary>
        public OTPType OtpType { get; set; } = OtpType;
    }
}

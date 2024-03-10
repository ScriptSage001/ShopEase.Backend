using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to reset password using OTP
    /// </summary>
    /// <param name="Request"></param>
    public sealed record ResetPasswordUsingOtpCommand(ResetPasswordUsingOtpRequest Request) : ICommand
    {
    }
}

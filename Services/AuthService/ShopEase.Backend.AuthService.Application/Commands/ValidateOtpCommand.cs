using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Validate Otp Command
    /// </summary>
    /// <param name="Request"></param>
    public sealed record ValidateOtpCommand(ValidateOtpRequest Request) : ICommand
    {
    }
}

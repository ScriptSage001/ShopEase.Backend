using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Generate Reset Password Token
    /// </summary>
    /// <param name="Email"></param>
    public sealed record GenerateResetPasswordTokenCommand(string Email) : ICommand<string>
    {
    }
}

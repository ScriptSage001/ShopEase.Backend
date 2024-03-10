using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Send Welcome Mail
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    public sealed record SendWelcomeMailCommand(string Name, string Email) : ICommand
    {
    }
}

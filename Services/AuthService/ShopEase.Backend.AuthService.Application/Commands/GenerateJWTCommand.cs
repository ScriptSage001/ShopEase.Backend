using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Generate new JWT
    /// </summary>
    /// <param name="email"></param>
    public sealed record GenerateJWTCommand(string Email) : ICommand<TokenModel>
    {
    }
}

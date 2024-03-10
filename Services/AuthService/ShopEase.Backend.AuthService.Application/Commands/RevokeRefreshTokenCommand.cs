using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Revoke Refresh Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="email"></param>
    public sealed record RevokeRefreshTokenCommand(Guid? UserId, string? Email) : ICommand
    {
    }
}

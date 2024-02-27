using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Revoke Refresh Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="email"></param>
    public sealed record RevokeRefreshTokenCommand(Guid? userId, string? email) : ICommand
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid? UserId { get; set; } = userId;

        /// <summary>
        /// User Enail
        /// </summary>
        public string? Email { get; set; } = email;
    }
}

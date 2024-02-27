using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    public sealed record RefreshTokenCommand(RefreshTokenRequest request) : ICommand<TokenModel>
    {
        public RefreshTokenRequest Request { get; set; } = request;
    }
}

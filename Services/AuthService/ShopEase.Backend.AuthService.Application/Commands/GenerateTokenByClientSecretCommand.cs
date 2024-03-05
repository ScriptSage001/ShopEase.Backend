using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Generate Token By ClientSecret
    /// </summary>
    /// <param name="ClientCredentials"></param>
    public sealed record GenerateTokenByClientSecretCommand(ClientCredentials ClientCredentials) : ICommand<TokenModel>
    {
        /// <summary>
        /// To get or init ClientCredentials
        /// </summary>
        public ClientCredentials ClientCredentials { get; init; } = ClientCredentials;
    }
}

using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Generate new JWT
    /// </summary>
    /// <param name="email"></param>
    public sealed record GenerateJWTCommand(string email) : ICommand<string>
    {
        /// <summary>
        /// Users Email
        /// </summary>
        public string Email { get; set; } = email;
    }
}

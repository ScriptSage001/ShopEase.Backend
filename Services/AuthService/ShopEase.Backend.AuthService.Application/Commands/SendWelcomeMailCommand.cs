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
        /// <summary>
        /// User's Full Name
        /// </summary>
        public string Name { get; set; } = Name;

        /// <summary>
        /// Recipients Email
        /// </summary>
        public string Email { get; set; } = Email;
    }
}

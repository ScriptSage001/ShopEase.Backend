using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to reset password using old password
    /// </summary>
    /// <param name="Request"></param>
    public sealed record ResetPasswordCommand(ResetPasswordRequest Request) : ICommand
    {
        /// <summary>
        /// To get or set ResetPasswordRequest
        /// </summary>
        public ResetPasswordRequest Request { get; set; } = Request;
    }
}

using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Validate Otp Command
    /// </summary>
    /// <param name="request"></param>
    public sealed record ValidateOtpCommand(ValidateOtpRequest request) : ICommand
    {
        /// <summary>
        /// ValidateOtpRequest
        /// </summary>
        public ValidateOtpRequest Request { get; set; } = request; 
    }
}

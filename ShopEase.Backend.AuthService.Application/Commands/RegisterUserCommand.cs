using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Register User
    /// </summary>
    /// <param name="userRegister"></param>
    public class RegisterUserCommand(UserRegisterDto userRegister) : ICommandAsync
    {
        /// <summary>
        /// User Register DTO
        /// </summary>
        public UserRegisterDto UserRegister { get; set; } = userRegister;

        /// <summary>
        /// Is User Created
        /// </summary>
        public bool IsUserCreated { get; set; }

        /// <summary>
        /// Nullable Error object
        /// </summary>
        public Error? Error { get; set; }
    }
}

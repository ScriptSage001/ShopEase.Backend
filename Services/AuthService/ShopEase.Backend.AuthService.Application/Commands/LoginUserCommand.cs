using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command for User Login
    /// </summary>
    /// <param name="userLogin"></param>
    public record LoginUserCommand(UserCredentialsDto userCredentials) : ICommandAsync
    {
        /// <summary>
        /// UserCredentials DTO
        /// </summary>
        public UserCredentialsDto UserCredentials { get; set; } = userCredentials;

        /// <summary>
        /// Result Object
        /// </summary>
        public Result<string> Result { get; set; }
    }
}

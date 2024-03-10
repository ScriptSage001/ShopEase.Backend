using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Register User
    /// </summary>
    /// <param name="userRegister"></param>
    public sealed record RegisterUserCommand(UserRegisterDto UserRegister) : ICommand
    {
    }
}

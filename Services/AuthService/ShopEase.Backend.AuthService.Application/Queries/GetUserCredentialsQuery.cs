using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Queries
{
    /// <summary>
    /// Query to Get UserCredentials
    /// </summary>
    /// <param name="email"></param>
    public record GetUserCredentialsQuery(string Email) : IQuery<UserCredentialsDto>
    {
    }
}

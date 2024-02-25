using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Queries
{
    /// <summary>
    /// Query to Get UserCredentials
    /// </summary>
    /// <param name="email"></param>
    public record GetUserCredentialsQuery(string email) : IQuery<UserCredentialsDto>
    {
        /// <summary>
        /// To get or set Email 
        /// </summary>
        public string Email {  get; set; } = email;
    }
}

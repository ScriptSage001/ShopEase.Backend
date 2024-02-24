using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Entities;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Queries
{
    /// <summary>
    /// Query to Get UserCredentials
    /// </summary>
    /// <param name="email"></param>
    public record GetUserCredentialsQuery(string email) : IQuery<Result<UserCredentialsDto>>
    {
        /// <summary>
        /// To get or set Email 
        /// </summary>
        public string Email {  get; set; } = email;
    }
}

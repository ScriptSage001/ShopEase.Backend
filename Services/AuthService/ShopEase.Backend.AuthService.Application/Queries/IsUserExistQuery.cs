using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;

namespace ShopEase.Backend.AuthService.Application.Queries
{
    /// <summary>
    /// Query to check if the User Exists
    /// </summary>
    public sealed record IsUserExistQuery(string Email) : IQuery<bool>
    {
    }
}

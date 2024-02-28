using ShopEase.Backend.AuthService.Core.Entities;

namespace ShopEase.Backend.AuthService.Application.Abstractions
{
    /// <summary>
    /// AuthService Repository
    /// </summary>
    public interface IAuthServiceRepository
    {
        /// <summary>
        /// To Create New User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> CreateUserAsync(User user);

        /// <summary>
        /// To Create New User Credentials
        /// </summary>
        /// <param name="userCredentials"></param>
        Task CreateUserCredentialsAsync(UserCredentials userCredentials);

        /// <summary>
        /// To fetch UserCredentials
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        UserCredentials? GetUserCredentials(string email);

        /// <summary>
        /// To fetch UserCredentials
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserCredentials? GetUserCredentials(Guid userId);

        /// <summary>
        /// To Update UserCredentials
        /// </summary>
        /// <param name="userCredentials"></param>
        Task UpdateUserCredentialsAsync(UserCredentials userCredentials);
    }
}

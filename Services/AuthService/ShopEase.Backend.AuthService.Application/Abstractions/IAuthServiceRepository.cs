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

        /// <summary>
        /// To Create UserOtpDetail
        /// </summary>
        /// <param name="userOtpDetails"></param>
        /// <returns></returns>
        Task CreateUserOtpDetailAsync(UserOtpDetails userOtpDetails);

        /// <summary>
        /// To Update UserOtpDetail
        /// </summary>
        /// <param name="userOtpDetails"></param>
        /// <returns></returns>
        Task UpdateUserOtpDetailAsync(UserOtpDetails userOtpDetails);

        /// <summary>
        /// To Get UserOTP Details
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        UserOtpDetails? GetUserOtpDetails(string email);

        /// <summary>
        /// To Delete UserOtpDetails
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task DeleteUserOtpDetailsAsync(string email);
    }
}

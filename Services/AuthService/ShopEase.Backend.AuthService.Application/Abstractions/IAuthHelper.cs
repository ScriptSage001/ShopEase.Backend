using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Abstractions
{
    /// <summary>
    /// Interface for AuthHelper
    /// </summary>
    public interface IAuthHelper
    {
        /// <summary>
        /// To Create Password Hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        // <summary>
        /// To Verify Password during Login
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns>boolean</returns>
        bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordSalt);

        /// <summary>
        /// To generate JWT
        /// </summary>
        /// <param name="email"></param>
        /// <returns>TokenModel</returns>
        TokenModel? CreateToken(string email);

        /// <summary>
        /// To Refresh JWT
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TokenModel? RefreshToken(RefreshTokenRequest request);

        /// <summary>
        /// To Generate Reset Password Token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string GenerateResetPasswordToken(string email);

        /// <summary>
        /// To Verify Reset Password Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool VerifyResetPasswordToken(string email, string token);

        /// <summary>
        /// To Validate ClientSecret
        /// </summary>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        bool ValidateClientSecret(string clientSecret);
    }

}

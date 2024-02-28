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
    }

}

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static ShopEase.Backend.AuthService.Core.CommonConstants.TokenConstants;

namespace ShopEase.Backend.AuthService.Application.Helper
{
    /// <summary>
    /// AuthHelper Implementaion
    /// </summary>
    public class AuthHelper : IAuthHelper
    {
        #region Variables

        /// <summary>
        /// Instance of AppSettings
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Instance of AuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for AuthHelper Class
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="authServiceRepository"></param>
        public AuthHelper(IOptions<AppSettings> appSettings, IAuthServiceRepository authServiceRepository)
        {
            _appSettings = appSettings.Value;
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To Create Password Hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// To Verify Password during Login
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns>boolean</returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            byte[] newHash;
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                newHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return newHash.SequenceEqual(passwordHash);
        }

        /// <summary>
        /// To generate JWT
        /// </summary>
        /// <param name="email"></param>
        /// <returns>TokenModel</returns>
        public TokenModel? CreateToken(string email)
        {
            var userCreds = _authServiceRepository.GetUserCredentials(email);

            if (userCreds != null)
            {
                var accessToken = GenerateAccessToken(email, out DateTime expirationTime);
                var refreshToken = GenerateRefreshToken(out int refreshTokenExpirationTime);
                
                userCreds.RefreshToken = refreshToken;
                userCreds.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTime == 0 ? 7 : refreshTokenExpirationTime);
                userCreds.UpdatedOn = DateTime.Now;

                _authServiceRepository.UpdateUserCredentialsAsync(userCreds);

                return new TokenModel() 
                { 
                    AccessToken = accessToken, 
                    RefreshToken = refreshToken, 
                    ExpirationTime = expirationTime
                };
            }

            return null;
        }

        /// <summary>
        /// To Refresh JWT
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TokenModel? RefreshToken(RefreshTokenRequest request)
        {
            string accessToken = request.AccessToken;
            string refreshToken = request.RefreshToken;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            var userEmail = jwtToken.Claims.First(c => c.Type == ClaimType.Email).Value;

            var userCreds = _authServiceRepository.GetUserCredentials(userEmail);

            if (userCreds == null || userCreds.RefreshToken != refreshToken || userCreds.RefreshTokenExpirationTime < DateTime.Now)
            {
                return null;
            }
            else
            {
                var newAccessToken = GenerateAccessToken(userEmail, out DateTime expirationTime);
                var newRefreshToken = GenerateRefreshToken(out int refreshTokenExpirationTime);

                userCreds.RefreshToken = newRefreshToken;
                userCreds.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTime == 0 ? 7 : refreshTokenExpirationTime);
                userCreds.UpdatedOn = DateTime.Now;

                _authServiceRepository.UpdateUserCredentialsAsync(userCreds);

                return new TokenModel()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpirationTime = expirationTime
                };
            }
        }

        /// <summary>
        /// To Generate Reset Password Token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GenerateResetPasswordToken(string email)
        {
            List<Claim> claims =
            [
                new Claim(ClaimType.Email, email),
                new Claim(ClaimType.TokenType, ClaimTypeValue.ResetPassword)
            ];

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            int.TryParse(_appSettings.ResetPasswordTokenExpirationTimeInMin, out int tokenExpirationTime);

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Issuer = _appSettings.Issuer,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpirationTime == 0 ? 5 : tokenExpirationTime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        /// <summary>
        /// To Verify Reset Password Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VerifyResetPasswordToken(string email, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userEmail = jwtToken.Claims.First(c => c.Type == ClaimType.Email)?.Value;
            var tokenType = jwtToken.Claims.First(c => c.Type == ClaimType.TokenType)?.Value;
            var expiresOn = jwtToken.ValidTo;

            return (email.Equals(userEmail, StringComparison.OrdinalIgnoreCase) 
                        && ClaimTypeValue.ResetPassword.Equals(tokenType, StringComparison.OrdinalIgnoreCase) 
                        && expiresOn >= DateTime.UtcNow);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To Generate Access Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        private string GenerateAccessToken(string email, out DateTime expirationTime)
        {
            List<Claim> claims =
            [
                new Claim(ClaimType.Email, email),
                new Claim(ClaimType.TokenType, ClaimTypeValue.AccessToken)
            ];

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            int.TryParse(_appSettings.TokenExpirationTime, out int tokenExpirationTime);

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Issuer = _appSettings.Issuer,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpirationTime == 0 ? 1440 : tokenExpirationTime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            expirationTime = token.ValidTo;
            return jwt;
        }

        /// <summary>
        /// To Generate Random Base64 Refresh Token
        /// </summary>
        /// <param name="refreshTokenExpirationTime"></param>
        /// <returns></returns>
        private string GenerateRefreshToken(out int refreshTokenExpirationTime)
        {
            int.TryParse(_appSettings.RefreshTokenExpirationTimeInDays, out refreshTokenExpirationTime);

            var randomNumber = new byte[64];

            using var randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(randomNumber);
            
            return Convert.ToBase64String(randomNumber);
        }


        #endregion
    }
}

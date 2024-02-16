using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopEase.Backend.AuthService.Application.Helper
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
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        // <summary>
        /// To Verify Password during Login
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns>boolean</returns>
        public bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordSalt);

        /// <summary>
        /// To generate JWT
        /// </summary>
        /// <param name="email"></param>
        /// <returns>string jwt</returns>
        public string CreateToken(string email);
    }

    /// <summary>
    /// AuthHelper Implementaion
    /// </summary>
    public class AuthHelper : IAuthHelper
    {
        #region Variables

        /// <summary>
        /// Instance of IConfiguration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Instance of AppSettings
        /// </summary>
        private readonly AppSettings _appSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for AuthHelper Class
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appSettings"></param>
        public AuthHelper(IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;

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
        /// <returns>string jwt</returns>
        public string CreateToken(string email)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Email, email)
            ];

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            int.TryParse(_appSettings.TokenExpirationTime, out int tokenExpirationTime);

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Issuer = _appSettings.Issuer,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(tokenExpirationTime == 0 ? 1440 : tokenExpirationTime),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        #endregion
    }
}

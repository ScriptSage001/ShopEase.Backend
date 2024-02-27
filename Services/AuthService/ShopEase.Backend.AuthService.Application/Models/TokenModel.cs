namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// Model Class for JWT and Refresh Token
    /// </summary>
    public sealed class TokenModel
    {
        /// <summary>
        /// JWT as Access Token
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Base64 string Refresh Token
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// JWT expiration time
        /// </summary>
        public DateTime ExpirationTime { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// Request DTO to Refresh JWT
    /// </summary>
    public sealed class RefreshTokenRequest
    {
        /// <summary>
        /// JWT as Access Token
        /// </summary>
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Base64 string Refresh Token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

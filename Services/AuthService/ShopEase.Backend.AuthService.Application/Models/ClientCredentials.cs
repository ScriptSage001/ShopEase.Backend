using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// DTO for ClientCredentials
    /// </summary>
    public sealed class ClientCredentials
    {
        /// <summary>
        /// To get or set User Email
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set ClientSecret
        /// </summary>
        [Required]
        public string ClientSecret { get; set; } = string.Empty;
    }
}

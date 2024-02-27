using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// DTO for User Login
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// To get or set Email 
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set Password
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

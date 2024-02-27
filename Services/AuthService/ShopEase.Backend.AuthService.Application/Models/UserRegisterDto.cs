using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// UserRegister Dto
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// To get or set Name
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// To get or set Email 
        /// </summary>
        [Required, EmailAddress, MinLength(6)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set MobileNumber
        /// </summary>
        [Required, MaxLength(13), MinLength(10)]
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// To get or set AltMobileNumber
        /// </summary>
        [MaxLength(13), MinLength(10)]
        public string? AltMobileNumber { get; set; }

        /// <summary>
        /// To get or set Password
        /// </summary>
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}

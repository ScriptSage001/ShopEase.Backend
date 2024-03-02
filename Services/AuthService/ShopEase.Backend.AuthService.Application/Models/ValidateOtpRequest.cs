using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// DTO for OTP Validation
    /// </summary>
    public sealed class ValidateOtpRequest
    {
        /// <summary>
        /// To get or set Email 
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set Otp
        /// </summary>
        [Required, StringLength(6)]
        public string Otp { get; set; } = string.Empty;
    }
}

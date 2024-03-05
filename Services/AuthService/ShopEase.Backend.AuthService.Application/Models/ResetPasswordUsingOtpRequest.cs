using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// DTO to reset password using OTP
    /// </summary>
    public sealed class ResetPasswordUsingOtpRequest
    {
        /// <summary>
        /// To get or set Email 
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set New Password
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// To get or set New Password
        /// </summary>
        [Required]
        public string ResetPasswordToken { get; set; } = string.Empty;
    }
}

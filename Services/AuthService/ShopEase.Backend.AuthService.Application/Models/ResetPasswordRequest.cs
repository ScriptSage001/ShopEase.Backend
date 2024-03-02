using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    /// <summary>
    /// DTO to reset password using old password
    /// </summary>
    public sealed class ResetPasswordRequest
    {
        /// <summary>
        /// To get or set Email 
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set Old Password
        /// </summary>
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// To get or set New Password
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// To get or set Confirmation Password
        /// </summary>
        [Required]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

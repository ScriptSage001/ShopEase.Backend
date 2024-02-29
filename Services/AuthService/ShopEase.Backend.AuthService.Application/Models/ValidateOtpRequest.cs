using System.ComponentModel.DataAnnotations;

namespace ShopEase.Backend.AuthService.Application.Models
{
    public sealed class ValidateOtpRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(6)]
        public string Otp { get; set; } = string.Empty;
    }
}

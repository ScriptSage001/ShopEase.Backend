using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopEase.Backend.AuthService.Core.Entities
{
    /// <summary>
    /// UserOtpDetails Entity Class
    /// </summary>
    [Table("UserOtpDetails", Schema = "Auth")]
    public sealed class UserOtpDetails
    {
        /// <summary>
        /// UserOtpDetails Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// EmailId of the User for Login Purpose
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// OTP
        /// </summary>
        public string Otp { get; set; } = string.Empty;

        /// <summary>
        /// OTP Expires On
        /// </summary>
        public DateTime OtpExpiresOn { get; set; }

        /// <summary>
        /// CreatedOn DateTime
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedOn DateTime
        /// </summary>
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}

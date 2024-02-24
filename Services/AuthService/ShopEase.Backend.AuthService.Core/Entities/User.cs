using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ShopEase.Backend.AuthService.Core.Entities
{
    /// <summary>
    /// User Entity Class
    /// </summary>
    [Table("User", Schema = "Users")]
    public sealed class User
    {
        #region Properties

        /// <summary>
        /// User Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Mobile Number
        /// </summary>
        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Alternate Mobile Number
        /// </summary>
        [AllowNull]
        public string AltMobileNumber { get; set; }

        /// <summary>
        /// CreatedOn DateTime
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedOn DateTime
        /// </summary>
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// RowStatus
        /// </summary>
        public bool RowStatus { get; set; } = true;

        #endregion
    }
}

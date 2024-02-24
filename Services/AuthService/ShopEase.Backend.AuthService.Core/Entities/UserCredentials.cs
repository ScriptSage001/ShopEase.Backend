using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopEase.Backend.AuthService.Core.Entities
{
    /// <summary>
    /// UserCredentials Entity Class
    /// </summary>
    [Table("UserCredentials", Schema = "Auth")]
    public sealed class UserCredentials
    {
        #region Properties

        /// <summary>
        /// UserCredentials Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Id of User Entity
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// EmailId of the User for Login Purpose
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password Hash
        /// </summary>
        [Required]
        public byte[] PasswordHash { get; set; } = new byte[32];

        /// <summary>
        /// Password Salt
        /// </summary>
        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[32];

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

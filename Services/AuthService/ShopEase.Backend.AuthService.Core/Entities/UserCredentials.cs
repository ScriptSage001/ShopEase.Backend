using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Core.Entities
{
    /// <summary>
    /// UserCredentials Entity Class
    /// </summary>
    public sealed class UserCredentials : Entity
    {
        #region Properties

        /// <summary>
        /// Id of User Entity
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Password Hash
        /// </summary>
        public byte[] PasswordHash { get; set; } = new byte[32];

        /// <summary>
        /// Password Salt
        /// </summary>
        public byte[] PasswordSalt { get; set; } = new byte[32];

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for UserCredentials
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createdOn"></param>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        public UserCredentials(Guid id, Guid userId, byte[] passwordHash, byte[] passwordSalt) : base(id)
        {
            UserId = userId;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        #endregion
    }
}

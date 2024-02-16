using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Core.Entities
{
    /// <summary>
    /// User Entity Class
    /// </summary>
    public sealed class User : Entity
    {
        #region Properties

        /// <summary>
        /// Full Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Alternate Mobile Number
        /// </summary>
        public string? AltMobileNumber { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for User Entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="mobileNumber"></param>
        /// <param name="altMobileNubler"></param>
        public User(Guid id, string name, string email, string mobileNumber, string? altMobileNubler) 
            : base(id)
        {
            Name = name;
            Email = email;
            MobileNumber = mobileNumber;
            AltMobileNumber = altMobileNubler;
        }

        #endregion
    }
}

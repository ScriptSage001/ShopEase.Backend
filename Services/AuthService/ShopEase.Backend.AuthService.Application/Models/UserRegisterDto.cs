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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// To get or set Email 
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// To get or set MobileNumber
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// To get or set AltMobileNumber
        /// </summary>
        public string? AltMobileNumber { get; set; }

        /// <summary>
        /// To get or set Password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}

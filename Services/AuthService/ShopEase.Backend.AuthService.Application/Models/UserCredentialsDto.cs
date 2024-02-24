namespace ShopEase.Backend.AuthService.Application.Models
{
    public class UserCredentialsDto
    {
        /// <summary>
        /// EmailId of the User for Login Purpose
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Password Hash
        /// </summary>
        public byte[] PasswordHash { get; set; } = new byte[32];

        /// <summary>
        /// Password Salt
        /// </summary>
        public byte[] PasswordSalt { get; set; } = new byte[32];
    }
}

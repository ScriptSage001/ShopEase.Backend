namespace ShopEase.Backend.AuthService.Application.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string TokenExpirationTime { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}

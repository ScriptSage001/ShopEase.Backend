namespace ShopEase.Backend.AuthService.Core
{
    public static class CommonConstants
    {
        /// <summary>
        /// Constants related to Emails
        /// </summary>
        public struct EmailConstants
        {
            /// <summary>
            /// Subject Lines for different Emails
            /// </summary>
            public struct Subject
            {
                public const string Otp = "OTP for Verification.";
                public const string Welcome = "Welcome to ShopEase!";
            }
        }
    }
}
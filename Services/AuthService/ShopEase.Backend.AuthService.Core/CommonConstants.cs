using System.Security.Cryptography.X509Certificates;

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
            /// Different OTP Types to send over Email
            /// </summary>
            public enum OTPType
            {
                VerifyEmail,
                ResetPassword,
                Login
            }

            /// <summary>
            /// Subject Lines for different Emails
            /// </summary>
            public struct Subject
            {
                public const string VerifyEmailOtp = "OTP for Email Verification";
                public const string ResetPasswordOtp = "OTP for Password Reset";
                public const string LoginOtp = "OTP for Login";
                public const string Welcome = "Welcome to ShopEase!";
            }
        }

        public struct TokenConstants
        {
            public struct ClaimType
            {
                public const string Email = "Email";
                public const string TokenType = "TokenType";
            }

            public struct ClaimTypeValue
            {
                public const string ResetPassword = "ResetPassword";
                public const string AccessToken = "AccessToken";
            }
        }
    }
}
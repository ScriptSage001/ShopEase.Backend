using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Core.CustomErrors
{
    public class CustomErrors
    {
        public struct CreateUserErrors
        {
            public static Error InternalError = new("InternalError", "CreateUser failed due to internal error at DB.");
            public static Error UserExists = new("UserExists", "CreateUser failed because user already exists.");            
        }

        public struct LoginUserErrors
        {
            public static Error UserDoesntExists = new("UserDoesntExists", "Login failed because user doesn't exists.");
            public static Error UserInactive = new("UserInactive", "Login failed because user is inactive.");
            public static Error IncorrectPassword = new("IncorrectPassword", "Login failed because password is incorrect.");
        }

        public struct ResetPasswordErrors
        {
            public static Error UserDoesntExists = new("UserDoesntExists", "ResetPassword failed because user doesn't exists.");
            public static Error UserInactive = new("UserInactive", "ResetPassword failed because user is inactive.");
            public static Error IncorrectPassword = new("IncorrectPassword", "ResetPassword failed because password is incorrect.");
            public static Error IncorrectToken = new("IncorrectToken", "ResetPassword failed because password reset token is incorrect.");
        }

        public struct AuthErrors
        {
            public static Error JwtGenerationFailed = new("JwtGenerationFailed", "Jwt Generation Failed.");
            public static Error ResetPasswordTokenGenerationFailed = new("ResetPasswordTokenGenerationFailed", "ResetPasswordToken Generation Failed.");
            public static Error TokenRefreshFailed = new("TokenRefreshFailed", "Token Refresh Failed Due to Invalid Access or Refresh Token.");
            public static Error RevokeRefreshTokenFailed = new("RevokeRefreshTokenFailed", "Revoke Refresh Token Failed due to Incorrect Credentials.");
        }

        public struct OtpErrors
        {
            public static Error ValidationFailed = new("ValidationFailed", "OTP Validation Failed, Please Try Again.");
            public static Error UserDoesntExists = new("UserDoesntExists", "Send OTP failed because user doesn't exists.");
        }
    }
}

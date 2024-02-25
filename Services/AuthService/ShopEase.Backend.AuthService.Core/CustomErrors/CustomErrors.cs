using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Core.CustomErrors
{
    public static class CustomErrors
    {
        public struct CreateUserErrors
        {
            public static Error InternalError = new("InternalError", "CreateUser failed due to internal error at DB.");
            public static Error UserExists = new("UserExists", "CreateUser failed because user already exists.");            
        }

        public struct LoginUserErrors
        {
            public static Error UserDoesntExists = new("UserDoesntExists", "GetUser failed because user doesn't exists.");
            public static Error UserInactive = new("UserInactive", "GetUser failed because user is inactive.");
            public static Error IncorrectPassword = new("IncorrectPassword", "Login failed because password is incorrect.");
        }

        public struct AuthErrors
        {
            public static Error JwtGenerationFailed = new("JwtGenerationFailed", "Jwt Generation Failed.");
        }
    }
}

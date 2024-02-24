namespace ShopEase.Backend.AuthService.Core.CustomErrors
{
    public static class CustomErrors
    {
        public struct CreateUserErrors
        {
            public struct InternalError
            {
                public static string Code = "InternalError";
                public static string Message = "CreateUser failed due to internal error at DB.";
            }

            public struct UserExists
            {
                public static string Code = "UserExists";
                public static string Message = "CreateUser failed because user already exists.";
            }
        }

        public struct LoginUserErrors
        {
            public struct UserDoesntExists
            {
                public static string Code = "UserDoesntExists";
                public static string Message = "GetUser failed because user doesn't exists.";
            }

            public struct UserInactive
            {
                public static string Code = "UserInactive";
                public static string Message = "GetUser failed because user is inactive.";
            }

            public struct IncorrectPassword
            {
                public static string Code = "IncorrectPassword";
                public static string Message = "Login failed because password is incorrect.";
            }
        }
    }
}

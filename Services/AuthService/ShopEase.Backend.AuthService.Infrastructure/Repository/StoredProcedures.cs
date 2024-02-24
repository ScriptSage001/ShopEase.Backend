namespace ShopEase.Backend.AuthService.Infrastructure.Repository
{
    public class StoredProcedures
    {
        public static readonly string CreateUser = "user.spu_createuser";
        public static readonly string CreateUserCredentials = "auth.spu_createusercredentials";

        public static readonly string GetUserCredentialsByEmail = "auth.spu_getusercredentials";
    }
}

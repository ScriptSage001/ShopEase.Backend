using ShopEase.Backend.AuthService.Core.Entities;

namespace ShopEase.Backend.AuthService.Application.Abstractions
{
    public interface IAuthServiceRepository
    {
        int CreateUser(User user);

        void CreateUserCredentials(UserCredentials userCredentials);

        UserCredentials? GetUserCredentials(string email);
    }
}

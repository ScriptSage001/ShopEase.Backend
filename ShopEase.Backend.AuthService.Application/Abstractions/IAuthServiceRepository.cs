using ShopEase.Backend.AuthService.Core.Entities;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application
{
    public interface IAuthServiceRepository
    {
        Error RegisterUser(User user);
    }
}

using MediatR;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}

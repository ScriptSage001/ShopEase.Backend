using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator
{
    /// <summary>
    /// Custom Mediator Service for Command Query Implementaiton
    /// </summary>
    public interface IApiService
    {
        #region Request Methods

        Task<Result<TResponse>> RequestAsync<TResponse>(IQuery<TResponse> query);

        Result<TResponse> Request<TResponse>(IQuery<TResponse> query);

        #endregion

        #region Send Methods

        Result Send<TCommand>(TCommand command) where TCommand : ICommand;

        Task<Result> SendAsync<TCommand>(TCommand command) where TCommand : ICommand;

        Result<TResponse> Send<TResponse>(ICommand<TResponse> command);

        Task<Result<TResponse>> SendAsync<TResponse>(ICommand<TResponse> command);

        #endregion
    }
}

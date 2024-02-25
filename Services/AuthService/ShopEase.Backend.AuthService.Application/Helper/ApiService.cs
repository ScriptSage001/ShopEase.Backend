﻿using MediatR;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Helper
{
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

    public class ApiService : IApiService
    {
        private readonly IMediator _mediator;

        public ApiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Request Methods

        public Task<Result<TResponse>> RequestAsync<TResponse>(IQuery<TResponse> query)
        {
            ArgumentNullException.ThrowIfNull(nameof(query));

            return _mediator.Send(query);
        }

        public Result<TResponse> Request<TResponse>(IQuery<TResponse> query)
        {
            ArgumentNullException.ThrowIfNull(nameof(query));

            return _mediator.Send(query).Result;
        }

        #endregion

        #region Send Methods

        public Result Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            return _mediator.Send(command).Result;
        }

        public Task<Result> SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            return _mediator.Send(command);
        }

        public Result<TResponse> Send<TResponse>(ICommand<TResponse> command)
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            return _mediator.Send(command).Result;
        }

        public Task<Result<TResponse>> SendAsync<TResponse>(ICommand<TResponse> command)
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            return _mediator.Send(command);
        }

        #endregion
    }
}

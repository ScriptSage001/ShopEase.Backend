using MediatR;

namespace ShopEase.Backend.AuthService.Application.Helper
{
    public interface IQuery<T> : IRequest<T> { }
    public interface IQueryHandler<TQuery, T> : IRequestHandler<TQuery, T> where TQuery : IQuery<T> { }
    public interface ICommand : IRequest { }
    public interface ICommand<T> : IRequest<T> { }
    public interface ICommandAsync : IRequest { }
    public interface ICommandHandler<TCommand, T> : IRequestHandler<TCommand, T> where TCommand : ICommand<T> { }
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand { }
    public interface ICommandHandlerAsync<TCommand> : IRequestHandler<TCommand> where TCommand : ICommandAsync { }

    public interface IApiService
    {
        Task<T> RequestAsync<T>(IQuery<T> query);

        T Request<T>(IQuery<T> query);

        void Send<TCommand>(TCommand command) where TCommand : ICommand;

        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommandAsync;
    }

    public class ApiService : IApiService
    {
        private readonly IMediator _mediator;

        public ApiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<T> RequestAsync<T>(IQuery<T> query)
        {
            ArgumentNullException.ThrowIfNull(nameof(query));

            return _mediator.Send<T>(query);
        }

        public T Request<T>(IQuery<T> query)
        {
            ArgumentNullException.ThrowIfNull(nameof(query));

            return _mediator.Send<T>(query).Result;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            _mediator.Send(command);
        }

        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommandAsync
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            return _mediator.Send(command);
        }

        public T Send<T>(ICommand<T> command)
        {
            ArgumentNullException.ThrowIfNull(nameof(command));

            return _mediator.Send<T>(command).Result;
        }
    }
}

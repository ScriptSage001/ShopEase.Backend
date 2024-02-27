using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class RevokeRefreshTokenCommandHandler : ICommandHandler<RevokeRefreshTokenCommand>
    {
        #region Variables

        /// <summary>
        /// Instance of AuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for RevokeRefreshTokenCommandHandler
        /// </summary>
        /// <param name="authServiceRepository"></param>
        public RevokeRefreshTokenCommandHandler(IAuthServiceRepository authServiceRepository)
        {
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle Method for RevokeRefreshTokenCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var userCreds = command.userId != null ? 
                                    _authServiceRepository.GetUserCredentials((Guid)command.UserId) : 
                                    _authServiceRepository.GetUserCredentials(command.Email);

            if (userCreds != null)
            {
                userCreds.RefreshToken = null;
                userCreds.UpdatedOn = DateTime.Now;

                _authServiceRepository.UpdateUserCredentials(userCreds);

                return Task.FromResult(Result.Success());
            }
            else
            {
                return Task.FromResult(Result.Failure(AuthErrors.RevokeRefreshTokenFailed));
            }
        }

        #endregion
    }
}

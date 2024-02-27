using MediatR;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenModel>
    {
        #region Variables

        /// <summary>
        /// Instance of AuthHelper
        /// </summary>
        private readonly IAuthHelper _authHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for RefreshTokenCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        public RefreshTokenCommandHandler(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle mehtod for RefreshTokenCommandHandler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<Result<TokenModel>> IRequestHandler<RefreshTokenCommand, Result<TokenModel>>.Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var refreshTokenResponse = _authHelper.RefreshToken(command.Request);

            if (refreshTokenResponse == null)
            {
                return Task.FromResult(Result.Failure<TokenModel>(AuthErrors.TokenRefreshFailed));
            }
            else
            {
                return Task.FromResult(Result.Success(refreshTokenResponse));
            }
        }

        #endregion
    }
}

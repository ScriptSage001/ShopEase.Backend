﻿using ShopEase.Backend.AuthService.Application.Abstractions;
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
        public async Task<Result> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var userCreds = command.UserId != null ? 
                                    _authServiceRepository.GetUserCredentials((Guid)command.UserId) : 
                                    _authServiceRepository.GetUserCredentials(command.Email);

            if (userCreds != null)
            {
                userCreds.RefreshToken = null;
                userCreds.UpdatedOn = DateTime.Now;

                await _authServiceRepository.UpdateUserCredentialsAsync(userCreds);

                return Result.Success();
            }
            else
            {
                return Result.Failure(AuthErrors.RevokeRefreshTokenFailed);
            }
        }

        #endregion
    }
}

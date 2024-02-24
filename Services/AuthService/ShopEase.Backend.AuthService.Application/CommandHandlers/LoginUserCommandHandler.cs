﻿using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class LoginUserCommandHandler : ICommandHandlerAsync<LoginUserCommand>
    {
        #region Variables

        /// <summary>
        /// Instance of AuthHelper
        /// </summary>
        private readonly IAuthHelper _authHelper;

        /// <summary>
        /// Instance of AuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for LoginUserCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        /// <param name="authServiceRepository"></param>
        public LoginUserCommandHandler(IAuthHelper authHelper, IAuthServiceRepository authServiceRepository)
        {
            _authHelper = authHelper;
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle method for LoginUserCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var isVerified = _authHelper.VerifyPasswordHash(command.UserCredentials.Password,
                                            command.UserCredentials.PasswordHash,
                                            command.UserCredentials.PasswordSalt);

            if (isVerified)
            {
                var token = _authHelper.CreateToken(command.UserCredentials.Email);
                command.Result = token;
            }
            else
            {
                command.Result = Result.Failure<string>(
                                    new(LoginUserErrors.IncorrectPassword.Code, LoginUserErrors.IncorrectPassword.Message));
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}

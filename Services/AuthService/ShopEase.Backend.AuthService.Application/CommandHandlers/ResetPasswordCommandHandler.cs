using MediatR;
using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Core.Primitives;
using System.Security.AccessControl;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
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
        /// Constructor for ResetPasswordCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        /// <param name="authServiceRepository"></param>
        public ResetPasswordCommandHandler(IAuthHelper authHelper, IAuthServiceRepository authServiceRepository)
        {
            _authHelper = authHelper;
            _authServiceRepository = authServiceRepository;

        }

        #endregion

        #region Handle Method

        public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var userCreds = _authServiceRepository.GetUserCredentials(command.Request.Email);

            if (userCreds != null)
            {
                var isVerified = _authHelper.VerifyPasswordHash(command.Request.OldPassword, userCreds.PasswordHash, userCreds.PasswordSalt);

                if (isVerified)
                {
                    _authHelper.CreatePasswordHash(command.Request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                    userCreds.PasswordHash = passwordHash;
                    userCreds.PasswordSalt = passwordSalt;
                    userCreds.UpdatedOn = DateTime.Now;

                    await _authServiceRepository.UpdateUserCredentialsAsync(userCreds);

                    return Result.Success();
                }
                else
                {
                    return Result.Failure(LoginUserErrors.IncorrectPassword);
                }
            }
            else
            {
                return Result.Failure(LoginUserErrors.UserDoesntExists);
            }
        }

        #endregion
    }
}

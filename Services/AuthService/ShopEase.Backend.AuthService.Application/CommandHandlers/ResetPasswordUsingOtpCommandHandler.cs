using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class ResetPasswordUsingOtpCommandHandler : ICommandHandler<ResetPasswordUsingOtpCommand>
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
        /// Constructor for ResetPasswordUsingOtpCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        /// <param name="authServiceRepository"></param>
        public ResetPasswordUsingOtpCommandHandler(IAuthHelper authHelper, IAuthServiceRepository authServiceRepository)
        {
            _authHelper = authHelper;
            _authServiceRepository = authServiceRepository;

        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handler Method for ResetPasswordUsingOtpCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> Handle(ResetPasswordUsingOtpCommand command, CancellationToken cancellationToken)
        {
            var userCreds = _authServiceRepository.GetUserCredentials(command.Request.Email);

            if (userCreds != null)
            {
                var isVerified = _authHelper.VerifyResetPasswordToken(command.Request.Email, command.Request.ResetPasswordToken);

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
                    return Result.Failure(ResetPasswordErrors.IncorrectToken);
                }
            }
            else
            {
                return Result.Failure(ResetPasswordErrors.UserDoesntExists);
            }
        }

        #endregion
    }
}

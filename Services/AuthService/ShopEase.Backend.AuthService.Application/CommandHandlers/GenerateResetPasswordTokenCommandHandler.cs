using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class GenerateResetPasswordTokenCommandHandler : ICommandHandler<GenerateResetPasswordTokenCommand, string>
    {
        #region Variables

        /// <summary>
        /// Instance of AuthHelper
        /// </summary>
        private readonly IAuthHelper _authHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for GenerateResetPasswordTokenCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        public GenerateResetPasswordTokenCommandHandler(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Hadle method to Handle GenerateResetPasswordTokenCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result<string>> Handle(GenerateResetPasswordTokenCommand command, CancellationToken cancellationToken)
        {
            var token = _authHelper.GenerateResetPasswordToken(command.Email);

            if (token != null)
            {
                return Task.FromResult(Result.Success(token));
            }

            return Task.FromResult(Result.Failure<string>(AuthErrors.ResetPasswordTokenGenerationFailed));
        }

        #endregion
    }
}

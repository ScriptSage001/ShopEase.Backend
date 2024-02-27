using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    /// <summary>
    /// Hnadler for GenerateJWTCommand
    /// </summary>
    internal sealed class GenerateJWTCommandHandler : ICommandHandler<GenerateJWTCommand, TokenModel>
    {
        #region Variables

        /// <summary>
        /// Instance for AuthHelper
        /// </summary>
        private readonly IAuthHelper _authHelper;

        #endregion

        #region  Constructor

        /// <summary>
        /// Constructor for GenerateJWTCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        public GenerateJWTCommandHandler(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handler method for GenerateJWTCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result<TokenModel>> Handle(GenerateJWTCommand command, CancellationToken cancellationToken)
        {
            // Generate Token
            var token = _authHelper.CreateToken(command.Email);

            if (token == null)
            {
                return Task.FromResult(Result.Failure<TokenModel>(AuthErrors.JwtGenerationFailed));
            }
            return Task.FromResult(Result.Success(token));
        }

        #endregion
    }
}

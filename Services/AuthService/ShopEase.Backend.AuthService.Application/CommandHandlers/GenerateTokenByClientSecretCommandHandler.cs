using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    internal sealed class GenerateTokenByClientSecretCommandHandler : ICommandHandler<GenerateTokenByClientSecretCommand, TokenModel>
    {
        #region Variables

        /// <summary>
        /// Instance for AuthHelper
        /// </summary>
        private readonly IAuthHelper _authHelper;

        /// <summary>
        /// Instance of AuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region  Constructor

        /// <summary>
        /// Constructor for GenerateTokenByClientSecretCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        /// <param name="authServiceRepository"></param>
        public GenerateTokenByClientSecretCommandHandler(IAuthHelper authHelper, IAuthServiceRepository authServiceRepository)
        {
            _authHelper = authHelper;
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handler method for GenerateTokenByClientSecretCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result<TokenModel>> Handle(GenerateTokenByClientSecretCommand command, CancellationToken cancellationToken)
        {
            // Validate User
            var isUserExists = _authServiceRepository.IsUserExists(command.ClientCredentials.Email);

            // Validate Secret
            var isSecretValidated = _authHelper.ValidateClientSecret(command.ClientCredentials.ClientSecret);

            if (isUserExists && isSecretValidated)
            {
                // Generate Token
                var token = _authHelper.CreateToken(command.ClientCredentials.Email);
                if (token == null)
                {
                    return Task.FromResult(Result.Failure<TokenModel>(AuthErrors.JwtGenerationFailed));
                }
                return Task.FromResult(Result.Success(token));
            }

            return Task.FromResult(Result.Failure<TokenModel>(AuthErrors.JwtGenerationFailed));
        }

        #endregion
    }
}

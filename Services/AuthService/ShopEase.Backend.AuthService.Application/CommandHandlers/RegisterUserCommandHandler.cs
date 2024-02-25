using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Core.Entities;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    /// <summary>
    /// Handler for RegisterUserCommand
    /// </summary>
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
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
        /// Constructor for RegisterUserCommandHandler
        /// </summary>
        /// <param name="authHelper"></param>
        /// <param name="authServiceRepository"></param>
        public RegisterUserCommandHandler(IAuthHelper authHelper, IAuthServiceRepository authServiceRepository)
        {
            _authHelper = authHelper;
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle method for RegisterUserCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = command.UserRegister.Name,
                Email = command.UserRegister.Email,
                MobileNumber = command.UserRegister.MobileNumber,
                AltMobileNumber = command.UserRegister.AltMobileNumber,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                RowStatus = true
            };

            // Tech Debt - Move User to UserService
            var createUserResult = _authServiceRepository.CreateUser(user);

            if (createUserResult > 0)
            {
                CreateUserCredentials(command.UserRegister.Password, user.Id, user.Email);

                return Task.FromResult(Result.Success());
            }
            else
            {
                return createUserResult == -1 ?
                    Task.FromResult(Result.Failure(CreateUserErrors.UserExists)) : 
                    Task.FromResult(Result.Failure(CreateUserErrors.InternalError));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To create and save User Credentials
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        private void CreateUserCredentials(string password, Guid userId, string email)
        {
            _authHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            UserCredentials userCredentials = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _authServiceRepository.CreateUserCredentials(userCredentials);
        }

        #endregion
    }
}

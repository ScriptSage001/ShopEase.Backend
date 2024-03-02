using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Entities;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    /// <summary>
    /// ValidateOtpCommand Handler
    /// </summary>
    internal sealed class ValidateOtpCommandHandler : ICommandHandler<ValidateOtpCommand>
    {
        #region Variables

        /// <summary>
        /// Instance of AuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for ValidateOtpCommandHandler
        /// </summary>
        /// <param name="authServiceRepository"></param>
        public ValidateOtpCommandHandler(IAuthServiceRepository authServiceRepository)
        {
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle method to ValidateOtpCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> Handle(ValidateOtpCommand command, CancellationToken cancellationToken)
        {
            var userOtpDetails = _authServiceRepository.GetUserOtpDetails(command.Request.Email);

            if (userOtpDetails == null)
            {
                return Result.Failure(OtpErrors.ValidationFailed);
            }

            if (IsOtpValid(command.Request, userOtpDetails))
            {
                await _authServiceRepository.DeleteUserOtpDetailsAsync(command.Request.Email);

                return Result.Success();
            }

            return Result.Failure(OtpErrors.ValidationFailed);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To Check if the OTP is Valid or not
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userOtpDetails"></param>
        /// <returns></returns>
        private static bool IsOtpValid(ValidateOtpRequest request, UserOtpDetails userOtpDetails)
        {
            return request.Otp.Equals(userOtpDetails.Otp) && userOtpDetails.OtpExpiresOn >= DateTime.Now;
        }

        #endregion
    }
}

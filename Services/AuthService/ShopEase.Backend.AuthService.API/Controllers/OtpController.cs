using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Application.Queries;
using static ShopEase.Backend.AuthService.Core.CommonConstants.EmailConstants;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.API.Controllers
{
    /// <summary>
    /// Controller for OTP Services
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        #region Variables

        /// <summary>
        /// Instance of IApiService
        /// </summary>
        private readonly IApiService _apiService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for OtpController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="apiService"></param>
        public OtpController(IApiService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Public EndPoints

        /// <summary>
        /// To Send Otp For Email Verification
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send/verify-email/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendOtpForEmailVerification(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest($"ErrorCode: InvalidEmail, ErrorMessage: Please provide valid UserEmail.");
                }

                var result = await _apiService.SendAsync(new SendOtpCommand(email, OTPType.VerifyEmail));

                if (result.IsSuccess)
                {
                    return Ok($"Otp sent successfully to the following email address: {email}");
                }

                return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during SendOtpForEmailVerification. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Send Otp To Reset Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send/forgot-password/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendOtpToResetPassword(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest($"ErrorCode: InvalidEmail, ErrorMessage: Please provide valid UserEmail.");
                }

                var result = await _apiService.SendAsync(new SendOtpCommand(email, OTPType.ResetPassword));

                if (result.IsSuccess)
                {
                    return Ok($"Otp sent successfully to the following email address: {email}");
                }

                return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during SendOtpToResetPassword. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Send Otp For Login Without Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send/login/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendOtpForLogin(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest($"ErrorCode: InvalidEmail, ErrorMessage: Please provide valid UserEmail.");
                }

                var isUserExists = await _apiService.RequestAsync(new IsUserExistQuery(email));

                if (isUserExists.IsFailure)
                {
                    return BadRequest($"ErrorCode: {isUserExists.Error?.Code}, ErrorMessage: {isUserExists.Error?.Message}");
                }
                else
                {
                    if (isUserExists.Value)
                    {
                        var result = await _apiService.SendAsync(new SendOtpCommand(email, OTPType.Login));

                        if (result.IsSuccess)
                        {
                            return Ok($"Otp sent successfully to the following email address: {email}");
                        }

                        return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
                    }

                    return BadRequest($"ErrorCode: {OtpErrors.UserDoesntExists.Code}, ErrorMessage: {OtpErrors.UserDoesntExists.Message}");
                }                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during SendOtpForLogin. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Validate Otp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("validate")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidateOtp(ValidateOtpRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: InvalidRequest, ErrorMessage: Please provide valid Request.");
                }

                var result = await _apiService.SendAsync(new ValidateOtpCommand(request));

                if (result.IsSuccess)
                {
                    return Ok($"Otp validated successfully.");
                }

                return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during ValidateOtp. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Validate Otp For Password Reset
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("validate/forgot-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidateOtpForResetPassword(ValidateOtpRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: InvalidRequest, ErrorMessage: Please provide valid Request.");
                }

                var result = await _apiService.SendAsync(new ValidateOtpCommand(request));

                if (result.IsSuccess)
                {
                    var resetPasswordToken = await _apiService.SendAsync(new GenerateResetPasswordTokenCommand(request.Email));
                    return Ok(resetPasswordToken.Value);
                }

                return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during ValidateOtp. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Application.Queries;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CommonConstants.EmailConstants;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.API.Controllers
{
    /// <summary>
    /// Controller for Auth Services
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Variables

        /// <summary>
        /// Instance of IApiService
        /// </summary>
        private readonly IApiService _apiService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for AuthController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="apiService"></param>
        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Public EndPoints

        #region Register & Login

        /// <summary>
        /// To Register new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: RequestEmpty, ErrorMessage: Request is empty. Please provide valid request.");
                }

                var command = new RegisterUserCommand(request);
                var registrationResult = await _apiService.SendAsync(command);

                if (registrationResult.IsFailure)
                {
                    return BadRequest($"ErrorCode: {registrationResult.Error?.Code}, ErrorMessage: {registrationResult.Error?.Message}");
                }
                else
                {
                    var tokenResult = await _apiService.SendAsync(new GenerateJWTCommand(request.Email));

                    if (tokenResult.IsSuccess)
                    {
                        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        _apiService.SendAsync(new SendWelcomeMailCommand(request.Name, request.Email));
                        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                        return Ok(tokenResult.Value);
                    }
                    else
                    {
                        return StatusCode(500, $"ErrorCode: {tokenResult.Error?.Code}, ErrorMessage: {tokenResult.Error?.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during Registration. Message: {ex.Message}. " +
                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Login a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: RequestEmpty, ErrorMessage: Request is empty. Please provide valid request.");
                }

                var userCredsResult = await _apiService.RequestAsync(new GetUserCredentialsQuery(request.Email));

                if (userCredsResult.IsFailure)
                {
                    return BadRequest($"ErrorCode: {userCredsResult.Error?.Code}, ErrorMessage: {userCredsResult.Error?.Message}");
                }
                else
                {
                    var userCredentials = userCredsResult.Value;
                    userCredentials.Password = request.Password;

                    var command = new LoginUserCommand(userCredentials);
                    var result = await _apiService.SendAsync(command);

                    if (result.IsFailure)
                    {
                        return Unauthorized($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
                    }
                    else
                    {
                        var tokenResult = await _apiService.SendAsync(new GenerateJWTCommand(request.Email));

                        if (tokenResult.IsSuccess)
                        {
                            return Ok(tokenResult.Value);
                        }
                        else
                        {
                            return StatusCode(500, $"ErrorCode: {tokenResult.Error?.Code}, ErrorMessage: {tokenResult.Error?.Message}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during Login. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Login a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login/otp")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginUsingOtp([FromBody] ValidateOtpRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: RequestEmpty, ErrorMessage: Request is empty. Please provide valid request.");
                }                                
                else
                {
                    var result = await _apiService.SendAsync(new ValidateOtpCommand(request));

                    if (result.IsFailure)
                    {
                        return Unauthorized($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
                    }
                    else
                    {
                        var tokenResult = await _apiService.SendAsync(new GenerateJWTCommand(request.Email));

                        if (tokenResult.IsSuccess)
                        {
                            return Ok(tokenResult.Value);
                        }
                        else
                        {
                            return StatusCode(500, $"ErrorCode: {tokenResult.Error?.Code}, ErrorMessage: {tokenResult.Error?.Message}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during Login. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #region Password

        /// <summary>
        /// Reset Password using old password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("password/reset")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: RequestEmpty, ErrorMessage: Request is empty. Please provide valid request.");
                }
                else
                {
                    var result = await _apiService.SendAsync(new ResetPasswordCommand(request));

                    if (result.IsSuccess)
                    {
                        return Ok("Password Changed Successfully!");
                    }

                    return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during ResetPassword. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #region Refresh Token

        /// <summary>
        /// To Refresh Tokens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: RequestEmpty, ErrorMessage: Request is empty. Please provide valid request.");
                }

                var refreshResult = await _apiService.SendAsync(new RefreshTokenCommand(request));

                if (refreshResult.IsFailure)
                {
                    return Unauthorized($"ErrorCode: {refreshResult.Error?.Code}, ErrorMessage: {refreshResult.Error?.Message}");
                }
                else
                {
                    return Ok(refreshResult.Value);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during Token Refresh. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Revoke Refresh Tokens by User Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("revoke/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeRefreshToken(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest($"ErrorCode: InvalidEmail, ErrorMessage: Please provide valid UserEmail.");
                }

                var revokeResult = await _apiService.SendAsync(new RevokeRefreshTokenCommand(null, email));

                if (revokeResult.IsFailure)
                {
                    return Unauthorized($"ErrorCode: {revokeResult.Error?.Code}, ErrorMessage: {revokeResult.Error?.Message}");
                }
                else
                {
                    return Ok(revokeResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during Revoke Refresh Token. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Revoke Refresh Tokens by User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("revoke/{id:Guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeRefreshToken(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest($"ErrorCode: InvalidUserId, ErrorMessage: Please provide valid UserId.");
                }

                var revokeResult = await _apiService.SendAsync(new RevokeRefreshTokenCommand(id, null));

                if (revokeResult.IsFailure)
                {
                    return Unauthorized($"ErrorCode: {revokeResult.Error?.Code}, ErrorMessage: {revokeResult.Error?.Message}");
                }
                else
                {
                    return Ok(revokeResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during Revoke Refresh Token. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #region OTP

        /// <summary>
        /// To Send Otp For Email Verification
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("email/send-otp/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                return BadRequest($"Exception occured during SendOtpForEmailVerification. Message: {ex.Message}. " +
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
        [Route("password/reset/send-otp/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                return BadRequest($"Exception occured during SendOtpToResetPassword. Message: {ex.Message}. " +
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
        [Route("login/send-otp/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                return BadRequest($"Exception occured during SendOtpForLogin. Message: {ex.Message}. " +
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
        [Route("otp/validate")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                return BadRequest($"Exception occured during ValidateOtp. Message: {ex.Message}. " +
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
        [Route("password/reset/validate-otp")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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
                    return Ok(resetPasswordToken);
                }

                return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occured during ValidateOtp. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #endregion
    }
}

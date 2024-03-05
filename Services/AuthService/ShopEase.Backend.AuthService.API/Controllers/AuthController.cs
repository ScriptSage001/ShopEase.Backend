using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Application.Queries;

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
        /// To Register a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Token Model - AccessToken, RefreshToken, TokenExpirationDate</returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
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
                return StatusCode(500, $"Exception occured during Registration. Message: {ex.Message}. " +
                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Login a user using Email and Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Token Model - AccessToken, RefreshToken, TokenExpirationDate</returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
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
                return StatusCode(500, $"Exception occured during Login. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Login a user using Email and OTP
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Token Model - AccessToken, RefreshToken, TokenExpirationDate</returns>
        [HttpPost]
        [Route("login/otp")]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
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
                return StatusCode(500, $"Exception occured during Login. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #region Password

        /// <summary>
        /// To Reset Password using old password
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
                return StatusCode(500, $"Exception occured during ResetPassword. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        /// <summary>
        /// To Reset Password using OTP - Forgot Password Scenario
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("password/reset/forgot")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPasswordUsingOtp([FromBody] ResetPasswordUsingOtpRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"ErrorCode: RequestEmpty, ErrorMessage: Request is empty. Please provide valid request.");
                }
                else
                {
                    var result = await _apiService.SendAsync(new ResetPasswordUsingOtpCommand(request));

                    if (result.IsSuccess)
                    {
                        return Ok("Password Changed Successfully!");
                    }

                    return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during ResetPassword. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #endregion
    }
}

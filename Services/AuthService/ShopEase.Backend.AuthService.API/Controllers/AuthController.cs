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
        /// To Generate Otp
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("otp/generate/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateOtp(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest($"ErrorCode: InvalidEmail, ErrorMessage: Please provide valid UserEmail.");
                }

                var result = await _apiService.SendAsync(new SendOtpCommand(email));

                if (result.IsSuccess)
                {
                    return Ok($"Otp sent successfully to the following email address: {email}");
                }

                return BadRequest($"ErrorCode: {result.Error?.Code}, ErrorMessage: {result.Error?.Message}");
            }
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #endregion
    }
}

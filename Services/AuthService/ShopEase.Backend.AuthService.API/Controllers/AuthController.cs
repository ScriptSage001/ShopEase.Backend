using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
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
                // Validate request - Fluent Validation

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
                // Validate request - Fluent Validation

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
                // Validate request - Fluent Validation
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
        [HttpPost]
        [Route("revoke/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeRefreshToken(string email)
        {
            try
            {
                // Validate request - Fluent Validation
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
        [HttpPost]
        [Route("revoke/{id:Guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeRefreshToken(Guid id)
        {
            try
            {
                // Validate request - Fluent Validation
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
    }
}

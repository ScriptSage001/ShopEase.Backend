using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Application.Queries;

namespace ShopEase.Backend.AuthService.API.Controllers
{
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
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            try
            {
                // Validate request - Fluent Validation

                var command = new RegisterUserCommand(request);
                await _apiService.SendAsync(command);

                if (command.Result.IsFailure)
                {
                    return BadRequest($"ErrorCode: {command.Result.Error?.Code}, ErrorMessage: {command.Result.Error?.Message}");
                }

                return Ok($"Bearer: {command.Result.Value}");
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
                    await _apiService.SendAsync(command);

                    if (command.Result.IsFailure)
                    {
                        return BadRequest($"ErrorCode: {command.Result.Error?.Code}, ErrorMessage: {command.Result.Error?.Message}");
                    }

                    return Ok($"Bearer: {command.Result.Value}");
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
    }
}

using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Commands;

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
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            try
            {
                // Validate request - Fluent Validation

                var command = new RegisterUserCommand(request);
                _apiService.SendAsync(command);

                if (!command.IsUserCreated)
                {
                    return Conflict($"ErrorCode: {command.Error?.Code}, ErrorMessage: {command.Error?.Message}");
                }

                return Ok("User Registered Successfully.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}

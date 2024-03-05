using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.API.Controllers
{
    /// <summary>
    /// Controller for Auth Services
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        #region Variables

        /// <summary>
        /// Instance of IApiService
        /// </summary>
        private readonly IApiService _apiService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for TokenController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="apiService"></param>
        public TokenController(IApiService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region Public EndPoints

        #region Refresh Token

        /// <summary>
        /// To Refresh Tokens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
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
                return StatusCode(500, $"Exception occured during Token Refresh. Message: {ex.Message}. " +
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
        [Route("refresh/revoke/{email}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
                return StatusCode(500, $"Exception occured during Revoke Refresh Token. Message: {ex.Message}. " +
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
        [Route("refresh/revoke/{id:Guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
                return StatusCode(500, $"Exception occured during Revoke Refresh Token. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #region Access Token

        /// <summary>
        /// To Generate Access and Refresh Tokens using Client Secret
        /// </summary>
        /// <param name="clientCredentials"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTokenByClientSecret([FromBody] ClientCredentials clientCredentials)
        {
            try
            {
                if(clientCredentials == null)
                {
                    return BadRequest($"ErrorCode: Request, ErrorMessage: Please provide valid request.");
                }

                var tokenResult = await _apiService.SendAsync(new GenerateTokenByClientSecretCommand(clientCredentials));

                if (tokenResult.IsSuccess)
                {
                    return Ok(tokenResult.Value);
                }

                return BadRequest($"ErrorCode: {tokenResult.Error?.Code}, ErrorMessage: {tokenResult.Error?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception occured during Token Generation. Message: {ex.Message}. " +
                                    $"Stack Trace: {ex.StackTrace}. Inner Exception: {ex.InnerException?.ToString()}");
                // Tech Debt - Add logging, reduce details from return statement.
            }
        }

        #endregion

        #endregion
    }
}

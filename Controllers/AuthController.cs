using LibrarianWorkplaceAPI.Core.Auth;
using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarianWorkplaceAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;
        public AuthController(ILogger<AuthController> logger, ITokenManager tokenManager)
        {
            _logger = logger;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// Получениу токена для доступа к API
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Authenticate([FromBody] UserCredential userCredential)
        {
            var token = _tokenManager.Authenticate(userCredential.UserName, userCredential.Password);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}

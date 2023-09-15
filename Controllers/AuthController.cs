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
        public AuthController(ILogger<BooksController> logger, ITokenManager tokenManager)
        {
            _logger = logger;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

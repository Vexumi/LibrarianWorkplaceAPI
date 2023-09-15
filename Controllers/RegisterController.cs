using LibrarianWorkplaceAPI.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibrarianWorkplaceAPI.Controllers;

namespace LibrarianWorkplaceAPI.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;
        public RegisterController(ILogger<BooksController> logger, ITokenManager tokenManager)
        {
            _logger = logger;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Registration(UserModel newUser)
        {
            if (_tokenManager.Register(newUser) is null) return BadRequest();
            return CreatedAtAction(nameof(AuthController.Authenticate), new UserCredential() { UserName = newUser.UserName, Password = newUser.Password });
        }
    }
}

using LibrarianWorkplaceAPI.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibrarianWorkplaceAPI.Controllers;
using LibrarianWorkplaceAPI.Models.GetModels;
using Microsoft.AspNetCore.Cors;

namespace LibrarianWorkplaceAPI.Controllers
{
    [EnableCors]
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;
        public RegisterController(ILogger<RegisterController> logger, ITokenManager tokenManager)
        {
            _logger = logger;
            _tokenManager = tokenManager;
        }


        /// <summary>
        /// Регистрация библиотеки в API
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Registration(RegisterUserModel user)
        {
            var newUser = new UserModel { UserName = user.UserName, Password = user.Password, Address = user.Address, LibraryName = user.LibraryName };

            if (_tokenManager.Register(newUser) is null) return BadRequest();
            return CreatedAtAction(nameof(AuthController.Authenticate), new UserCredential() { UserName = newUser.UserName, Password = newUser.Password });
        }
    }
}

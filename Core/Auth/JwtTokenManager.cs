using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibrarianWorkplaceAPI.Core.Auth
{
    public class JwtTokenManager: ITokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersDbUnit _usersContext;

        public JwtTokenManager(IConfiguration configuration, IUsersDbUnit usersContext) 
        {
            _configuration = configuration;
            _usersContext = usersContext;
        }

        public string? Authenticate(string username, string password)
        {

            //Check if username and password in DB, else return null
            var user = _usersContext.Users.Find(c => c.UserName == username && c.Password == password).FirstOrDefault();
            if (user == null) { return null; }

            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, user.LibraryName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

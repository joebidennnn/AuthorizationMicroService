using AuthorizationService.Models;
using AuthorizationService.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AuthenticationService));
        public AuthenticationService(IUserRepository useRepo, IConfiguration configuration)
        {
            _userRepo = useRepo;
            _configuration = configuration;
        }
        public string Authenticate(User credentials)
        {
            User user = new User();
            try
            {
                user = _userRepo.GetUser(credentials);
                string token = null;
                if (user != null)
                {
                    token = CreateJwtToken(user.UserName);
                }
                return token;
            }
            catch (Exception e)
            {
                _log.Error("Error in Provider while getting token - " + e.Message);
                throw;
            }
        }

        private string CreateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }
                ),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

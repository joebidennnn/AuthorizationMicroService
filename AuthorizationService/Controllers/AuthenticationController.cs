using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AuthenticationController));
        private readonly IAuthenticationService authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        // GET: api/<AuthenticationController>
        [HttpPost]
        public IActionResult AuthenticateUser(User user)
        {
            try
            {
                _log.Info("Credentials of user : '" + user.UserName + "' Received");
                string token = authenticationService.Authenticate(user);
                if (token != null)
                {
                    _log.Info("User : '" + user.UserName + "' is valid and token returned");
                    return Ok(new { token });
                }
                else
                {
                    _log.Info("Invalid username" + user.UserName);
                    return NotFound("invalid username/password");
                }
            }
            catch(Exception exception)
            {
                _log.Error("Error inside Controller while logging in. - " + exception.Message);
                return StatusCode(500);
            }
        }
    }
}

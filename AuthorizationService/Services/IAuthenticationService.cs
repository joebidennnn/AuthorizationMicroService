using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public interface IAuthenticationService
    {
        string Authenticate(User user);
    }
}

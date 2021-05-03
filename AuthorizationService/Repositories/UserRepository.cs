using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserRepository));
        private static readonly List<User> Users = new List<User>()
        {
            new User(){UserID = 1,UserName = "joe",Password = "biden"},
            new User(){UserID = 2,UserName = "connor",Password = "flight"},
            new User(){UserID = 3,UserName = "cypher",Password = "camera"}
        };
        public User GetUser(User user)
        {
            try
            {
                User userFound = Users.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                return userFound;
            }
            catch (Exception e)
            {
                _log.Error("Error in Repository while getting User - " + e.Message);
                throw;
            }
        }
    }
}

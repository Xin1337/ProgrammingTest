using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurrea.Models;

namespace Gurrea.Interfaces
{
    internal class AuthenticationService
    {
        private readonly User user = new User()
        {
            Username = "admin",
            Password = "password"
        };

        public bool Login(string username, string password)
        {
            return username == user.Username
                && password == user.Password;
        }
    }
}

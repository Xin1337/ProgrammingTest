using Gurrea.Models;

namespace Gurrea.Services
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

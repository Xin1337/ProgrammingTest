using Gurrea.Interfaces;
using Gurrea.Menus;
using Gurrea.Services;

namespace Gurrea
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AuthenticationService authenticationService = new AuthenticationService();
            LoginMenu loginMenu = new LoginMenu(authenticationService);

            if (!loginMenu.Run())
            {
                return;
            }

            PersonService personService = new PersonService();
            PersonMenu personMenu = new PersonMenu(personService);
            personMenu.Run();
        }
    }
}
using System;
using Gurrea.Interfaces;

namespace Gurrea.Menus
{
    internal class LoginMenu
    {
        private readonly AuthenticationService authenticationService;

        public LoginMenu(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public bool Run()
        {
            const int maximumAttempts = 3;

            for (int attempt = 1; attempt <= maximumAttempts; attempt++)
            {
                Console.Clear();
                Console.WriteLine("Login");
                Console.WriteLine("=====");

                Console.Write("Username: ");
                string username = Console.ReadLine() ?? string.Empty;

                Console.Write("Password: ");
                string password = ReadPassword();

                if (authenticationService.Login(username, password))
                {
                    return true;
                }

                Console.WriteLine($"Invalid credentials. Attempt {attempt} of {maximumAttempts}.");
                Pause();
            }

            Console.WriteLine("Too many failed login attempts.");
            return false;
        }

        private static string ReadPassword()
        {
            string password = string.Empty;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return password;
                }

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write('*');
                }
            }
        }

        private static void Pause()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}

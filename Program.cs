using Gurrea.Interfaces;
using Gurrea.Models;
using Gurrea.Services;

namespace Gurrea
{
    internal class Program
    {
        private static readonly PersonService personService = new PersonService();
        private static readonly AuthenticationService authenticationService = new AuthenticationService();
        private static void Main(string[] args)
        {
            if (!Login())
            {
                return;
            }
            RunMainMenu();
        }

        private static void RunMainMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Person Management System");
                Console.WriteLine("================================");
                Console.WriteLine("1. Add Person");
                Console.WriteLine("2. Display People");
                Console.WriteLine("3. Search Person by ID");
                Console.WriteLine("4. Update Person");
                Console.WriteLine("5. Delete Person");
                Console.WriteLine("6. Exit");

                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        AddPerson();
                        break;

                    case "2":
                        DisplayPeople();
                        break;

                    case "3":
                        SearchPersonById();
                        break;

                    case "4":
                        UpdatePerson();
                        break;

                    case "5":
                        DeletePerson();
                        break;

                    case "6":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
            Console.WriteLine("Application exited. Press any key to close.");
        }

        private static void AddPerson()
        {
            Console.Clear();
            Console.WriteLine("Add a new person");
            Console.WriteLine("================================");

            Console.Write("First Name: ");
            string firstName = Console.ReadLine() ?? string.Empty;

            Console.Write("Middle Name: ");
            string middleName = Console.ReadLine() ?? string.Empty;

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine() ?? string.Empty;

            DateOnly birthday = ReadBirthday();

            List<string> addresses = ReadAddresses();

            Person person = personService.AddPerson(
                firstName,
                middleName,
                lastName,
                birthday,
                addresses);

            Console.WriteLine();
            Console.WriteLine($"Person added successfully. ID: {person.Id}");

            Pause();
        }

        private static void DisplayPeople()
        {
            Console.Clear();

            Console.WriteLine("People");
            Console.WriteLine("================================");

            List<Person> people = personService.GetAllPeople();

            if (people.Count == 0)
            {
                Console.WriteLine("No people have been added.");
                Pause();
                return;
            }

            foreach (Person person in people)
            {
                int age = PersonService.CalculateAge(person.Birthday);

                Console.WriteLine($"ID: {person.Id}");
                Console.WriteLine($"Name: {person.FullName}");
                Console.WriteLine($"Birthday: {person.Birthday:yyyy-MM-dd}");
                Console.WriteLine($"Age: {age}");
                Console.WriteLine("Addresses");

                foreach (Address address in person.Addresses)
                {
                    Console.WriteLine($"- {address.Description}");
                }

                Console.WriteLine(new string('-', 40));
            }

            Pause();
        }

        private static void SearchPersonById()
        {
            Console.Clear();
            Console.WriteLine("Search Person by ID");

            Console.Write("Enter Person ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Person? person = personService.GetPersonById(id);
                if (person != null)
                {
                    int age = PersonService.CalculateAge(person.Birthday);
                    Console.WriteLine($"ID: {person.Id}");
                    Console.WriteLine($"Name: {person.FullName}");
                    Console.WriteLine($"Birthday: {person.Birthday:yyyy-MM-dd}");
                    Console.WriteLine($"Age: {age}");
                    Console.WriteLine("Addresses");
                    foreach (Address address in person.Addresses)
                    {
                        Console.WriteLine($"- {address.Description}");
                    }
                }
                else
                {
                    Console.WriteLine($"No person found with ID {id}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }

            Pause();
        }

        public static void UpdatePerson()
        {
            Console.Clear();

            Console.WriteLine("Update Person");
            Console.WriteLine("==============");

            Console.Write("Enter Person ID to update: ");
            string idInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Invalid ID format.");
                Pause();
                return;
            }

            Person? person = personService.GetPersonById(id);

            if (person == null)
            {
                Console.WriteLine($"No person found with ID {id}.");
                Pause();
                return;
            }

            string firstName = ReadOptionalText(
                $"First Name ({person.FirstName}): ",
                person.FirstName);

            string middleName = ReadOptionalText(
                $"Middle Name ({person.MiddleName}): ",
                person.MiddleName);

            string lastName = ReadOptionalText(
                $"Last Name ({person.LastName}): ",
                person.LastName);

            DateOnly birthday = ReadOptionalBirthday(person.Birthday);

            List<string> addresses = ReadOptionalAddresses(person.Addresses);

            bool updated = personService.UpdatePerson(
                id,
                firstName,
                middleName,
                lastName,
                birthday,
                addresses);

            Console.WriteLine(
                updated
                    ? "Person updated successfully."
                    : "Failed to update person.");

            Pause();
        }

        public static void DeletePerson()
        {
            Console.Clear();
            Console.WriteLine("Delete Person");
            Console.WriteLine("================================");
            Console.Write("Enter Person ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool deleted = personService.DeletePerson(id);
                if (deleted)
                {
                    Console.WriteLine("Person deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"No person found with ID {id}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
            Pause();
        }

        private static DateOnly ReadBirthday()
        {
            while (true)
            {
                Console.Write("Birthday (yyyy-MM-dd): ");
                string input = Console.ReadLine() ?? string.Empty;

                if (DateOnly.TryParse(input, out DateOnly birthday)
                    && birthday <= DateOnly.FromDateTime(DateTime.Today))
                {
                    return birthday;
                }

                Console.WriteLine("Please enter a valid birthday.");
            }
        }

        private static List<string> ReadAddresses()
        {
            List<string> addresses = new List<string>();

            while (true)
            {
                Console.Write("Address: ");
                string address = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(address))
                {
                    addresses.Add(address);
                }

                Console.Write("Add another address? (Y/N): ");
                string answer = Console.ReadLine() ?? string.Empty;

                if (!answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }

            return addresses;
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static string ReadOptionalText(string prompt, string defaultValue)
        {
            Console.Write(prompt);

            string? input = Console.ReadLine();

            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }

        private static DateOnly ReadOptionalBirthday(DateOnly defaultValue)
        {
            while (true)
            {
                Console.Write("Birthday (yyyy-MM-dd) or press enter to keep current: ");

                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return defaultValue;
                }

                if (DateOnly.TryParse(input, out DateOnly birthday)
                    && birthday <= DateOnly.FromDateTime(DateTime.Today))
                {
                    return birthday;
                }

                Console.WriteLine("Please enter a valid birthday or press enter to keep current.");
            }
        }

        private static List<string> ReadOptionalAddresses(
            List<Address> currentAddresses)
        {
            Console.WriteLine("Current addresses:");

            foreach (Address address in currentAddresses)
            {
                Console.WriteLine($"- {address.Description}");
            }

            Console.Write("Enter a new address or press Enter to keep current: ");
            string firstAddress = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(firstAddress))
            {
                List<string> existingAddresses = new List<string>();

                foreach (Address address in currentAddresses)
                {
                    existingAddresses.Add(address.Description);
                }

                return existingAddresses;
            }

            List<string> addresses = new List<string>
            {
                firstAddress
            };

            while (true)
            {
                Console.Write("Add another address? (Y/N): ");
                string answer = Console.ReadLine() ?? string.Empty;

                if (!answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.Write("Address: ");
                string addressInput = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(addressInput))
                {
                    addresses.Add(addressInput);
                }
            }

            return addresses;
        }

        private static bool Login()
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.Clear();
                Console.WriteLine("Login");
                Console.WriteLine("================================");
                Console.Write("Username: ");
                string username = Console.ReadLine() ?? string.Empty;
                Console.Write("Password: ");
                string password = ReadPassword();
                if (authenticationService.Login(username, password))
                {
                    return true;
                }
                Console.WriteLine($"Invalid credentials. Attempt {attempt} of {maxAttempts}.");
                Pause();
            }
            Console.WriteLine("Too many failed login attempts. Exiting application.");
            return false;
        }

        private static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
    }
}
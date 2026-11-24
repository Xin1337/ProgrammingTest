using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurrea.Models;
using Gurrea.Services;

namespace Gurrea.Menus
{
    internal class PersonMenu
    {
        private readonly PersonService personService;

        public PersonMenu(PersonService personService)
        {
            this.personService = personService;
        }

        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Person Management System");
                Console.WriteLine("=======================================");
                Console.WriteLine("1. Add Person");
                Console.WriteLine("2. Display People");
                Console.WriteLine("3. Search Person by ID");
                Console.WriteLine("4. Update Person");
                Console.WriteLine("5. Delete Person");
                Console.WriteLine("6. Generate Report");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option: ");

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
                        GenerateReport();
                        break;
                    case "7":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        private void AddPerson()
        {
            Console.Clear();
            Console.WriteLine("Add a new person");
            Console.WriteLine("================");

            Console.Write("First Name: ");
            string firstName = Console.ReadLine() ?? string.Empty;
            Console.Write("Middle Name: ");
            string middleName = Console.ReadLine() ?? string.Empty;
            Console.Write("Last Name: ");
            string lastName = Console.ReadLine() ?? string.Empty;

            DateOnly birthday = ReadBirthday();
            List<string> addresses = ReadAddresses();

            Person person = personService.AddPerson(
                firstName, middleName, lastName, birthday, addresses);

            Console.WriteLine($"Person added successfully. ID: {person.Id}");
            Pause();
        }

        private void DisplayPeople()
        {
            Console.Clear();
            Console.WriteLine("People");
            Console.WriteLine("======");

            List<Person> people = personService.GetAllPeople();

            if (people.Count == 0)
            {
                Console.WriteLine("No people have been added.");
                Pause();
                return;
            }

            foreach (Person person in people)
            {
                DisplayPerson(person);
                Console.WriteLine(new string('-', 40));
            }

            Pause();
        }

        private void SearchPersonById()
        {
            Console.Clear();
            Console.WriteLine("Search Person by ID");
            Console.Write("Enter Person ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                Pause();
                return;
            }

            Person? person = personService.GetPersonById(id);

            if (person == null)
            {
                Console.WriteLine($"No person found with ID {id}.");
            }
            else
            {
                DisplayPerson(person);
            }

            Pause();
        }

        private void UpdatePerson()
        {
            Console.Clear();
            Console.WriteLine("Update Person");
            Console.Write("Enter Person ID to update: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
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

            string firstName = ReadOptionalText("First Name", person.FirstName);
            string middleName = ReadOptionalText("Middle Name", person.MiddleName);
            string lastName = ReadOptionalText("Last Name", person.LastName);
            DateOnly birthday = ReadOptionalBirthday(person.Birthday);
            List<string> addresses = ReadOptionalAddresses(person.Addresses);

            bool updated = personService.UpdatePerson(
                id, firstName, middleName, lastName, birthday, addresses);

            Console.WriteLine(updated
                ? "Person updated successfully."
                : "Failed to update person.");
            Pause();
        }

        private void DeletePerson()
        {
            Console.Clear();
            Console.WriteLine("Delete Person");
            Console.Write("Enter Person ID to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
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

            Console.Write($"Delete {person.FullName}? (Y/N): ");
            string confirmation = Console.ReadLine() ?? string.Empty;

            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase)
                && personService.DeletePerson(id))
            {
                Console.WriteLine("Person deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }

            Pause();
        }

        private void GenerateReport()
        {
            Console.Clear();
            Console.WriteLine("Person Report");
            Console.WriteLine("=============");
            Console.WriteLine("ID\tName\t\tBirthday\t\tAge\tAddresses");

            List<Person> people = personService.GetAllPeople();

            foreach (Person person in people)
            {
                Console.WriteLine(
                    $"{person.Id}\t{person.FullName}\t{person.Birthday:yyyy-MM-dd}\t{PersonService.CalculateAge(person.Birthday)}\t{string.Join(", ", person.Addresses.Select(a => a.Description))}");
            }

            Pause();
        }

        private static void DisplayPerson(Person person)
        {
            Console.WriteLine($"ID: {person.Id}");
            Console.WriteLine($"Name: {person.FullName}");
            Console.WriteLine($"Birthday: {person.Birthday:yyyy-MM-dd}");
            Console.WriteLine($"Age: {PersonService.CalculateAge(person.Birthday)}");
            Console.WriteLine("Addresses:");

            foreach (Address address in person.Addresses)
            {
                Console.WriteLine($"- {address.Description}");
            }
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

        private static string ReadOptionalText(string label, string currentValue)
        {
            Console.Write($"{label} ({currentValue}): ");
            string input = Console.ReadLine() ?? string.Empty;
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        private static DateOnly ReadOptionalBirthday(DateOnly currentBirthday)
        {
            while (true)
            {
                Console.Write($"Birthday ({currentBirthday:yyyy-MM-dd}): ");
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return currentBirthday;
                }

                if (DateOnly.TryParse(input, out DateOnly birthday)
                    && birthday <= DateOnly.FromDateTime(DateTime.Today))
                {
                    return birthday;
                }

                Console.WriteLine("Please enter a valid birthday.");
            }
        }

        private static List<string> ReadOptionalAddresses(List<Address> currentAddresses)
        {
            Console.WriteLine("Press Enter immediately to retain current addresses.");
            Console.Write("New address: ");
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

            List<string> addresses = new List<string> { firstAddress };

            while (true)
            {
                Console.Write("Add another address? (Y/N): ");
                string answer = Console.ReadLine() ?? string.Empty;

                if (!answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.Write("Address: ");
                string address = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(address))
                {
                    addresses.Add(address);
                }
            }

            return addresses;
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}

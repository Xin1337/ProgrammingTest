using Gurrea.Models;

namespace Gurrea.Services
{
    internal class PersonService
    {
        private readonly List<Person> people = new List<Person>();
        private int nextPersonId = 1;
        private int nextAddressId = 1;

        public Person AddPerson(
            string firstName,
            string middleName,
            string lastName,
            DateOnly birthday,
            List<string> addressDescriptions)
        {
            Person person = new Person
            {
                Id = nextPersonId++,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Birthday = birthday
            };

            foreach (string addressDescription in addressDescriptions)
            {
                person.Addresses.Add(new Address
                {
                    Id = nextAddressId++,
                    Description = addressDescription
                });
            }
            people.Add(person);
            return person;
        }

        public List<Person> GetAllPeople()
        {
            return people;
        }

        public Person? GetPersonById(int id)
        {
            return people.FirstOrDefault(person => person.Id == id);
        }

        public bool UpdatePerson(
            int id,
            string firstName,
            string middleName,
            string lastName,
            DateOnly birthday,
            List<string> addressDescriptions)
        {
            Person? person = GetPersonById(id);

            if (person == null)
            {
                  return false;
            }

            person.FirstName = firstName;
            person.MiddleName = middleName;
            person.LastName = lastName;
            person.Birthday = birthday;

            person.Addresses.Clear();
            foreach (string addressDescription in addressDescriptions)
            {
                person.Addresses.Add(new Address
                {
                    Id = nextAddressId++,
                    Description = addressDescription
                });
            }
            return true;
        }

        public bool DeletePerson (int id)
        {
            Person? person = GetPersonById(id);

            if (person == null)
            {
                return false;
            }

            return people.Remove(person);
        }

        public bool AddAddress(int personId, string description)
        {
            Person? person = GetPersonById(personId);

            if (person == null || string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            person.Addresses.Add(new Address
            {
                Id = nextAddressId++,
                Description = description
            });

            return true;
        }

        public bool DeleteAddress(int personId, int addressId)
        {
            Person? person = GetPersonById(personId);

            if (person == null)
            {
                return false;
            }

            Address? address = person.Addresses
                .FirstOrDefault(item => item.Id == addressId);

            return address != null && person.Addresses.Remove(address);
        }

        public static int CalculateAge(DateOnly birthday)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today); // Get today's date as a DateOnly object
            int age = today.Year - birthday.Year; // Calculate the initial age based on the year difference
            if (birthday > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}

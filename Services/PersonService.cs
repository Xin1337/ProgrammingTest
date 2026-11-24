using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static int CalculateAge(DateOnly birthday)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}

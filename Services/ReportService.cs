using System.Text;
using Gurrea.Models;

namespace Gurrea.Services
{
    internal class ReportService
    {
        public string GenerateReport(List<Person> people)
        {
            if (people.Count == 0)
            {
                return "No people have been added yet.";
            }

            StringBuilder report = new StringBuilder();

            int totalAddresses = 0;
            int totalAge = 0;
            Person youngestPerson = people[0];
            Person oldestPerson = people[0];

            int children = 0;
            int adults = 0;
            int seniors = 0;

            foreach (Person person in people)
            {
                int age = PersonService.CalculateAge(person.Birthday);
                
                totalAddresses += person.Addresses.Count;
                totalAge += age;

                if (person.Birthday > youngestPerson.Birthday)
                {
                    youngestPerson = person;
                }

                if (person.Birthday < oldestPerson.Birthday)
                {
                    oldestPerson = person;
                }

                if (age < 18)
                {
                    children++;
                }
                else if (age < 65)
                {
                    adults++;
                }
                else
                {
                    seniors++;
                }
            }

            double averageAge = (double)totalAge / people.Count;

            report.AppendLine("Report Summary:");
            report.AppendLine("================================");
            report.AppendLine($"Total number of people: {people.Count}");
            report.AppendLine($"Total number of addresses: {totalAddresses}");
            report.AppendLine($"Average age: {averageAge:F2}");
            report.AppendLine();
            report.AppendLine($"Youngest person: {youngestPerson.FullName} (Age: {PersonService.CalculateAge(youngestPerson.Birthday)})");
            report.AppendLine($"Oldest person: {oldestPerson.FullName} (Age: {PersonService.CalculateAge(oldestPerson.Birthday)})");
            report.AppendLine();
            report.AppendLine("Age Distribution:");
            report.AppendLine($"Under 18: {children}");
            report.AppendLine($"18-64: {adults}");
            report.AppendLine($"65 and over: {seniors}");
            report.AppendLine();
            report.AppendLine("People with multiple addresses:");

            bool foundMultipleAddresses = false;

            foreach (Person person in people)
            {
                if (person.Addresses.Count > 1)
                {
                    report.AppendLine($"- {person.FullName}");
                    foundMultipleAddresses = true;
                }
            }

            if (!foundMultipleAddresses)
            {
                report.AppendLine("None");
            }

            report.AppendLine();
            report.AppendLine("All people");

            foreach (Person person in people)
            {
                int age = PersonService.CalculateAge(person.Birthday);

                report.AppendLine(
                    $"- {person.FullName}, Age: {age}, " +
                    $"Addresses: {person.Addresses.Count}");
            }

            return report.ToString();
        }
    }
}

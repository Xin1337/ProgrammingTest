using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurrea.Models
{
    internal class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        public List<Address> Addresses {get; set; } = new List<Address>();
        public string FullName
        {
            get
            {
                return string.Join(
                " ",
                new[] { FirstName, MiddleName, LastName }.Where(name => !string.IsNullOrWhiteSpace(name))); // Concatenate names with spaces, ignoring empty or whitespace names

            }
        }
    }
}

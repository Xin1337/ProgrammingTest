using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurrea.Models
{
    internal class Address
    {
        public int Id { get; set; } 
        public string Description { get; set; } = string.Empty; // Street address, e.g., "123 Main St"
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFnetVisitations.Entities
{
    internal class Student
    {
        public Guid Id { get; init; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Birthday { get; set; }

        public override string ToString()
        {
            return FirstName+LastName;
        }
    }
}

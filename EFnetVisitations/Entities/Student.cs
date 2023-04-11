using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFnetVisitations.Entities
{
    [Index(nameof(FirstName),nameof(LastName))]
    class Student
    {
        public Guid Id { get; init; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Birthday { get; set; }
        public Passport? Passport { get; set; } = null;
        public List<Visit>? Visits { get; set; }
        public Guid GroupId { get; set; }
        public Group? Group { get; set; }
        public override string ToString()
        {
            return FirstName+LastName;
        }
    }
}

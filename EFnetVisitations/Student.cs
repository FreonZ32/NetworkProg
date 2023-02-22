using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFnetVisitations
{
    internal class Student
    {
        public long Id { get; init; }
        public string? Name { get; set; }
        public DateTime Birthday { get; set; }
        //public List<DateOnly>Visits {get; set; } = new List<DateOnly>();
    }
}

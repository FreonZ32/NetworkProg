using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFnetVisitations.Entities
{
    class Group
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Student>? Students { get; set; }
        //public Nullable<int> StudentsCount => Students.Count;
        public override string ToString()
        {
            return Name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperNetVisitaions
{
    class Student
    {
        public Student() { }
        public Student(string id, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Birthday= birthday;

        }
        public Guid id { get; set; }
        public string? FirstName { get; set; } = "";
        public string? LastName { get; set; } = "";
        public DateTime? Birthday { get; set; }

        public string Id 
        {
            get { return id.ToString(); }
            set { id = new Guid(value); }
        }

        public override string ToString()
        {
            return FirstName+ " " + LastName;
        }
    }
}

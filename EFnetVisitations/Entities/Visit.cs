using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFnetVisitations.Entities
{
    class Visit
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}

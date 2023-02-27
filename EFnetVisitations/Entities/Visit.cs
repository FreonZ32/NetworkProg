using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFnetVisitations.Entities
{
    internal class Visit
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Student? Student { get; set; }
    }
}

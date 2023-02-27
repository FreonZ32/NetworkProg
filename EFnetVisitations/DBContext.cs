using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EFnetVisitations.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFnetVisitations
{
    internal class DBContext : DbContext
    {
        private const string ConnectionString = "Data Source=I:\\VisualStudio\\NetworkProg\\EFnetVisitations\\bin\\Debug\\net7.0-windows\\hello.db";

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Visit> Visits=> Set<Visit>();
    }
}

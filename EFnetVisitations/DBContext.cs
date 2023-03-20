using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly StreamWriter logStream = new StreamWriter("DbLogs.txt", true);
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString).LogTo(logStream.WriteLine);
        }

        public override void Dispose()
        {
            base.Dispose();
            logStream.Dispose();
        }
        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await logStream.DisposeAsync();

        }
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Visit> Visits=> Set<Visit>();
        public DbSet<Subject> Subjects=> Set<Subject>();
        public DbSet<Group> Groups=> Set<Group>();
    }
}

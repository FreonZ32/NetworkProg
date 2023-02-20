using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace AddNetVisitation
{
    internal class StudentsVisitationService
    {
        public Visitation[] GetVisitations()
        {
            using var connection = sqliteConnection();
            connection.Open();
            var sql = "SELECT * FROM Visitations";
            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();
            var result = new List<Visitation>();
            foreach (IDataRecord row in reader)
            {
                var visit = new Visitation
                (
                    Id: row.GetInt64(row.GetOrdinal("Id")),
                    Name: row.GetString(row.GetOrdinal("Name")),
                    Date: DateOnly.Parse(row.GetString(row.GetOrdinal("Date")))
                );
                result.Add(visit);
            }
            return result.ToArray();
        }
        public void CreateTable()
        {
            using var connection = sqliteConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE Visitations(" +
                "Id INTEGER PRIMARY KEY NOT NULL," +
                "Name TEXT NOT NULL," +
                "Date DATE NOT NULL" +
                ")";
            command.ExecuteNonQuery();
        }
        public void FillVisitations(string name, DateOnly date)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO Visitations(Name, Date)" +
                $" VALUES ('{name}','{date}')";
            command.ExecuteNonQuery();
        }
        public bool FindSameName(string Name)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var sql = $"SELECT Name FROM Visitations WHERE Name LIKE '{Name}'";
            using var command = new SqliteCommand(sql, connection);
            var result = command.ExecuteReader();
            if (result.HasRows)
            { return true; }
            else { return false; }
        }
        public SqliteConnection sqliteConnection()
        {
            var connectionString = "Data Source=myappdb.db";
            using var connection = new SqliteConnection(connectionString);
            return connection;
        }
    }
}

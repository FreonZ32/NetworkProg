using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Formats.Tar;

namespace AddNetVisitation
{

    
    internal class StudentsVisitationService
    {
        string NameOfMainTable = "VisitationsNameTB";
       
        public void ShowAllVisitations()
        {
            List<VisitationNames> visitationNames = new List<VisitationNames>();
            using var connection = sqliteConnection();
            connection.Open();
            var sql = "SELECT Id,Name FROM " + NameOfMainTable;
            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();
            foreach (IDataRecord row in reader)
            {
                var visit = new VisitationNames
                (
                    Id: row.GetInt64(row.GetOrdinal("Id")),
                    Name: row.GetString(row.GetOrdinal("Name"))
                );
                Console.Write("\n"+ visit.Id.ToString()+")"+visit.Name.ToString()+":");
                var visitDate = ShowVisitationByName(visit.Name);
                foreach(VisitationPTB visPVT in visitDate)
                {
                    Console.Write(visPVT.Date.ToString()+" ");
                }
            }
        }
        public List<VisitationPTB> ShowVisitationByName(string name)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var sql = $"SELECT Id,Date FROM {name}PT;";
            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();
            var result = new List<VisitationPTB>();
            foreach (IDataRecord row in reader)
            {
                var visit = new VisitationPTB
                (
                    Id: row.GetInt64(row.GetOrdinal("Id")),
                    Date: row.GetString(row.GetOrdinal("Date"))
                );
                result.Add(visit);
            }
            return result;
        }

        public void CreateTable()
        {
            using var connection = sqliteConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"CREATE TABLE {NameOfMainTable} (Id INTEGER PRIMARY KEY NOT NULL, Name TEXT NOT NULL)";
            command.ExecuteNonQuery();
        }
        public void CreatePersonalTable(string name, DateOnly date)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"CREATE TABLE {name}PT (Id INTEGER PRIMARY KEY NOT NULL, Date TEXT NOT NULL)";
            command.ExecuteNonQuery();
            command.CommandText = $"INSERT INTO {NameOfMainTable} (Name) VALUES ('{name}')";
            command.ExecuteNonQuery();
            command.CommandText = $"INSERT INTO {name}PT (Date) VALUES ('{date}')";
            command.ExecuteNonQuery();
        }
        public void FillVisitationByName(string name, DateOnly date)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {name}PT (Date) VALUES ('{date}')";
            command.ExecuteNonQuery();
        }
        public void DeletePersonalTable(string name)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {NameOfMainTable} WHERE Name LIKE '{name}'";
            command.ExecuteNonQuery();
            command.CommandText = $"DROP TABLE {name}PT;";
            command.ExecuteNonQuery();
        }
        public bool FindSameName(string Name)
        {
            using var connection = sqliteConnection();
            connection.Open();
            var sql = $"SELECT Name FROM {NameOfMainTable} WHERE Name LIKE '{Name}'";
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

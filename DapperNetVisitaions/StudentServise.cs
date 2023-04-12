using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DapperNetVisitaions
{
    class StudentServise:IDisposable
    {
        private readonly SqliteConnection _connection;
        public StudentServise() 
        {
            _connection = new("Data Source=Students.db");
        }
        public void Dispose() => _connection.Dispose();
        public async Task AddTable(string tableName)
        {
            await _connection.OpenAsync();
            var sql = $"CREATE TABLE {tableName} (Id GUID PRIMARY KEY NOT NULL, FirstName TEXT,LastName TEXT, Birthday DATE);";
            _connection.ExecuteAsync(sql);
        }
        public async Task<IReadOnlyList<Student>> GetStudents()
        {
            var sql = "select * from Students";
            var students = await _connection.QueryAsync<Student>(sql);
            return students.ToList();
        }
        public void AddStudent(Student student)
        {
            var sql = $"INSERT INTO Students (Id, FirstName, LastName, Birthday) VALUES (@Id, @FirstName, @LastName, @Birthday)";
            _connection.Execute(sql,student);
        }
        public async Task<Student> GetStudentById(Guid id)
        {
            var sql = @"select * from Students where Id=@id";
            Student? student = await _connection.QuerySingleAsync<Student>(
                sql, new { id });
            return student;
        }
        public async Task<IReadOnlyList<Student>> GetStudentByFirstLastName(string name)
        {
            Student student = new Student();
            var sql = $@"select * from Students where FirstName LIKE @n OR LastName LIKE @n";
            var students = await _connection.QueryAsync<Student>(sql, new { n = "%" + name + "%" });
            return students.ToList();
        }
        public void Update(Student student)
        {
            var sql = @"UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Birthday = @Birthday WHERE Id = @Id";
            _connection.Execute(sql, student);
        }
        public void Delete(Student student)
        {
            var sql = @"DELETE FROM Students WHERE Id = @Id";
            _connection.Execute(sql,student);
        }
    }
}

using System;
using System.Data.SQLite;
using System.Collections.Generic;
using HolmesglenStudentManager.Models;

namespace HolmesglenStudentManager.DataAccess
{
    public class StudentDALConnectedMode
    {
        private readonly string _connectionString;

        public StudentDALConnectedMode(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Student", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                        });
                    }
                }
            }
            return students;
        }

        public Student GetStudentById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Student WHERE StudentId = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            StudentId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                        };
                    }
                }
            }
            return null;
        }

        public bool AddStudent(Student student)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Student (StudentId, FirstName, LastName, Email) VALUES (@StudentId, @FirstName, @LastName, @Email)", connection);
                command.Parameters.AddWithValue("@StudentId", student.StudentId);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateStudent(Student student)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE StudentId = @StudentId", connection);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@StudentId", student.StudentId);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteStudent(int studentId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Student WHERE StudentId = @StudentId", connection);
                command.Parameters.AddWithValue("@StudentId", studentId);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}

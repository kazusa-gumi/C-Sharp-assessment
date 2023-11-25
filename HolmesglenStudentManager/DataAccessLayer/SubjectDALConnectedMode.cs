using System;
using System.Data.SQLite;
using HolmesglenStudentManager.Models;

namespace HolmesglenStudentManager.DataAccess
{
    public class SubjectDALConnectedMode
    {
        private readonly string _connectionString;

        public SubjectDALConnectedMode(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Subject", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjects.Add(new Subject
                        {
                            SubjectId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            NumberOfSession = reader.GetInt32(2),
                            HourPerSession = reader.GetInt32(3),
                        });
                    }
                }
            }
            return subjects;
        }

        public Subject GetSubjectById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Subject WHERE SubjectId = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Subject
                        {
                            SubjectId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            NumberOfSession = reader.GetInt32(2),
                            HourPerSession = reader.GetInt32(3),
                        };
                    }
                }
            }
            return null;
        }

        public bool AddSubject(Subject subject)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Subject (SubjectId, Title, NumberOfSession, HourPerSession) VALUES (@SubjectId, @Title, @NumberOfSession, @HourPerSession)", connection);
                command.Parameters.AddWithValue("@SubjectId", subject.SubjectId);
                command.Parameters.AddWithValue("@Title", subject.Title);
                command.Parameters.AddWithValue("@NumberOfSession", subject.NumberOfSession);
                command.Parameters.AddWithValue("@HourPerSession", subject.HourPerSession);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateSubject(Subject subject)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Student SET Title = @Title, NumberOfSession = @NumberOfSession, HourPerSession = @HourPerSession WHERE SubjectId = @SubjectId", connection);
                command.Parameters.AddWithValue("@Title", subject.Title);
                command.Parameters.AddWithValue("@NumberOfSession", subject.NumberOfSession);
                command.Parameters.AddWithValue("@HourPerSession", subject.HourPerSession);
                command.Parameters.AddWithValue("@SubjectId", subject.SubjectId);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteSubject(int subjectId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Subject WHERE SubjectId = @SubjectId", connection);
                command.Parameters.AddWithValue("@SubjectId", subjectId);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}

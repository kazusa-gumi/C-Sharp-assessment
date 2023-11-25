using System;
using System.Data.SQLite;
using System.Collections.Generic;
using HolmesglenStudentManager.Models;

namespace HolmesglenStudentManager.DataAccess
{
    public class EnrollmentDALConnectedMode
    {
        private readonly string _connectionString;

        public EnrollmentDALConnectedMode(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Enrollment> GetAllEnrollments()
        {
            var enrollments = new List<Enrollment>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Enrollment", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(new Enrollment
                        {
                            Id = reader.GetInt32(0),
                            StudentID_FK = reader.GetInt32(1),
                            SubjectID_FK = reader.GetInt32(2),
                        });
                    }
                }
            }
            return enrollments;
        }

        public Enrollment GetEnrollmentById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Enrollment WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Enrollment
                        {
                            Id = reader.GetInt32(0),
                            StudentID_FK = reader.GetInt32(1),
                            SubjectID_FK = reader.GetInt32(2),
                        };
                    }
                }
            }
            return null;
        }

        public Subject GetSubjectById(int subjectId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Subject WHERE SubjectId = @subjectId", connection);
                command.Parameters.AddWithValue("@subjectId", subjectId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Subject
                        {
                            SubjectId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                        };
                    }
                }
            }
            return null;
        }

        public bool AddEnrollment(Enrollment enrollment)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Enrollment (StudentID_FK, SubjectID_FK) VALUES (@StudentID, @SubjectID)", connection);
                command.Parameters.AddWithValue("@StudentID", enrollment.StudentID_FK);
                command.Parameters.AddWithValue("@SubjectID", enrollment.SubjectID_FK);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateEnrollment(Enrollment enrollment)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Enrollment SET StudentID_FK = @StudentID, SubjectID_FK = @SubjectID WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@StudentID", enrollment.StudentID_FK);
                command.Parameters.AddWithValue("@SubjectID", enrollment.SubjectID_FK);
                command.Parameters.AddWithValue("@Id", enrollment.Id);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteEnrollment(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM Enrollment WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<EnrollmentDetail> GetAllEnrollmentDetails()
        {
            var enrollmentDetails = new List<EnrollmentDetail>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(@"
            SELECT e.Id, s.StudentId, s.FirstName || ' ' || s.LastName AS StudentName, su.SubjectId, su.Title
            FROM Enrollment e
            JOIN Student s ON e.StudentID_FK = s.StudentId
            JOIN Subject su ON e.SubjectID_FK = su.SubjectId
            ", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollmentDetails.Add(new EnrollmentDetail
                        {
                            EnrollmentId = reader.GetInt32(0),
                            StudentId = reader.GetInt32(1),
                            StudentName = reader.GetString(2),
                            SubjectId = reader.GetInt32(3),
                            SubjectTitle = reader.GetString(4),
                        });
                    }
                }
            }
            return enrollmentDetails;
        }

    }
}

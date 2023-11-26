// DisconnectedStudentDAL.cs
using System.Data;
using System.Data.SQLite;

namespace HolmesglenStudentManager.DataAccess
{
    public class DisconnectedStudentDAL
    {
        private string _connectionString;

        public DisconnectedStudentDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataSet GetAllStudents()
        {
            var dataSet = new DataSet();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var sql = "SELECT * FROM Student";
                var adapter = new SQLiteDataAdapter(sql, connection);
                adapter.Fill(dataSet, "Student");
            }
            return dataSet;
        }

        public void InsertStudent(DataSet dataSet, DataRow newStudent)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var adapter = new SQLiteDataAdapter("SELECT * FROM Student", connection);
                var commandBuilder = new SQLiteCommandBuilder(adapter);

                // Here we need to set the adapter's InsertCommand, but since
                // we're within the DAL, we have access to _connectionString
                adapter.InsertCommand = commandBuilder.GetInsertCommand();

                // Add the new student DataRow to the "Students" DataTable
                dataSet.Tables["Student"].Rows.Add(newStudent);

                // Perform the update on the database
                adapter.Update(dataSet, "Student");
            }
        }

        public void UpdateStudent(DataSet dataSet, DataRow student)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var adapter = new SQLiteDataAdapter("SELECT * FROM Student", connection);
                var commandBuilder = new SQLiteCommandBuilder(adapter);

                // Here we need to set the adapter's UpdateCommand
                adapter.UpdateCommand = commandBuilder.GetUpdateCommand();

                // Perform the update on the database
                adapter.Update(dataSet, "Student");
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = "DELETE FROM Student WHERE StudentId = @id";
                    cmd.Parameters.AddWithValue("@id", studentId);
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                connection.Close();
            }
        }

    }
}
// SQLiteDataAccess.cs
using System;
using System.Data.SQLite;

namespace HolmesglenStudentManager.DataAccess
{
    public class SQLiteDataAccess
    {
        private string connectionString = "Data Source=/Users/naoikazusa/Desktop/Database/Holmesglen.studentData.db";

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public void TestConnection()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}

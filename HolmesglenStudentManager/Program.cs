using CsvHelper;
using System.Globalization;
using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;
using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.PresentationLayer;


namespace HolmesglenStudentManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display the title of the system on the console
            Console.WriteLine("Holmesglen Student Management System");

            // EntityFrameWork
            //var db = new AppDBContext();
            // Connected Mode
            var sqliteDataAccess = new SQLiteDataAccess();
            sqliteDataAccess.TestConnection();

            // ここでは SQLiteDataAccess から接続文字列を取得するか、直接指定します
            var connectionString = sqliteDataAccess.ConnectionString;
            var studentDalConnected = new StudentDALConnectedMode(connectionString);
            var studentBLLConnected = new StudentBLLConnectedMode(studentDalConnected);
            var studentUIConnectedMode = new StudentUIConnectedMode(studentBLLConnected);


            var subjectDalConnected = new SubjectDALConnectedMode(connectionString);
            var subjectBLLConnected = new SubjectBLLConnectedMode(subjectDalConnected);
            var subjectUIConnectedMode = new SubjectUIConnectedMode(subjectBLLConnected);

            var enrollmentDalConnected = new EnrollmentDALConnectedMode(connectionString);
            var enrollmentBLLConnected = new EnrollmentBLLConnectedMode(enrollmentDalConnected);
            var enrollmentUIConnectedMode = new EnrollmentUIConnectedMode(enrollmentBLLConnected);

            //var subjectDal = new SubjectDAL(db);
            //var subjectBLL = new SubjectBLL(subjectDal);
            //var studentDal = new StudentDAL(db);
            //var studentBLL = new StudentBLL(studentDal);
            //var subjectUI = new SubjectUI(subjectBLL);
            //var enrollmentDal = new EnrollmentDAL(db);
            //var enrollmentBLL = new EnrollmentBLL(enrollmentDal, studentBLL, subjectBLL);
            //var enrollmentUI = new EnrollmentUI(enrollmentBLL, studentBLL, subjectBLL);

            // connectionMode
            //var sqliteDataAccess = odenew SQLiteDataAccess();
            //sqliteDataAccess.TestConnection();

            // disconnection mode session15

            // Main application loop flag
            bool running = true;
            while (running)
            {
                // Display main operations menu
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1) Student operations");
                Console.WriteLine("2) Subject operations");
                Console.WriteLine("3) Enrollment operations");
                Console.WriteLine("4) Import Data");
                Console.WriteLine("5) ExportData");
                Console.WriteLine("6) Exit");
                // Prompt the user to select an option
                Console.Write("Please enter an option: ");

                // Read user input
                var input = Console.ReadLine();
                // Act based on the selected option
                switch (input)
                {
                    case "1":
                        // Display the student operations submenu
                        studentUIConnectedMode.DisplayStudentMenu();
                        break;
                    case "2":
                        // Display the subject operations submenu
                        subjectUIConnectedMode.DisplaySubjectMenu();
                        break;
                    case "3":
                        // Display the enrollment operations submenu
                        enrollmentUIConnectedMode.DisplayEnrollmentMenu();
                        break;
                    case "4":
                        // Option to import students from CSV
                        ImportStudentsFromCsv(studentBLLConnected);
                        break;
                    case "5":
                        // Option to export students to CSV
                        ExportStudentsToCsv(studentBLLConnected);
                        break;
                    case "6":
                        // Exit the application loop
                        running = false;
                        break;
                    default:
                        // Handle invalid input and prompt user to try again
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }

            // Display a thank you message when the application is about to close
            Console.WriteLine("Thank you for using Holmesglen Student Management System.");
        }

        // Method to import student data from CSV
        static void ImportStudentsFromCsv(StudentBLLConnectedMode studentBLL)
        {
            // Ask the user to enter the full path to the CSV file
            Console.Write("Enter the full path to the CSV file to import: ");
            string csvFilePath = Console.ReadLine();

            // Check if the specified file exists
            if (!File.Exists(csvFilePath))
            {
                // If the file does not exist, issue a warning and exit the method
                Console.WriteLine("File does not exist.");
                return;
            }
            try
            {
                // Open file reader and pass to CSV reader
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Read records from a CSV file
                    var records = csv.GetRecords<Student>();
                    // Process for each record
                    foreach (var record in records)
                    {
                        // Add imported student data
                        studentBLL.AddStudent(record);
                    }
                }
                // Inform user that all student data has been successfully imported
                Console.WriteLine("Students imported successfully.");
            }
            catch (Exception ex)
            {
                // Display errors, if any, in the console
                Console.WriteLine("An error occurred while importing students: " + ex.Message);
            }
        }

        // Method to export student data to CSV
        static void ExportStudentsToCsv(StudentBLLConnectedMode studentBLL)
        {
            // Ask the user to enter the full path to the CSV file to be exported.
            Console.Write("Enter the full path for the CSV file to export: ");
            string csvFilePath = Console.ReadLine();

            try
            {
                // Open a file writer at the given path and pass it to the CSV writer
                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Get all student data through StudentBLL
                    var students = studentBLL.GetAllStudents();
                    // Write acquired data to CSV file
                    csv.WriteRecords(students);
                }
                // Tell the user that all student data has been successfully exported
                Console.WriteLine("Students exported successfully.");
            }
            catch (Exception ex)
            {
                // If an error occurs during the export process, display it in the console
                Console.WriteLine("An error occurred while exporting students: " + ex.Message);
            }
        }
    }
}
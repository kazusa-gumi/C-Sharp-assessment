using CsvHelper;
using System.Globalization;
using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;
using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.PresentationLayer;
using HolmesglenStudentManager.BusinessLogic;

namespace HolmesglenStudentManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display the title of the system on the console
            Console.WriteLine("Holmesglen Student Management System");


            // Connected Mode
            var sqliteDataAccess = new SQLiteDataAccess();
            sqliteDataAccess.TestConnection();

            var connectionString = sqliteDataAccess.ConnectionString;
            var studentDalConnected = new StudentDALConnectedMode(connectionString);
            var studentBLLConnected = new StudentBLLConnectedMode(studentDalConnected);
            var studentUIConnectedMode = new StudentUIConnectedMode(studentBLLConnected);


            var subjectDalConnected = new SubjectDALConnectedMode(connectionString);
            var subjectBLLConnected = new SubjectBLLConnectedMode(subjectDalConnected);
            var subjectUIConnectedMode = new SubjectUIConnectedMode(subjectBLLConnected);

            var enrollmentDalConnected = new EnrollmentDALConnectedMode(connectionString);
            var enrollmentBLLConnected = new EnrollmentBLLConnectedMode(enrollmentDalConnected, studentDalConnected);
            var enrollmentUIConnectedMode = new EnrollmentUIConnectedMode(enrollmentBLLConnected, subjectBLLConnected, studentBLLConnected);

            // EntityFrameWork Mode
            //var db = new AppDBContext();
            // EntityFrameWork Mode
            //var subjectDal = new SubjectDAL(db);
            //var subjectBLL = new SubjectBLL(subjectDal);
            //var subjectUI = new SubjectUI(subjectBLL);

            // Disconnected Mode
            //var connectionString = "Data Source=/Users/naoikazusa/Desktop/Database/Holmesglen.studentData.db";
            //// Disconnected Mode
            //var studentDAL = new DisconnectedStudentDAL(connectionString); 
            //var studentBLL = new DisconnectedStudentBLL(studentDAL);
            //var studentUI = new DisconnectedStudentUI(studentBLL);


            // Main application loop flag
            bool running = true;
            while (running)
            {
                // Display main operations menu
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1) Student operations");
                Console.WriteLine("2) Subject operations");
                Console.WriteLine("3) Enrollment operations");
                Console.WriteLine("4) Exit");
                // Prompt the user to select an option
                Console.Write("Please enter an option: ");

                // Read user input
                var input = Console.ReadLine();
                // Act based on the selected option
                switch (input)
                {
                    case "1":
                        // Display the student operations submenu
                        // Connected mode
                        studentUIConnectedMode.DisplayStudentMenu();
                        // DisConnected mode
                        //studentUIConnectedMode.DisplayStudentMenu();
                        //studentUI.DisplayStudentMenu();
                        break;
                    case "2":
                        // Display the subject operations submenu
                        subjectUIConnectedMode.DisplaySubjectMenu();
                        // EntityFrameWork Mode
                        //subjectUI.DisplaySubjectMenu();
                        break;
                    case "3":
                        // Display the enrollment operations submenu
                        enrollmentUIConnectedMode.DisplayEnrollmentMenu();
                        break;
                    case "4":
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
    }
}
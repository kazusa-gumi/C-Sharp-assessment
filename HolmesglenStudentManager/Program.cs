using CsvHelper;
using System.Globalization;
using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;
using static System.Collections.Specialized.BitVector32;
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

            // Initialize the database context
            var db = new AppDBContext();
            var studentDal = new StudentDAL(db);
            // SQLiteDataAccess
            // TestSQLiteConnection();
            var studentBLL = new StudentBLL(studentDal);
            var studentUI = new StudentUI(studentBLL);
            var subjectDal = new SubjectDAL(db);
            var subjectBLL = new SubjectBLL(subjectDal);
            var subjectUI = new SubjectUI(subjectBLL);
            var enrollmentBLL = new EnrollmentBLL(db);

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
                        studentUI.DisplayStudentMenu();
                        break;
                    case "2":
                        // Display the subject operations submenu
                        subjectUI.DisplaySubjectMenu();
                        break;
                    case "3":
                        // Display the enrollment operations submenu
                        DisplayEnrollmentMenu(enrollmentBLL, studentBLL, subjectBLL);
                        break;
                    case "4":
                        // Option to import students from CSV
                        ImportStudentsFromCsv(studentBLL);
                        break;
                    case "5":
                        // Option to export students to CSV
                        ExportStudentsToCsv(studentBLL);
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
        static void DisplayEnrollmentMenu(EnrollmentBLL enrollmentBLL, StudentBLL studentBLL, SubjectBLL subjectBLL)
        {
            bool inEnrollmentMenu = true;
            while (inEnrollmentMenu) // Loop to keep the enrollment menu active
            {
                // Print the enrollment operations options to the console
                Console.WriteLine("\nEnrollment Operations:");
                Console.WriteLine("1) Create Enroll Student");
                Console.WriteLine("2) List All Enrollments");
                Console.WriteLine("3) Update Enrollment");
                Console.WriteLine("4) Cancel Enrollment");
                Console.WriteLine("5) Generate Enrollment Report");
                Console.WriteLine("6) Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": // Option to enroll a student in a subject
                        Console.Write("Enter student ID (number): ");
                        if (!int.TryParse(Console.ReadLine(), out int studentId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                            return;  // 整数以外の入力をした場合は早期にメソッドから抜けます
                        }

                        Console.Write("Enter subject ID (number): ");
                        if (!int.TryParse(Console.ReadLine(), out int subjectId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number for subject ID.");
                            return;  // 整数以外の入力をした場合は早期にメソッドから抜けます
                        }

                        // studentIdとsubjectIdをint型でオブジェクトにセット
                        var newEnrollment = new Enrollment
                        {
                            StudentID_FK = studentId,
                            SubjectID_FK = subjectId
                        };

                        // Try adding the new enrollment using EnrollmentBLL and inform user of the result
                        if (enrollmentBLL.AddEnrollment(newEnrollment))
                        {
                            Console.WriteLine("*********************");
                            Console.WriteLine("Successfully added to Enrollment.");
                            Console.WriteLine("It also successfully sent an email to STUDENT with the above text.");

                        }
                        else
                        {
                            Console.WriteLine("Failed to add enrollment. The enrollment might already exist or the input is invalid.");
                        }
                        break;
                    case "2": // Option to list all existing enrollments
                        var allEnrollments = enrollmentBLL.GetAllEnrollments(); // Get all enrollments
                                                                                // Loop through each enrollment and print its details
                        foreach (var enrollment in allEnrollments)
                        {
                            Console.WriteLine($"Enrollment ID: {enrollment.Id}, " +
                                              $"Student ID: {enrollment.StudentID_FK}, " +
                                              $"Subject ID: {enrollment.SubjectID_FK}");
                        }
                        break;
                    case "3": // Option to update an existing enrollment
                        Console.Write("Enter the enrollment ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int enrollmentId))
                        {
                            var enrollmentToUpdate = enrollmentBLL.GetEnrollmentById(enrollmentId);
                            if (enrollmentToUpdate != null)
                            {
                                
                                Console.WriteLine($"Current Student ID: {enrollmentToUpdate.StudentID_FK}, Current Subject ID: {enrollmentToUpdate.SubjectID_FK}");

                                Console.Write("Enter new student ID (leave blank for no change): ");
                                var newStudentIdInput = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newStudentIdInput))
                                {
                                    if (int.TryParse(newStudentIdInput, out int newStudentId)) // 新しい学生IDをint型としてパース
                                    {
                                        var student = studentBLL.GetStudentById(newStudentId);
                                        if (student != null)
                                        {
                                            enrollmentToUpdate.StudentID_FK = newStudentId; // 更新をint型の新しいIDで行う
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid student ID. No student found with that ID.");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid student ID. Please enter a numeric value.");
                                        break;
                                    }
                                }

                                Console.Write("Enter new subject ID (leave blank for no change): ");

                                var newSubjectIdInput = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newSubjectIdInput))
                                {
                                    if (int.TryParse(newSubjectIdInput, out int newSubjectId)) // 新しい科目IDをint型としてパース
                                    {
                                        var subject = subjectBLL.GetSubjectById(newSubjectId);
                                        if (subject != null)
                                        {
                                            enrollmentToUpdate.SubjectID_FK = newSubjectId; // 更新をint型の新しいIDで行う
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid subject ID. No subject found with that ID.");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid subject ID. Please enter a numeric value.");
                                        break;
                                    }
                                }
                                // 同様に
                                if (enrollmentBLL.UpdateEnrollment(enrollmentToUpdate, studentBLL, subjectBLL))
                                {
                                    Console.WriteLine("Enrollment updated successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to update enrollment.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Enrollment not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid enrollment ID.");
                        }
                        break;
                    case "4": // Option to cancel or delete an enrollment
                        Console.Write("Enter enrollment ID to cancel: "); // Prompt for enrollment ID
                                                                          // Read and parse the enrollment ID; if input is invalid, display an error message
                                                                          // If the ID entered by the user is a valid integer, parse it and store the value in the 'enrollmentId' variable
                        if (int.TryParse(Console.ReadLine(), out enrollmentId))
                        {
                            // Attempt to delete the enrollment with the specified ID using the EnrollmentBLL
                            if (enrollmentBLL.DeleteEnrollment(enrollmentId))
                            {
                                // If the deletion is successful, print a confirmation message to the console
                                Console.WriteLine("Enrollment cancelled successfully.");
                            }
                            else
                            {
                                // If the deletion fails (enrollment does not exist or another issue), print an error message to the console
                                Console.WriteLine("Failed to cancel enrollment or enrollment not found.");
                            }
                        }
                        else
                        {
                            // If the entered ID is not a valid integer, print an error message to the console
                            Console.WriteLine("Invalid enrollment ID.");
                        }
                        break; // Exit the switch-case block

                    case "5": // 新しいオプションとしてレポート生成機能を追加
                        enrollmentBLL.GenerateEnrollmentReport();
                        break;

                    case "6": // メインメニューに戻る
                        inEnrollmentMenu = false;
                        break;

                    default:
                        // If an unrecognized option is selected, inform the user and prompt to try again
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        // Method to import student data from CSV
        static void ImportStudentsFromCsv(StudentBLL studentBLL)
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
        static void ExportStudentsToCsv(StudentBLL studentBLL)
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
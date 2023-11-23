using CsvHelper;
using System.Globalization;
using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;
using static System.Collections.Specialized.BitVector32;
using HolmesglenStudentManager.DataAccess;

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
            // SQLiteDataAccess
            // TestSQLiteConnection();
            // Create instances of the Business Logic Layer for students, subjects, and enrollments
            var studentBLL = new StudentBLL(db);
            var subjectBLL = new SubjectBLL(db);
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
                        DisplayStudentMenu(studentBLL);
                        break;
                    case "2":
                        // Display the subject operations submenu
                        DisplaySubjectMenu(subjectBLL);
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

        //static void TestSQLiteConnection()
        //{
        //    var sqliteDataAccess = new SQLiteDataAccess();
        //    sqliteDataAccess.TestConnection();
        //}
        // Method to display the sutudent operations menu
        static void DisplayStudentMenu(StudentBLL studentBLL)
        {
            // Flag to keep the student menu loop running
            bool inStudentMenu = true;
            while (inStudentMenu)
            {
                // Display the student operations sub-menu
                Console.WriteLine("\nStudent Operations:");
                Console.WriteLine("1) Create Student");
                Console.WriteLine("2) List All Students");
                Console.WriteLine("3) Update Student");
                Console.WriteLine("4) Delete Student");
                Console.WriteLine("5) Back to Main Menu");
                Console.Write("Select an option: ");
                // Read the user's choice from the console
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": // Option to create a new student
                              // Prompt the user for student details and read the input
                        Console.Write("Enter student ID: ");
                        string studentId = Console.ReadLine();
                        Console.Write("Enter first name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Enter last name: ");
                        string lastName = Console.ReadLine();
                        Console.Write("Enter email: ");
                        string email = Console.ReadLine();
                        // Create a new student object
                        var newStudent = new Student
                        {
                            StudentId = studentId,
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email
                        };
                        // Add the new student to the database and display the result
                        if (studentBLL.AddStudent(newStudent))
                        {
                            Console.WriteLine("Student added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add student. ID might already exist or input is invalid.");
                        }
                        break;
                    case "2": // Option to list all students
                              // Retrieve all students from the database and display their information
                        var allStudents = studentBLL.GetAllStudents();
                        foreach (var student in allStudents)
                        {
                            Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}, Email: {student.Email}");
                        }
                        break;
                    case "3": // Option to update an existing student's information
                              // Prompt the user for the ID of the student to update
                        Console.Write("Enter student ID to update: ");
                        string updateId = Console.ReadLine();
                        // Retrieve the student from the database
                        var studentToUpdate = studentBLL.GetStudentById(updateId);
                        if (studentToUpdate != null)
                        {
                            // Read the new values for the student's details, allowing the user to leave fields blank to keep them unchanged
                            Console.Write("Enter new first name (leave blank to keep current): ");
                            studentToUpdate.FirstName = Console.ReadLine();
                            Console.Write("Enter new last name (leave blank to keep current): ");
                            studentToUpdate.LastName = Console.ReadLine();
                            Console.Write("Enter new email (leave blank to keep current): ");
                            studentToUpdate.Email = Console.ReadLine();
                            // Update the student in the database and display the result
                            if (studentBLL.UpdateStudent(studentToUpdate))
                            {
                                Console.WriteLine("Student updated successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Failed to update student.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Student not found."); // Inform the user if the requested student does not exist
                        }
                        break;
                    case "4": // Option to delete a student
                              // Prompt the user for the ID of the student to delete
                        Console.Write("Enter student ID to delete: ");
                        string deleteId = Console.ReadLine();
                        // Delete the student from the database and display the result
                        if (studentBLL.DeleteStudent(deleteId))
                        {
                            Console.WriteLine("Student deleted successfully!"); // Confirmation message for successful deletion
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete student or student not found."); // Error message for failed deletion or when the student does not exist
                        }
                        break;
                    case "5":
                        // Exit the student menu and return to the main menu
                        inStudentMenu = false;
                        break;
                    default:
                        // Message displayed when an unknown option is selected
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
        // Method to display the subject operations menu
        static void DisplaySubjectMenu(SubjectBLL subjectBLL)
        {
            // Flag to control the display of the subject menu
            bool inSubjectMenu = true;
            while (inSubjectMenu) // Loop to keep the subject menu active
            {
                // Display the available subject operations to the user
                Console.WriteLine("\nSubject Operations:");
                Console.WriteLine("1) Create Subject");
                Console.WriteLine("2) List All Subjects");
                Console.WriteLine("3) Update Subject");
                Console.WriteLine("4) Delete Subject");
                Console.WriteLine("5) Back to Main Menu");
                Console.Write("Select an option: ");
                // Read the user's menu selection from the console
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": // Option to create a new subject
                              // Prompt for and read subject details from the user
                        Console.Write("Enter subject ID: ");
                        string subjectId = Console.ReadLine();
                        Console.Write("Enter subject title: ");
                        string title = Console.ReadLine();
                        // Validation for the number of sessions
                        Console.Write("Enter number of sessions: ");
                        int numberOfSession;
                        while (!int.TryParse(Console.ReadLine(), out numberOfSession))
                        {
                            Console.Write("Invalid input - please enter a valid number of sessions: ");
                        }
                        // Validation for the hours per session
                        Console.Write("Enter hours per session: ");
                        int hourPerSession;
                        while (!int.TryParse(Console.ReadLine(), out hourPerSession))
                        {
                            Console.Write("Invalid input - please enter a valid number of hours per session: ");
                        }
                        // Create and populate a new Subject object with the provided information
                        var newSubject = new Subject
                        {
                            SubjectId = subjectId,
                            Title = title,
                            NumberOfSession = numberOfSession, // Added property
                            HourPerSession = hourPerSession // Added property
                        };
                        // Attempt to add the new subject to the database using the SubjectBLL
                        if (subjectBLL.AddSubject(newSubject))
                        {
                            Console.WriteLine("Subject added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add subject. ID might already exist or input is invalid.");
                        }
                        break;
                    case "2": // Option to list all subjects
                              // Retrieve all subjects from the database
                        var allSubjects = subjectBLL.GetAllSubjects();
                        // Display each subject's detail
                        foreach (var subject in allSubjects)
                        {
                            Console.WriteLine($"ID: {subject.SubjectId}, Title: {subject.Title}, " +
                            $"Sessions: {subject.NumberOfSession}, Hours/Session: {subject.HourPerSession}");
                        }
                        break;
                    case "3": // Option to update a subject's information
                              // Prompt for and read the subject ID to update from the user
                        Console.Write("Enter subject ID to update: ");
                        string updateId = Console.ReadLine();
                        // Attempt to retrieve the subject to update based on the provided ID
                        var subjectToUpdate = subjectBLL.GetSubjectById(updateId);
                        if (subjectToUpdate != null)
                        {
                            // Prompt the user for a new title and read the input. Allow retention of the current title if the input is blank.
                            Console.Write("Enter new title (leave blank to keep current): ");
                            var newTitle = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newTitle))
                            {
                                subjectToUpdate.Title = newTitle; // Assign the new title if input is provided
                            }

                            // Prompt the user for a new number of sessions. Only apply the change if the input is a valid integer.
                            Console.Write("Enter new number of sessions (leave blank to keep current): ");
                            var newNumberOfSessionsStr = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newNumberOfSessionsStr))
                            {
                                if (int.TryParse(newNumberOfSessionsStr, out var newNumberOfSession))
                                {
                                    subjectToUpdate.NumberOfSession = newNumberOfSession; // Assign the new number of sessions if input is valid
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input for number of sessions."); // Notify the user of invalid input
                                    break; // Exit the switch-case block
                                }
                            }

                            // Prompt the user for new hours per session. Similarly, only apply the change if the input is valid.
                            Console.Write("Enter new hours per session (leave blank to keep current): ");
                            var newHoursPerSessionStr = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newHoursPerSessionStr))
                            {
                                if (int.TryParse(newHoursPerSessionStr, out var newHourPerSession))
                                {
                                    subjectToUpdate.HourPerSession = newHourPerSession; // Assign the new hours per session if input is valid
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input for hours per session."); // Notify the user of invalid input
                                    break; // Exit the switch-case block
                                }
                            }

                            // Attempt to update the subject using the Business Logic Layer, and notify the user about the outcome
                            if (subjectBLL.UpdateSubject(subjectToUpdate))
                            {
                                Console.WriteLine("Subject updated successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Failed to update subject."); // This indicates an error during the update process
                            }
                        }
                        else
                        {
                            Console.WriteLine("Subject not found."); // Notify the user if the subject with the supplied ID was not found
                        }
                        break; // End the case block

                        
                        case "4": // Option to delete a subject
                                  // Prompt the user to enter the ID of the subject they wish to delete
                                    Console.Write("Enter subject ID to delete: ");
                                    string deleteId = Console.ReadLine();

                                    // Call the DeleteSubject method in the subjectBLL to attempt to delete the subject with the provided ID
                                    if (subjectBLL.DeleteSubject(deleteId))
                                    {
                                        // If the deletion is successful, inform the user
                                        Console.WriteLine("Subject deleted successfully!");
                                    }
                                    else
                                    {
                                        // If the deletion fails because the subject couldn't be found or another reason, inform the user
                                        Console.WriteLine("Failed to delete subject or subject not found.");
                                    }
                                    break;

                                case "5": // Option to return to the main menu
                                          // Set inSubjectMenu to false to exit the submenu and go back to the main menu
                                    inSubjectMenu = false;
                                    break;

                                default:
                                    // If the user enters an option that is not recognized, prompt them to try entering a valid option again
                                    Console.WriteLine("Invalid option, please try again.");
                                    break;
                                }
            }

        }
        // Method to display the enrollment operations menu
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
                        Console.Write("Enter student ID: "); // Prompt for student ID
                        string studentId = Console.ReadLine(); // Read student ID
                        Console.Write("Enter subject ID: "); // Prompt for subject ID
                        string subjectId = Console.ReadLine(); // Read subject ID
                                                               // Create a new Enrollment object with provided student and subject IDs
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
                                var newStudentId = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newStudentId))
                                {
                                    var student = studentBLL.GetStudentById(newStudentId);
                                    if (student != null)
                                    {
                                        enrollmentToUpdate.StudentID_FK = newStudentId;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid student ID.");
                                        break;
                                    }
                                }

                                Console.Write("Enter new subject ID (leave blank for no change): ");
                                var newSubjectId = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newSubjectId))
                                {
                                    var subject = subjectBLL.GetSubjectById(newSubjectId);
                                    if (subject != null)
                                    {
                                        enrollmentToUpdate.SubjectID_FK = newSubjectId;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid subject ID.");
                                        break;
                                    }
                                }
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
            Console.Write("Enter the full path for the CSV file to export: ");
            string csvFilePath = Console.ReadLine();

            try
            {
                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var students = studentBLL.GetAllStudents();
                    csv.WriteRecords(students);
                }
                Console.WriteLine("Students exported successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while exporting students: " + ex.Message);
            }
        }
    }
}
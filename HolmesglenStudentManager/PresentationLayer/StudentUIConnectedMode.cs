using HolmesglenStudentManager;
using HolmesglenStudentManager.Models;
using HolmesglenStudentManager.BusinessLogicLayer;

using System;
namespace HolmesglenStudentManager.PresentationLayer {
    public class StudentUIConnectedMode {
        private readonly StudentBLLConnectedMode _studentBLL;

        public StudentUIConnectedMode(StudentBLLConnectedMode studentBLL) {
            _studentBLL = studentBLL;
        }

        public void DisplayStudentMenu() {
            bool inStudentMenu = true;
            while (inStudentMenu) {
                Console.WriteLine("\nStudent Operations:");
                Console.WriteLine("1) Create Student");
                Console.WriteLine("2) List All Students");
                Console.WriteLine("3) Update Student");
                Console.WriteLine("4) Delete Student");
                Console.WriteLine("5) Import CSV");
                Console.WriteLine("6) Export CSV");
                Console.WriteLine("7) Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                switch (choice) {
                    case "1":
                        CreateStudent();
                        break;
                    case "2":
                        ListStudents();
                        break;
                    case "3":
                        UpdateStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        ImportStudentsFromCsv();
                        break;
                    case "6":
                        ExportStudentsToCsv();
                        break;
                    case "7":
                        inStudentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
        private void CreateStudent() {
            Console.Write("Enter student ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int studentId)) {
                Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                return;
            }
            
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            var newStudent = new Student
            {
                    StudentId = studentId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
            };
            bool isCreated = _studentBLL.AddStudent(newStudent);
            if (isCreated) {
                Console.WriteLine("Student added successfully!");
            }
            else {
                Console.WriteLine("Failed to add student. ID might already exist or input is invalid.");
            }
        }
        private void ListStudents() {
            var students = _studentBLL.GetAllStudents();
            foreach(var student in students)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}, Email: {student.Email}");
            }
        }

        private void UpdateStudent() {
            Console.Write("Enter student ID to update (number): ");
            if (!int.TryParse(Console.ReadLine(), out int id)) {
                Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                return;
            }

            var studentToUpdate = _studentBLL.GetStudentById(id);
            if (studentToUpdate == null) {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.Write("Enter first name (leave blank to keep current): ");
            var firstName = Console.ReadLine();
            Console.Write("Enter last name (leave blank to keep current): ");
            var lastName = Console.ReadLine();
            Console.Write("Enter email (leave blank to keep current): ");
            var email = Console.ReadLine();

            // Only update the fields if new values have been provided
            studentToUpdate.FirstName = string.IsNullOrEmpty(firstName) ? studentToUpdate.FirstName : firstName;
            studentToUpdate.LastName = string.IsNullOrEmpty(lastName) ? studentToUpdate.LastName : lastName;
            studentToUpdate.Email = string.IsNullOrEmpty(email) ? studentToUpdate.Email : email;

            // StudentBLLを通じて学生情報を更新し、結果をユーザーに報告
            bool updated = _studentBLL.UpdateStudent(studentToUpdate);
            if (updated) {
                Console.WriteLine("Student updated successfully.");
            }
            else {
                Console.WriteLine("Failed to update student.");
            }
        }

        private void DeleteStudent() {
            Console.Write("Enter student ID to delete (number): ");
            if (!int.TryParse(Console.ReadLine(), out int id)) {
                Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                return;
            }

            bool deleted = _studentBLL.DeleteStudent(id);
            if (deleted) {
                Console.WriteLine("Student deleted successfully.");
            }
            else {
                Console.WriteLine("Failed to delete student or student not found.");
            }
        }

        public void ImportStudentsFromCsv()
        {
            Console.Write("Enter the full path to the CSV file to import: ");
            string csvFilePath = Console.ReadLine();

            bool success = _studentBLL.ImportStudentsFromCsv(csvFilePath);
            if (success)
            {
                Console.WriteLine("Students imported successfully.");
            }
            else
            {
                Console.WriteLine("An error occurred while importing students.");
            }
        }

        private void ExportStudentsToCsv()
        {
            Console.Write("Enter the full path for the CSV file to export: ");
            string csvFilePath = Console.ReadLine();

            bool success = _studentBLL.ExportStudentsToCsv(csvFilePath);
            if (success)
            {
                Console.WriteLine("Students exported successfully.");
            }
            else
            {
                Console.WriteLine("An error occurred while exporting students.");
            }
        }
    }
}


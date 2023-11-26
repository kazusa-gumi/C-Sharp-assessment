// DisconnectedStudentUI.cs
using HolmesglenStudentManager.BusinessLogic;
using System;
using System.Data;
using System.Linq;
namespace HolmesglenStudentManager.PresentationLayer
{
    public class DisconnectedStudentUI
    {
        private readonly DisconnectedStudentBLL _studentBLL;
        public DisconnectedStudentUI(DisconnectedStudentBLL studentBLL)
        {
            _studentBLL = studentBLL;
        }
        public void DisplayStudentMenu()
        {
            bool inStudentMenu = true;
            while (inStudentMenu)
            {
                Console.WriteLine("\nStudent Operations:");
                Console.WriteLine("1) Create Student");
                Console.WriteLine("2) List All Students");
                Console.WriteLine("3) Update Student");
                Console.WriteLine("4) Delete Student");
                Console.WriteLine("5) Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
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
                        inStudentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
        private void CreateStudent()
        {
            Console.Write("Enter student ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                return;
            }
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            var studentsDataSet = _studentBLL.GetAllStudents();
            var newStudentRow = studentsDataSet.Tables["Student"].NewRow();
            newStudentRow["StudentId"] = studentId;
            newStudentRow["FirstName"] = firstName;
            newStudentRow["LastName"] = lastName;
            newStudentRow["Email"] = email;
            _studentBLL.AddStudent(studentsDataSet, newStudentRow);
            Console.WriteLine("New student added successfully!");
        }

        private void ListStudents()
        {
            var studentsDataSet = _studentBLL.GetAllStudents();
            foreach (DataRow row in studentsDataSet.Tables["Student"].Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.WriteLine($"ID: {row["StudentId"]}, Name: {row["FirstName"]} {row["LastName"]}, Email: {row["Email"]}");
                }
            }
        }

        private void UpdateStudent()
        {
            Console.Write("Enter student ID to update (number): ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                return;
            }

            var studentsDataSet = _studentBLL.GetAllStudents();
            var studentRow = studentsDataSet.Tables["Student"].AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("StudentId") == studentId);

            if (studentRow != null)
            {
                Console.Write("Enter first name (leave blank to keep current): ");
                var firstName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    studentRow["FirstName"] = firstName;
                }

                Console.Write("Enter last name (leave blank to keep current): ");
                var lastName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    studentRow["LastName"] = lastName;
                }

                Console.Write("Enter email (leave blank to keep current): ");
                var email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    studentRow["Email"] = email;
                }

                _studentBLL.UpdateStudent(studentsDataSet, studentRow);
                Console.WriteLine("Student updated successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        private void DeleteStudent()
        {
            Console.Write("Enter student ID to delete (number): ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                return;
            }
            var studentsDataSet = _studentBLL.GetAllStudents();
            // Call the DeleteStudent method with the studentId only
            _studentBLL.DeleteStudent(studentId);
            Console.WriteLine("Student deleted successfully.");
        }
    }
}
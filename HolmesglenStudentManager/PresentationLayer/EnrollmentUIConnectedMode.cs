using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;

using System;

namespace HolmesglenStudentManager.PresentationLayer
{
    public class EnrollmentUIConnectedMode
    {
        private readonly EnrollmentBLLConnectedMode _enrollmentBLL;
        private readonly SubjectBLLConnectedMode _subjectBLL;
        private readonly StudentBLLConnectedMode _studentBLL;

        public EnrollmentUIConnectedMode(EnrollmentBLLConnectedMode enrollmentBLL, SubjectBLLConnectedMode subjectBLL, StudentBLLConnectedMode studentBLL)
        {
            _enrollmentBLL = enrollmentBLL;
            _subjectBLL = subjectBLL;
            _studentBLL = studentBLL;
        }
        public void DisplayEnrollmentMenu()
        {
            bool inEnrollmentMenu = true;
            while (inEnrollmentMenu)
            {
                Console.WriteLine("\nEnrollment Operations:");
                Console.WriteLine("1) Add Enrollment");
                Console.WriteLine("2) List All Enrollments");
                Console.WriteLine("3) Update Enrollment");
                Console.WriteLine("4) Delete Enrollment");
                Console.WriteLine("5) Generate a report for all enrollment information");
                Console.WriteLine("6) Generate an email to notify a student about their enrollment.");
                Console.WriteLine("7) Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddEnrollment();
                        break;
                    case "2":
                        ListEnrollments();
                        break;
                    case "3":
                        UpdateEnrollment();
                        break;
                    case "4":
                        DeleteEnrollment();
                        break;
                    case "5":
                        DisplayAllEnrollmentDetails();
                        break;
                    case "6":
                        GenerateEnrollmentEmails();
                        break;
                    case "7":
                        inEnrollmentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        public bool ValidateSubjectId(int subjectId)
        {
            var subject = _subjectBLL.GetSubjectById(subjectId);
            return subject != null;
        }

        public void AddEnrollment()
        {
            Console.WriteLine("Add New Enrollment");

            Console.Write("Enter Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Student ID.");
                return;
            }

            var successfullyAddedSubjects = new List<string>();

            bool addingMoreSubjects = true;
            while (addingMoreSubjects)
            {
                Console.Write("Enter Subject ID (or 'done' to finish): ");
                var input = Console.ReadLine();

                if (input.ToLower() == "done")
                {
                    addingMoreSubjects = false;
                    continue;
                }

                if (!int.TryParse(input, out int newSubjectId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for Subject ID, or 'done' to finish.");
                    continue;
                }

                var subject = _subjectBLL.GetSubjectById(newSubjectId);
                if (subject == null)
                {
                    Console.WriteLine($"Subject ID {newSubjectId} does not exist.");
                    continue;
                }

                var newEnrollment = new Enrollment
                {
                    StudentID_FK = studentId,
                    SubjectID_FK = newSubjectId
                };

                bool isCreated = _enrollmentBLL.AddEnrollment(newEnrollment);
                if (isCreated)
                {
                    successfullyAddedSubjects.Add($"{subject.Title} (ID: {subject.SubjectId})");
                    Console.WriteLine($"Subject ID {newSubjectId} added successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to add subject ID {newSubjectId}. It might already be enrolled or another error occurred.");
                }
            }

            // Check if any subjects have been successfully added for the student
            if (successfullyAddedSubjects.Any())
            {
                Console.WriteLine("A new enrollment has been successfully registered.");
            }
        }

        private void ListEnrollments()
        {
            var enrollments = _enrollmentBLL.GetAllEnrollments();
            foreach (var enrollment in enrollments)
            {
                Console.WriteLine($"ID: {enrollment.Id}, Student ID: {enrollment.StudentID_FK}, Subject ID: {enrollment.SubjectID_FK}");
            }
        }

        private void UpdateEnrollment()
        {
            Console.WriteLine("Update Enrollment Information");

            Console.Write("Enter Enrollment ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int enrollmentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Enrollment ID.");
                return;
            }

            var existingEnrollment = _enrollmentBLL.GetEnrollmentById(enrollmentId);
            if (existingEnrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }
            Console.Write("Enter new Student ID (leave blank to keep current): ");
            var studentIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(studentIdInput) && int.TryParse(studentIdInput, out int newStudentId))
            {
                existingEnrollment.StudentID_FK = newStudentId;
            }

            Console.Write("Enter new Subject ID (leave blank to keep current): ");
            var subjectIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(subjectIdInput) && int.TryParse(subjectIdInput, out int newSubjectId))
            {
                var subject = _subjectBLL.GetSubjectById(newSubjectId);
                if (subject == null)
                {
                    Console.WriteLine($"Subject ID {newSubjectId} does not exist.");
                    return;
                }
                existingEnrollment.SubjectID_FK = newSubjectId;
            }

            bool updated = _enrollmentBLL.UpdateEnrollment(existingEnrollment);
            if (updated)
            {
                Console.WriteLine("Enrollment updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update enrollment.");
            }
        }

        private void DeleteEnrollment()
        {
            Console.Write("Enter Enrollment ID to delete (number): ");
            if (!int.TryParse(Console.ReadLine(), out int enrollmentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Enrollment ID.");
                return;
            }

            bool deleted = _enrollmentBLL.DeleteEnrollment(enrollmentId);
            if (deleted)
            {
                Console.WriteLine("Enrollment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete enrollment or enrollment not found.");
            }
        }

        private void DisplayAllEnrollmentDetails()
        {
            var enrollmentDetails = _enrollmentBLL.GetAllEnrollmentDetails();
            Console.WriteLine("Generate a report all enrollment information.");
            Console.WriteLine("Including studentId, name, SubjectId, title.");
            foreach (var detail in enrollmentDetails)
            {
                Console.WriteLine($"Enrollment ID: {detail.EnrollmentId}, Student ID: {detail.StudentId}, Name: {detail.StudentName}, Subject ID: {detail.SubjectId}, Title: {detail.SubjectTitle}");
            }
        }

        private void GenerateEnrollmentEmails()
        {
            // Presume there is a _studentBLL with a GetStudentById method
            var enrollments = _enrollmentBLL.GetAllEnrollments();
            var groupedEnrollments = enrollments.GroupBy(e => e.StudentID_FK).ToList();

            foreach (var group in groupedEnrollments)
            {
                int studentId = group.Key;

                // Retrieve the student details
                var student = _studentBLL.GetStudentById(studentId);
                if (student == null) continue; // Or handle this case as needed

                var fullName = $"{student.FirstName} {student.LastName}"; // Adjust format as needed

                var subjectsInfo = new List<string>();
                foreach (var enrollment in group)
                {
                    var subject = _subjectBLL.GetSubjectById(enrollment.SubjectID_FK);
                    if (subject != null)
                    {
                        subjectsInfo.Add($"{subject.Title} (ID: {subject.SubjectId})");
                    }
                }

                if (subjectsInfo.Count > 0)
                {
                    // Use fullName instead of "Student 888"
                    var emailMessage = _enrollmentBLL.GenerateEnrollmentEmail(fullName, subjectsInfo); // Adjust the signature of this method
                    Console.WriteLine(emailMessage);
                    Console.WriteLine("***************************************************************");
                }
            }
        }

    }
}

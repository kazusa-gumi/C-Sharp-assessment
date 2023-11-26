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

        public EnrollmentUIConnectedMode(EnrollmentBLLConnectedMode enrollmentBLL, SubjectBLLConnectedMode subjectBLL)
        {
            _enrollmentBLL = enrollmentBLL;
            _subjectBLL = subjectBLL;
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
                Console.WriteLine("6) Back to Main Menu");
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

            // Initialize a list to hold successfully added subjects
            var successfullyAddedSubjects = new List<string>();

            // Loop to allow multiple subject enrollments
            bool addingMoreSubjects = true;
            while (addingMoreSubjects)
            {
                Console.Write("Enter Subject ID (or 'done' to finish): ");
                var input = Console.ReadLine();

                // Check if the user has finished adding subjects
                if (input.ToLower() == "done")
                {
                    addingMoreSubjects = false;
                    continue;
                }

                // Validate the subject ID input
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

                // Create a new Enrollment object
                var newEnrollment = new Enrollment
                {
                    StudentID_FK = studentId,
                    SubjectID_FK = newSubjectId
                };

                // Add the enrollment through the BLL
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

        if (successfullyAddedSubjects.Count > 0)
        {
            var emailMessage = _enrollmentBLL.GenerateEnrollmentEmail(studentId, successfullyAddedSubjects);
            Console.WriteLine("\nEnrollment Confirmation Email:");
            Console.WriteLine(emailMessage);
        }
        else
        {
            Console.WriteLine("No new enrollments were added.");
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

    }
}

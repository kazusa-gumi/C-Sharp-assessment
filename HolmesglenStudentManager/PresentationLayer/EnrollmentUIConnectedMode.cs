using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;
using System;

namespace HolmesglenStudentManager.PresentationLayer
{
    public class EnrollmentUIConnectedMode
    {
        private readonly EnrollmentBLLConnectedMode _enrollmentBLL;

        public EnrollmentUIConnectedMode(EnrollmentBLLConnectedMode enrollmentBLL)
        {
            _enrollmentBLL = enrollmentBLL;
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

        private void AddEnrollment()
        {
            Console.WriteLine("Add New Enrollment");

            // Prompt for and validate the Student ID
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
                // Prompt for and validate the Subject ID
                Console.Write("Enter Subject ID (or 'done' to finish): ");
                var input = Console.ReadLine();

                if (input.ToLower() == "done")
                {
                    addingMoreSubjects = false;
                    continue;
                }

                if (!int.TryParse(input, out int subjectId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for Subject ID or 'done' to finish.");
                    continue;
                }

                // Create a new Enrollment object
                var newEnrollment = new Enrollment
                {
                    StudentID_FK = studentId,
                    SubjectID_FK = subjectId
                };

                // Add the enrollment through the BLL
                bool isCreated = _enrollmentBLL.AddEnrollment(newEnrollment);
                if (isCreated)
                {
                    // Fetch the subject details if needed
                    var subjectDetails = _enrollmentBLL.GetSubjectById(subjectId); // Assuming this method exists and fetches subject details.
                    successfullyAddedSubjects.Add($"{subjectDetails.Title} ({subjectDetails.SubjectId})");
                    Console.WriteLine($"Subject ID {subjectId} added successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to add subject ID {subjectId}. It might already be enrolled.");
                }
            }

            // After all subjects are processed, display the confirmation message
            if (successfullyAddedSubjects.Any())
            {
                Console.WriteLine($"Dear Student {studentId},\n");
                Console.WriteLine("You have been enrolled in the following subject:");
                foreach (var subject in successfullyAddedSubjects)
                {
                    Console.WriteLine($"> {subject}");
                }
                Console.WriteLine("\nPlease login to your account and confirm the above enrolments.\n");
                Console.WriteLine("Regards,");
                Console.WriteLine("CAIT Department");
            }
            else
            {
                Console.WriteLine("No enrollments were added.");
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

            // 登録IDの入力を求める
            Console.Write("Enter Enrollment ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int enrollmentId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Enrollment ID.");
                return;
            }

            // 既存の登録情報を取得
            var existingEnrollment = _enrollmentBLL.GetEnrollmentById(enrollmentId);
            if (existingEnrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            // 新しい学生IDの入力を求める
            Console.Write("Enter new Student ID (leave blank to keep current): ");
            var studentIdInput = Console.ReadLine();
            if (int.TryParse(studentIdInput, out int newStudentId))
            {
                existingEnrollment.StudentID_FK = newStudentId;
            }

            // 新しい科目IDの入力を求める
            Console.Write("Enter new Subject ID (leave blank to keep current): ");
            var subjectIdInput = Console.ReadLine();
            if (int.TryParse(subjectIdInput, out int newSubjectId))
            {
                existingEnrollment.SubjectID_FK = newSubjectId;
            }

            // 更新処理の実行
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

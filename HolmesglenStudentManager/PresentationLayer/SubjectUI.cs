using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;
using System;
namespace HolmesglenStudentManager.PresentationLayer
{
    public class SubjectUI
    {
        private readonly SubjectBLL _subjectBLL;
        public SubjectUI(SubjectBLL subjectBLL)
        {
            _subjectBLL = subjectBLL;
        }
        public void DisplaySubjectMenu()
        {
            bool inSubjectMenu = true;
            while (inSubjectMenu)
            {
                Console.WriteLine("\nSubject Operations:");
                Console.WriteLine("1) Create Subject");
                Console.WriteLine("2) List All Subjects");
                Console.WriteLine("3) Update Subject");
                Console.WriteLine("4) Delete Subject");
                Console.WriteLine("5) Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CreateSubject();
                        break;
                    case "2":
                        ListSubjects();
                        break;
                    case "3":
                        UpdateSubject();
                        break;
                    case "4":
                        DeleteSubject();
                        break;
                    case "5":
                        inSubjectMenu = false;  // Exit the subject menu
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
        public void CreateSubject()
        {
            Console.Write("Enter subject ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for subject ID.");
                return;
            }
            Console.Write("Enter subject title: ");
            string title = Console.ReadLine();
            Console.Write("Enter number of sessions: ");
            if (!int.TryParse(Console.ReadLine(), out int numberOfSessions))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for the number of sessions.");
                return;
            }
            Console.Write("Enter hours per session: ");
            if (!int.TryParse(Console.ReadLine(), out int hoursPerSession))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for hours per session.");
                return;
            }

            var newSubject = new Subject
            {
                SubjectId = subjectId,
                Title = title,
                NumberOfSession = numberOfSessions,
                HourPerSession = hoursPerSession
                // その他のプロパティについても同様に入力を受け付けます。
            };

            bool isCreated = _subjectBLL.AddSubject(newSubject);
            if (isCreated)
            {
                Console.WriteLine("Subject added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add subject. It might already exist.");
            }
        }
        private void ListSubjects()
        {
            var subjects = _subjectBLL.GetAllSubjects();
            foreach (var subject in subjects)
            {
                Console.WriteLine($"ID: {subject.SubjectId}, Title: {subject.Title}, Number of Sessions: {subject.NumberOfSession}, Hours per Session: {subject.HourPerSession}");
            }
        }
        public void UpdateSubject()
        {
            Console.Write("Enter subject ID to update (number): ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for subject ID.");
                return;
            }

            var subjectToUpdate = _subjectBLL.GetSubjectById(subjectId);
            if (subjectToUpdate == null)
            {
                Console.WriteLine("Subject not found.");
                return;
            }

            Console.Write("Enter new title (leave blank to keep current): ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTitle))
            {
                subjectToUpdate.Title = newTitle;
            }

            Console.Write("Enter new number of sessions (leave blank to keep current): ");
            if (int.TryParse(Console.ReadLine(), out int newNumberOfSessions))
            {
                subjectToUpdate.NumberOfSession = newNumberOfSessions;
            }

            Console.Write("Enter new hours per session (leave blank to keep current): ");
            if (int.TryParse(Console.ReadLine(), out int newHoursPerSession))
            {
                subjectToUpdate.HourPerSession = newHoursPerSession;
            }

            bool success = _subjectBLL.UpdateSubject(subjectToUpdate);
            if (success)
            {
                Console.WriteLine("Subject updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update subject.");
            }
        }

        public void DeleteSubject()
        {
            Console.Write("Enter subject ID to delete (number): ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for subject ID.");
                return;
            }

            bool success = _subjectBLL.DeleteSubject(subjectId);
            if (success)
            {
                Console.WriteLine("Subject deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete subject or subject not found.");
            }
        }

    }
}
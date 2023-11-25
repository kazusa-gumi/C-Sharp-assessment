using HolmesglenStudentManager;
using HolmesglenStudentManager.Models;
using HolmesglenStudentManager.BusinessLogicLayer;

using System;
namespace HolmesglenStudentManager.PresentationLayer
{
    public class SubjectUIConnectedMode
    {
        private readonly SubjectBLLConnectedMode _subjectBLL;

        public SubjectUIConnectedMode(SubjectBLLConnectedMode subjectBLL)
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
                        ListSubject();
                        break;
                    case "3":
                        UpdateSubject();
                        break;
                    case "4":
                        DeleteSubject();
                        break;
                    case "5":
                        inSubjectMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
        private void CreateSubject()
        {
            Console.Write("Enter subject ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for subject ID.");
                return;
            }
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter NumberOfSession (number): ");
            if (!int.TryParse(Console.ReadLine(), out int numberOfSession))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for NumberOfSession.");
                return;
            }
            Console.Write("Enter HourPerSession (number): ");
            if (!int.TryParse(Console.ReadLine(), out int hourPerSession))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for HourPerSession.");
                return;
            }
            var newSubject = new Subject
            {
                SubjectId = subjectId,
                Title = title,
                NumberOfSession = numberOfSession,
                HourPerSession = hourPerSession
            };
            bool isCreated = _subjectBLL.AddSubject(newSubject);
            if (isCreated)
            {
                Console.WriteLine("Subject added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add subject. ID might already exist or input is invalid.");
            }
        }
        private void ListSubject()
        {
            var subjects = _subjectBLL.GetAllSubjects();
            foreach (var subject in subjects)
            {
                Console.WriteLine($"ID: {subject.SubjectId}, Title: {subject.Title} NumberOfSession: {subject.NumberOfSession}, HourPerSession: {subject.HourPerSession}");
            }
        }

        private void UpdateSubject()
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
            var title = Console.ReadLine();
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

            // SubjectBLLを通じて科目情報を更新し、結果をユーザーに報告
            bool updated = _subjectBLL.UpdateSubject(subjectToUpdate);
            if (updated)
            {
                Console.WriteLine("Subject updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update subject.");
            }
        }


        private void DeleteSubject()
        {
            Console.Write("Enter subject ID to delete (number): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid input. Please enter a valid number for subject ID.");
                return;
            }

            bool deleted = _subjectBLL.DeleteSubject(id);
            if (deleted)
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


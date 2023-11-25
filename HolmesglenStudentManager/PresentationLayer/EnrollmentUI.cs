using System;
using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;

namespace HolmesglenStudentManager.PresentationLayer
{
    public class EnrollmentUI
    {
        private readonly EnrollmentBLL _enrollmentBLL;
        private readonly StudentBLL _studentBLL;
        private readonly SubjectBLL _subjectBLL;

        public EnrollmentUI(EnrollmentBLL enrollmentBLL, StudentBLL studentBLL, SubjectBLL subjectBLL)
        {
            _enrollmentBLL = enrollmentBLL;
            _studentBLL = studentBLL;
            _subjectBLL = subjectBLL;
        }

        public void DisplayEnrollmentMenu()
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
                    case "1": // Option to enroll a student in multiple subjects
                        Console.Write("Enter student ID (number): ");
                        if (!int.TryParse(Console.ReadLine(), out int studentId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                            break;
                        }

                        bool addingMoreSubjects = true;
                        while (addingMoreSubjects)
                        {
                            Console.Write("Enter subject ID (number) or press 'x' to finish: ");
                            var input = Console.ReadLine();
                            if (input.ToLower() == "x") // "x"で終了
                            {
                                addingMoreSubjects = false; // ループを終了する
                            }
                            else
                            {
                                if (int.TryParse(input, out int subjectId)) // 科目IDの解析を試みる
                                {
                                    var newEnrollment = new Enrollment
                                    {
                                        StudentID_FK = studentId,
                                        SubjectID_FK = subjectId
                                    };

                                    // EnrollmentBLLを使用して登録を追加し、ユーザーに結果を知らせる
                                    if (_enrollmentBLL.AddEnrollment(newEnrollment))
                                    {
                                        Console.WriteLine($"Successfully added to Enrollment. Subject ID: {subjectId}.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to add enrollment for Subject ID: {subjectId}. It might already exist.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid number for Subject ID or 'x' to finish.");
                                }
                            }
                        }
                        break;

                    case "2": // Option to list all existing enrollments
                        var allEnrollments = _enrollmentBLL.GetAllEnrollments();
                        // 生徒IDごとにグループ化と科目IDのコレクションを作成します。
                        var groupedEnrollments = allEnrollments
                            .GroupBy(e => e.StudentID_FK)
                            .Select(g => new
                            {
                                StudentId = g.Key,
                                SubjectIds = g.Select(e => e.SubjectID_FK).ToList()
                            });

                        // 各グループの詳細を表示します。
                        foreach (var group in groupedEnrollments)
                        {
                            var subjectIdsString = string.Join(", ", group.SubjectIds); // 科目IDをカンマで区切る
                            Console.WriteLine($"Student ID: {group.StudentId}, Subject IDs: {subjectIdsString}");
                        }
                        break;

                    case "3": // Option to update an existing enrollment for a student
                        Console.Write("Enter the student ID for updating enrollments (number): ");
                        if (!int.TryParse(Console.ReadLine(), out int updatingStudentId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number for student ID.");
                            break;
                        }

                        // A list to keep track of updated subject Ids, to avoid same subject entered multiple times
                        List<int> updatedSubjects = new List<int>();

                        bool updatingEnrollments = true;
                        while (updatingEnrollments)
                        {
                            Console.Write("Enter new subject ID (number) or 'x' to finish: ");
                            var input = Console.ReadLine();
                            if (input.ToLower() == "x")
                            {
                                updatingEnrollments = false;
                                continue;
                            }

                            if (int.TryParse(input, out int newSubjectId) && !updatedSubjects.Contains(newSubjectId))
                            {
                                var subject = _subjectBLL.GetSubjectById(newSubjectId);
                                if (subject != null) // ここでsubjectオブジェクトがnullではないことを確認する
                                {
                                    // subjectが存在する場合の処理
                                }
                                else
                                {
                                    Console.WriteLine($"Invalid subject ID: {newSubjectId}. Subject not found.");
                                }

                                var newEnrollment = new Enrollment
                                {
                                    StudentID_FK = updatingStudentId,
                                    SubjectID_FK = newSubjectId
                                };

                                if (_enrollmentBLL.AddEnrollment(newEnrollment))
                                {
                                    Console.WriteLine($"Enrollment for subject ID {newSubjectId} added successfully.");
                                    updatedSubjects.Add(newSubjectId); // Remember this subject has been added
                                }
                                else
                                {
                                    Console.WriteLine("Failed to add enrollment. It might already exist.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input or subject already updated.");
                            }
                        }
                        break;
                    case "4": // Option to cancel or delete an enrollment
                        Console.Write("Enter enrollment ID to cancel: "); // Prompt for enrollment ID
                        if (int.TryParse(Console.ReadLine(), out int enrollmentId)) // enrollmentId をこの場で宣言
                        {
                            // Attempt to delete the enrollment with the specified ID using the EnrollmentBLL
                            if (_enrollmentBLL.DeleteEnrollment(enrollmentId))
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
                        break;
                    case "5": // 新しいオプションとしてレポート生成機能を追加
                        _enrollmentBLL.GenerateEnrollmentReport();
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

        }
    
}

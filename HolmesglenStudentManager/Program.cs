using CsvHelper;
using System.Globalization;
using HolmesglenStudentManager.BusinessLogicLayer;
using HolmesglenStudentManager.Models;

namespace HolmesglenStudentManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Holmesglen Student Management System");

            var db = new AppDBContext();
            var studentBLL = new StudentBLL(db);
            var subjectBLL = new SubjectBLL(db); // SubjectBLLのインスタンスを生成
            var enrollmentBLL = new EnrollmentBLL(db); // EnrollmentBLLのインスタンスを生成

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1) Student operations");
                Console.WriteLine("2) Subject operations");
                Console.WriteLine("3) Enrollment operations");
                Console.WriteLine("4) Exit");
                Console.Write("Please enter an option: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        // 生徒に関する操作のサブメニューを表示
                        DisplayStudentMenu(studentBLL);
                        break;
                    case "2":
                        // 科目に関する操作のサブメニューを表示
                        DisplaySubjectMenu(subjectBLL);
                        break;
                    case "3":
                        // 登録に関する操作のサブメニューを表示
                        DisplayEnrollmentMenu(enrollmentBLL, studentBLL, subjectBLL);
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }

            Console.WriteLine("Thank you for using Holmesglen Student Management System.");
        }

        static void DisplayStudentMenu(StudentBLL studentBLL)
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
                    case "1": // 生徒の作成
                        Console.Write("Enter student ID: ");
                        string studentId = Console.ReadLine();
                        Console.Write("Enter first name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Enter last name: ");
                        string lastName = Console.ReadLine();
                        Console.Write("Enter email: ");
                        string email = Console.ReadLine();
                        var newStudent = new Student
                        {
                            StudentId = studentId,
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email
                        };
                        if (studentBLL.AddStudent(newStudent))
                        {
                            Console.WriteLine("Student added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add student. ID might already exist or input is invalid.");
                        }
                        break;
                    case "2": // すべての生徒を表示
                        var allStudents = studentBLL.GetAllStudents();
                        foreach (var student in allStudents)
                        {
                            Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}, Email: {student.Email}");
                        }
                        break;
                    case "3": // 生徒情報の更新
                        Console.Write("Enter student ID to update: ");
                        string updateId = Console.ReadLine();
                        var studentToUpdate = studentBLL.GetStudentById(updateId);
                        if (studentToUpdate != null)
                        {
                            Console.Write("Enter new first name (leave blank to keep current): ");
                            studentToUpdate.FirstName = Console.ReadLine();
                            Console.Write("Enter new last name (leave blank to keep current): ");
                            studentToUpdate.LastName = Console.ReadLine();
                            Console.Write("Enter new email (leave blank to keep current): ");
                            studentToUpdate.Email = Console.ReadLine();
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
                            Console.WriteLine("Student not found.");
                        }
                        break;
                    case "4": // 生徒の削除
                        Console.Write("Enter student ID to delete: ");
                        string deleteId = Console.ReadLine();
                        if (studentBLL.DeleteStudent(deleteId))
                        {
                            Console.WriteLine("Student deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete student or student not found.");
                        }
                        break;
                    case "5":
                        // メインメニューに戻る
                        inStudentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        static void DisplaySubjectMenu(SubjectBLL subjectBLL)
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
                    case "1": // 科目の作成
                        Console.Write("Enter subject ID: ");
                        string subjectId = Console.ReadLine();
                        Console.Write("Enter subject title: ");
                        string title = Console.ReadLine();
                        Console.Write("Enter number of sessions: ");
                        int numberOfSession;
                        while (!int.TryParse(Console.ReadLine(), out numberOfSession))
                        {
                            Console.Write("Invalid input - please enter a valid number of sessions: ");
                        }
                        Console.Write("Enter hours per session: ");
                        int hourPerSession;
                        while (!int.TryParse(Console.ReadLine(), out hourPerSession))
                        {
                            Console.Write("Invalid input - please enter a valid number of hours per session: ");
                        }

                        var newSubject = new Subject
                        {
                            SubjectId = subjectId,
                            Title = title,
                            NumberOfSession = numberOfSession, // 追加したプロパティ
                            HourPerSession = hourPerSession // 追加したプロパティ
                        };

                        if (subjectBLL.AddSubject(newSubject))
                        {
                            Console.WriteLine("Subject added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add subject. ID might already exist or input is invalid.");
                        }
                        break;


                    case "2": // すべての科目を表示
                        var allSubjects = subjectBLL.GetAllSubjects();
                        foreach (var subject in allSubjects)
                        {
                            Console.WriteLine($"ID: {subject.SubjectId}, Title: {subject.Title}, " +
                        $"Sessions: {subject.NumberOfSession}, Hours/Session: {subject.HourPerSession}");
                        }
                        break;

                        case "3": // 科目情報の更新
                        Console.Write("Enter subject ID to update: ");
                        string updateId = Console.ReadLine();
                        var subjectToUpdate = subjectBLL.GetSubjectById(updateId);
                        if (subjectToUpdate != null)
                        {
                            Console.Write("Enter new title (leave blank to keep current): ");
                            var newTitle = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newTitle))
                            {
                                subjectToUpdate.Title = newTitle;
                            }

                            Console.Write("Enter new number of sessions (leave blank to keep current): ");
                            var newNumberOfSessionsStr = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newNumberOfSessionsStr))
                            {
                                if (int.TryParse(newNumberOfSessionsStr, out var newNumberOfSession))
                                {
                                    subjectToUpdate.NumberOfSession = newNumberOfSession;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input for number of sessions.");
                                    break;
                                }
                            }

                            Console.Write("Enter new hours per session (leave blank to keep current): ");
                            var newHoursPerSessionStr = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newHoursPerSessionStr))
                            {
                                if (int.TryParse(newHoursPerSessionStr, out var newHourPerSession))
                                {
                                    subjectToUpdate.HourPerSession = newHourPerSession;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input for hours per session.");
                                    break;
                                }
                            }

                            if (subjectBLL.UpdateSubject(subjectToUpdate))
                            {
                                Console.WriteLine("Subject updated successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Failed to update subject.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Subject not found.");
                        }
                        break;

                    case "4": // 科目の削除
                        Console.Write("Enter subject ID to delete: ");
                        string deleteId = Console.ReadLine();
                        if (subjectBLL.DeleteSubject(deleteId))
                        {
                            Console.WriteLine("Subject deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete subject or subject not found.");
                        }
                        break;

                    case "5": // メインメニューに戻る
                        inSubjectMenu = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
            
        }
        static void DisplayEnrollmentMenu(EnrollmentBLL enrollmentBLL, StudentBLL studentBLL, SubjectBLL subjectBLL)
        {
            bool inEnrollmentMenu = true;
            while (inEnrollmentMenu)
            {
                Console.WriteLine("\nEnrollment Operations:");
                Console.WriteLine("1) Create Enroll Student");
                Console.WriteLine("2) List All Enrollments");
                Console.WriteLine("3) Update Enrollment");
                Console.WriteLine("4) Cancel Enrollment");
                Console.WriteLine("5) Back to Main Menu");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {


                    case "1": // 生徒を新しい科目に登録
                              // IDの入力を受け取る
                        Console.Write("Enter student ID: ");
                        string studentId = Console.ReadLine();
                        Console.Write("Enter subject ID: ");
                        string subjectId = Console.ReadLine();

                        // Enrollmentオブジェクトの作成
                        var newEnrollment = new Enrollment
                        {
                            StudentID = studentId,
                            SubjectID = subjectId
                        };

                        // EnrollmentBLLを使用してデータベースに新しい登録を追加
                        if (enrollmentBLL.AddEnrollment(newEnrollment, studentBLL, subjectBLL))
                        {
                            Console.WriteLine("Enrollment added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add enrollment. The enrollment might already exist or the input is invalid.");
                        }
                        break;

                    case "2": // 全ての登録をリスト表示
                        var allEnrollments = enrollmentBLL.GetAllEnrollments();
                        foreach (var enrollment in allEnrollments)
                        {
                            Console.WriteLine($"Enrollment ID: {enrollment.Id}, " +
                                              $"Student ID: {enrollment.StudentID}, " +
                                              $"Subject ID: {enrollment.SubjectID}");
                        }
                        break;
                    case "3": // 登録情報の更新
                        Console.Write("Enter the enrollment ID to update: ");
                        int enrollmentId; // 変数enrollmentIdをここで宣言
                        if (int.TryParse(Console.ReadLine(), out enrollmentId))
                        {
                            var enrollmentToUpdate = enrollmentBLL.GetEnrollmentById(enrollmentId);
                            // ...残りの更新ロジック...
                        }
                        else
                        {
                            Console.WriteLine("Invalid enrollment ID.");
                            break;
                        }
                        break;
                    case "4": // 登録をキャンセル（削除）
                        Console.Write("Enter enrollment ID to cancel: ");
                        if (int.TryParse(Console.ReadLine(), out enrollmentId)) // 同じ変数enrollmentIdをケース4でも使用
                        {
                            if (enrollmentBLL.DeleteEnrollment(enrollmentId))
                            {
                                Console.WriteLine("Enrollment cancelled successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to cancel enrollment or enrollment not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid enrollment ID.");
                        }
                        break;
                    case "5": // メインメニューに戻る
                        inEnrollmentMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }



    }
}
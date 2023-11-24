using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class EnrollmentBLL
    {
        private AppDBContext _db;

        public EnrollmentBLL(AppDBContext appDBContext)
        {
            _db = appDBContext;
        }

        public void GenerateEnrollmentReport()
        {
            var enrollments = GetAllEnrollments();
            Console.WriteLine($"{"Student ID",-15} {"Name",-30} {"Subject ID",-15} {"Title",-30}");
            foreach (var enrollment in enrollments)
            {
                var student = _db.Student.FirstOrDefault(s => s.StudentId == enrollment.StudentID_FK);
                var subject = _db.Subject.FirstOrDefault(s => s.SubjectId == enrollment.SubjectID_FK);
                if (student != null && subject != null)
                {
                    Console.WriteLine($"{student.StudentId,-15} {student.FirstName} {student.LastName,-30} {subject.SubjectId,-15} {subject.Title,-30}");
                }
            }
        }


        // すべての登録を取得するメソッド
        public List<Enrollment> GetAllEnrollments()
        {
            return _db.Enrollment.ToList();
        }

        // 特定のIDを持つ登録を取得するメソッド
        public Enrollment GetEnrollmentById(int id)
        {
            // Findメソッドは主キーを使ってデータベースからエンティティを探し出します。
            return _db.Enrollment.Find(id);
        }

        public bool AddEnrollment(Enrollment newEnrollment)
        {
            var existingEnrollment = _db.Enrollment
                .Any(e => e.StudentID_FK == newEnrollment.StudentID_FK
                    && e.SubjectID_FK == newEnrollment.SubjectID_FK);

            if (!existingEnrollment)
            {
                _db.Enrollment.Add(newEnrollment);
                _db.SaveChanges();

                // Fetch associated student and subject details
                var student = _db.Student.FirstOrDefault(s => s.StudentId == newEnrollment.StudentID_FK);
                var subject = _db.Subject.FirstOrDefault(su => su.SubjectId == newEnrollment.SubjectID_FK);

                // Generate the enrollment confirmation message
                string message =
                    $"Dear {student.LastName},\n\n" +
                    $"You have been enrolled in the following subject\n" +
                    $"> {subject.Title} ({subject.SubjectId})\n\n" +
                    $"Please login to your account and confirm the above enrolments.\n\n" +
                    $"Regards,";

                // Display the message in the console
                Console.WriteLine(message);

                return true;
            }
            return false;
        }


        public bool UpdateEnrollment(Enrollment enrollment, StudentBLL studentBLL, SubjectBLL subjectBLL)
        {
            // StudentID が存在するかチェック
            var studentExists = studentBLL.GetStudentById(enrollment.StudentID_FK) != null;
            if (!studentExists)
            {
                Console.WriteLine($"StudentID '{enrollment.StudentID_FK}' does not exist. Please create Student information first.");
                return false;
            }

            // SubjectID が存在するかチェック
            var subjectExists = subjectBLL.GetSubjectById(enrollment.SubjectID_FK) != null;
            if (!subjectExists)
            {
                Console.WriteLine($"SubjectID '{enrollment.SubjectID_FK}' does not exist. Please create Subject information first.");
                return false;
            }

            // 更新が必要な `Enrollment` エンティティを取得
            var enrollmentToUpdate = _db.Enrollment.Find(enrollment.Id);
            if (enrollmentToUpdate == null)
            {
                // 更新する登録が存在しない場合
                Console.WriteLine($"Enrollment ID '{enrollment.Id}' not found.");
                return false;
            }

            // 登録情報を更新する
            enrollmentToUpdate.StudentID_FK = enrollment.StudentID_FK;
            enrollmentToUpdate.SubjectID_FK = enrollment.SubjectID_FK;
            _db.SaveChanges();
            return true; // 更新成功
        }
        // 登録を削除するメソッド（登録解除）
        public bool DeleteEnrollment(int enrollmentId)
        {
            var enrollmentToDelete = _db.Enrollment.Find(enrollmentId);
            if (enrollmentToDelete != null)
            {
                _db.Enrollment.Remove(enrollmentToDelete);
                _db.SaveChanges();
                return true; // 削除成功
            }
            return false; // 登録が見つからないため、削除失敗
        }


    }
}
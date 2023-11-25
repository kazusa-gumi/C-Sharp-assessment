using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class EnrollmentBLL
    {
        private readonly EnrollmentDAL _enrollmentDal;
        private readonly StudentBLL _studentBll;
        private readonly SubjectBLL _subjectBll;

        // コンストラクタは引数としてDALとBLLのインスタンスを受け取ります
        public EnrollmentBLL(EnrollmentDAL enrollmentDal, StudentBLL studentBll, SubjectBLL subjectBll)
        {
            _enrollmentDal = enrollmentDal;
            _studentBll = studentBll;
            _subjectBll = subjectBll;
        }

        public List<Enrollment> GetAllEnrollments()
        {
            return _enrollmentDal.GetAllEnrollments();
        }

        public void GenerateEnrollmentReport()
        {
            var enrollments = _enrollmentDal.GetAllEnrollments();
            Console.WriteLine($"{"Student ID",-15} {"Name",-30} {"Subject ID",-15} {"Title",-30}");
            foreach (var enrollment in enrollments)
            {
                var student = _studentBll.GetStudentById(enrollment.StudentID_FK);
                var subject = _subjectBll.GetSubjectById(enrollment.SubjectID_FK);
                if (student != null && subject != null)
                {
                    Console.WriteLine($"{student.StudentId,-15} {student.FirstName} {student.LastName,-30} {subject.SubjectId,-15} {subject.Title,-30}");
                }
            }
        }

        // 他の GetAllEnrollments、GetEnrollmentById、AddEnrollment、および DeleteEnrollment メソッドも必要に応じて更新しておく必要があります。

        // 追加 (Create)
        public bool AddEnrollment(Enrollment newEnrollment)
        {
            if (!_enrollmentDal.EnrollmentExists(newEnrollment.StudentID_FK, newEnrollment.SubjectID_FK))
            {
                _enrollmentDal.AddEnrollment(newEnrollment);

                // Fetch associated student and subject details
                var student = _studentBll.GetStudentById(newEnrollment.StudentID_FK);
                var subject = _subjectBll.GetSubjectById(newEnrollment.SubjectID_FK);

                // Generate the enrollment confirmation message
                string message = $"Dear {student.LastName},\n\n"
                               + $"You have been enrolled in the following subject:\n"
                               + $"{subject.Title} (ID: {subject.SubjectId})\n\n"
                               + $"Please login to your account and confirm the above enrollment.\n\n"
                               + $"Regards,\n"
                               + $"The Administration";
                Console.WriteLine(message);

                return true;
            }
            else
            {
                Console.WriteLine($"Enrollment already exists for student ID {newEnrollment.StudentID_FK} and subject ID {newEnrollment.SubjectID_FK}.");
                return false;
            }
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
            var enrollmentToUpdate = _enrollmentDal.GetEnrollmentById(enrollment.Id);
            if (enrollmentToUpdate == null)
            {
                Console.WriteLine($"Enrollment ID '{enrollment.Id}' not found.");
                return false;
            }

            // 登録情報を更新する
            enrollmentToUpdate.StudentID_FK = enrollment.StudentID_FK;
            enrollmentToUpdate.SubjectID_FK = enrollment.SubjectID_FK;
            _enrollmentDal.UpdateEnrollment(enrollmentToUpdate); // DALを介した更新操作
            return true; // 更新成功
        }

        public Enrollment GetEnrollmentById(int enrollmentId)
        {
            return _enrollmentDal.GetEnrollmentById(enrollmentId);
        }

        // 登録を削除するメソッド（登録解除）
        public bool DeleteEnrollment(int enrollmentId)
        {
            var enrollmentToDelete = _enrollmentDal.GetEnrollmentById(enrollmentId);
            if (enrollmentToDelete != null)
            {
                _enrollmentDal.DeleteEnrollment(enrollmentToDelete);
                return true; // 削除成功
            }
            return false; // 登録が見つからないため、削除失敗
        }
    }
}
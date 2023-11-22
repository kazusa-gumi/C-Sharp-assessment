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

        public bool AddEnrollment(Enrollment newEnrollment, StudentBLL studentBLL, SubjectBLL subjectBLL)
        {
            // StudentID が存在するかチェック
            if (_db.Student.Any(s => s.StudentId == newEnrollment.StudentID) == false)
            {
                Console.WriteLine($"StudentID '{newEnrollment.StudentID}' does not exist.");
                return false;
            }

            // SubjectID が存在するかチェック
            if (_db.Subject.Any(s => s.SubjectId == newEnrollment.SubjectID) == false)
            {
                Console.WriteLine($"SubjectID '{newEnrollment.SubjectID}' does not exist.");
                return false;
            }

            // 登録の重複をチェック（同じ科目に複数回登録できないと仮定）
            var existingEnrollment = _db.Enrollment
                .FirstOrDefault(e => e.StudentID == newEnrollment.StudentID && e.SubjectID == newEnrollment.SubjectID);
            if (existingEnrollment == null)
            {
                _db.Enrollment.Add(newEnrollment);
                _db.SaveChanges();
                return true; // 登録成功
            }
            return false; // 既に登録が存在するため、登録失敗
        }

public bool UpdateEnrollment(Enrollment enrollment, StudentBLL studentBLL, SubjectBLL subjectBLL)
        {
            // StudentID が存在するかチェック
            var studentExists = studentBLL.GetStudentById(enrollment.StudentID) != null;
            if (!studentExists)
            {
                Console.WriteLine($"StudentID '{enrollment.StudentID}' does not exist. Please create Student information first.");
                return false;
            }

            // SubjectID が存在するかチェック
            var subjectExists = subjectBLL.GetSubjectById(enrollment.SubjectID) != null;
            if (!subjectExists)
            {
                Console.WriteLine($"SubjectID '{enrollment.SubjectID}' does not exist. Please create Subject information first.");
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
            enrollmentToUpdate.StudentID = enrollment.StudentID;
            enrollmentToUpdate.SubjectID = enrollment.SubjectID;
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


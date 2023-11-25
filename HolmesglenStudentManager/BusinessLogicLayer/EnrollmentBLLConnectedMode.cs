using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class EnrollmentBLLConnectedMode
    {
        private readonly EnrollmentDALConnectedMode _enrollmentDal;

        public EnrollmentBLLConnectedMode(EnrollmentDALConnectedMode enrollmentDal)
        {
            _enrollmentDal = enrollmentDal;
        }


        public Subject GetSubjectById(int subjectId)
        {
            return _enrollmentDal.GetSubjectById(subjectId);
        }

        public List<Enrollment> GetAllEnrollments()
        {
            try
            {
                // DALを使用してすべての登録を取得
                return _enrollmentDal.GetAllEnrollments();
            }
            catch (Exception ex)
            {
                // エラーハンドリング
                Console.WriteLine($"Error in GetAllEnrollments: {ex.Message}");
                return new List<Enrollment>();
            }
        }

        public Enrollment GetEnrollmentById(int id)
        {
            try
            {
                // 特定のIDを持つ登録を取得
                return _enrollmentDal.GetEnrollmentById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEnrollmentById: {ex.Message}");
                return null;
            }
        }

        public bool AddEnrollment(Enrollment newEnrollment)
        {
            try
            {
                // 登録の有効性を検証するロジックを追加することができます
                // 例: 学生IDと科目IDの有効性チェック
                return _enrollmentDal.AddEnrollment(newEnrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddEnrollment: {ex.Message}");
                return false;
            }
        }

        public bool UpdateEnrollment(Enrollment existingEnrollment)
        {
            try
            {
                // 登録情報の更新
                return _enrollmentDal.UpdateEnrollment(existingEnrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateEnrollment: {ex.Message}");
                return false;
            }
        }

        public bool DeleteEnrollment(int enrollmentId)
        {
            try
            {
                // 登録の削除
                return _enrollmentDal.DeleteEnrollment(enrollmentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteEnrollment: {ex.Message}");
                return false;
            }
        }

        public List<EnrollmentDetail> GetAllEnrollmentDetails()
        {
            try
            {
                return _enrollmentDal.GetAllEnrollmentDetails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllEnrollmentDetails: {ex.Message}");
                return new List<EnrollmentDetail>();
            }
        }

    }
}

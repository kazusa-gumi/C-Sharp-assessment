using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class EnrollmentBLLConnectedMode
    {
        private readonly EnrollmentDALConnectedMode _enrollmentDal;
        private readonly StudentDALConnectedMode _studentDal;

        public EnrollmentBLLConnectedMode(EnrollmentDALConnectedMode enrollmentDal, StudentDALConnectedMode studentDal)
        {
            _enrollmentDal = enrollmentDal;
            _studentDal = studentDal;
        }

        public Subject GetSubjectById(int subjectId)
        {
            return _enrollmentDal.GetSubjectById(subjectId);
        }

        public List<Enrollment> GetAllEnrollments()
        {
            try
            {
                
                return _enrollmentDal.GetAllEnrollments();
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error in GetAllEnrollments: {ex.Message}");
                return new List<Enrollment>();
            }
        }

        public Enrollment GetEnrollmentById(int id)
        {
            try
            {
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

        public string GenerateEnrollmentEmail(string studentFullName, List<string> subjects)
        {
            var emailMessage = $"Dear {studentFullName},\n\n" +
                               "You have been enrolled in the following subjects:\n" +
                               string.Join("\n", subjects.Select(s => $"- {s}")) +
                               "\n\nPlease log in to your account to confirm these enrollments.\n" +
                               "Regards,\n" +
                               "The Enrollment Department";
            return emailMessage;
        }

        // GenerateAndPrintEnrollmentEmailsメソッドの実装
        public void GenerateAndPrintEnrollmentEmails()
        {
            var enrollments = GetAllEnrollments(); // すべてのEnrollmentを取得する
            var groupedByStudentId = enrollments.GroupBy(e => e.StudentID_FK);

            foreach (var studentGroup in groupedByStudentId)
            {
                int studentId = studentGroup.Key;

                // StudentDALから学生のフルネームを取得します。
                var student = _studentDal.GetStudentById(studentId);
                var studentFullName = $"{student.LastName} {student.FirstName}";

                List<string> subjectsInfo = studentGroup.Select(enrollment =>
                {
                    var subject = _enrollmentDal.GetSubjectById(enrollment.SubjectID_FK);
                    return $"{subject.Title} (ID: {subject.SubjectId})";
                }).ToList();

                // 修正されたGenerateEnrollmentEmailを呼び出して、メール本文を取得します。
                var emailMessage = GenerateEnrollmentEmail(studentFullName, subjectsInfo);

                // メールメッセージをコンソールに出力します。
                Console.WriteLine(emailMessage);
                Console.WriteLine("************************************************************************************************************");
            }
        }
    }
}

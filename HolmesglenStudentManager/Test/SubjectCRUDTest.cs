using Microsoft.VisualStudio.TestTools.UnitTesting;
using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System;

namespace HolmesglenStudentManager.Tests
{
    [TestClass]
    public class EnrollmentTests
    {
        private readonly string connectionString = "Your Connection String Here";
        private readonly EnrollmentDALConnectedMode dal;

        public EnrollmentTests()
        {
            dal = new EnrollmentDALConnectedMode(connectionString);
        }

        public void AddEnrollment_Should_AddRecord()
        {
            // Arrange
            var enrollment = new Enrollment
            {
                StudentID_FK = 1, // Replace with actual student ID
                SubjectID_FK = 101 // Replace with actual subject ID
            };

            // Act
            var result = dal.AddEnrollment(enrollment);

            // Assert
            Assert.IsTrue(result);
        }

        public void GetEnrollmentById_Should_ReturnEnrollment()
        {
            // Arrange
            var dal = new EnrollmentDALConnectedMode(connectionString);
            int enrollmentId = 1; // Replace with actual enrollment ID

            // Act
            var result = dal.GetEnrollmentById(enrollmentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(enrollmentId, result.Id);
        }

        public void UpdateEnrollment_Should_UpdateRecord()
        {
            // Arrange
            var enrollmentToUpdate = dal.GetEnrollmentById(1); // 存在するEnrollment IDを指定してください。
            enrollmentToUpdate.StudentID_FK = 2; // 存在する別の学生IDに更新
            enrollmentToUpdate.SubjectID_FK = 102; // 存在する別の科目IDに更新

            // Act
            var result = dal.UpdateEnrollment(enrollmentToUpdate);

            // Assert
            Assert.IsTrue(result);
        }

        public void ListEnrollments_Should_ReturnAllEnrollments()
        {
            // Act
            var result = dal.GetAllEnrollments();

            // Assert
            Assert.IsTrue(result.Count > 0);
        }

        public void DeleteEnrollment_Should_RemoveRecord()
        {
            // Arrange
            int enrollmentIdToDelete = 1; // 削除するEnrollment IDを指定してください。

            // Act
            var result = dal.DeleteEnrollment(enrollmentIdToDelete);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(dal.GetEnrollmentById(enrollmentIdToDelete)); // 削除後にEnrollmentが存在しないことを確認
        }

        public void Cleanup()
        {
            var allEnrollments = dal.GetAllEnrollments();
            foreach (var enrollment in allEnrollments)
            {
                if (enrollment.Id > 1000) // 仮に1000以上のIDはテストデータと仮定
                {
                    dal.DeleteEnrollment(enrollment.Id);
                }
            }
        }
    }
}

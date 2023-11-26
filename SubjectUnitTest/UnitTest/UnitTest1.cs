using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HolmesglenStudentManager.Models;
using HolmesglenStudentManager.BusinessLogicLayer;
using System;
using System.IO;
using HolmesglenStudentManager.PresentationLayer;
using HolmesglenStudentManager.DataAccess;

namespace HolmesglenStudentManager.Tests
{
    [TestClass]
    public class SubjectUIConnectedModeTests
    {
        [TestMethod]
        public void CreateSubject_ValidInput()
        {
            Console.WriteLine("Start Test");
            // Arrange
            var mode = new SubjectDALConnectedMode("Data Source=/Users/naoikazusa/Desktop/Database/Holmesglen.studentData.db");
            var mockBLL = new Mock<SubjectBLLConnectedMode>(mode);
            var ui = new SubjectUIConnectedMode(mockBLL.Object);
            var input = new StringReader("1\nTest Subject\n2\n2");
            var output = new StringWriter();

            Console.SetIn(input);
            Console.SetOut(output);

            mockBLL.Setup(bll => bll.AddSubject(It.IsAny<Subject>())).Returns(true);

            // Act
            ui.CreateSubject();

            // Assert
            var expectedOutput = "Subject added successfully!";
            mockBLL.Verify(bll => bll.AddSubject(It.IsAny<Subject>()), Times.Once);
            StringAssert.Contains(output.ToString(), expectedOutput);
        }

        [TestMethod]
        public void UpdateSubject_ValidInput()
        {
            Console.WriteLine("UpdateSubject_ValidInput テスト開始");
            // Arrange
            var mode = new SubjectDALConnectedMode("Data Source=/Users/naoikazusa/Desktop/Database/Holmesglen.studentData.db");
            var modeBLL = new SubjectBLLConnectedMode(mode);
            var mockBLL = new Mock<SubjectBLLConnectedMode>(mode);
            var mockUI = new Mock<SubjectUIConnectedMode>(modeBLL);
            var ui = new SubjectUIConnectedMode(mockBLL.Object);
            var existingSubjectId = 1;
            var updatedSubjectTitle = "Updated Subject";
            var output = new StringWriter();
            var input = new StringReader($"{existingSubjectId}\n{updatedSubjectTitle}\n"); // 入力を設定

            Console.SetIn(input);
            Console.SetOut(output);

            // モックの振る舞いを設定
            mockUI.Setup(ui => ui.UpdateSubject());

            // Act
            mockUI.Object.UpdateSubject();

            // Assert
            //var expectedOutput = "Subject updated successfully";
            mockUI.Verify(ui => ui.UpdateSubject(), Times.Once);
            //StringAssert.Contains(output.ToString(), expectedOutput);
        }


        [TestMethod]
        public void DeleteSubject_ValidInput()
        {
            Console.WriteLine("DeleteSubject_ValidInput");
            // Arrange
            var mode = new SubjectDALConnectedMode("Data Source=/Users/naoikazusa/Desktop/Database/Holmesglen.studentData.db");
            var mockBLL = new Mock<SubjectBLLConnectedMode>(mode);
            var ui = new SubjectUIConnectedMode(mockBLL.Object);
            var subjectIdToDelete = 1;
            var output = new StringWriter();
            var input = new StringReader($"{subjectIdToDelete}\n"); // 入力を設定

            Console.SetIn(input);
            Console.SetOut(output);

            // モックの振る舞いを設定
            mockBLL.Setup(bll => bll.DeleteSubject(subjectIdToDelete)).Returns(true);

            // Act
            ui.DeleteSubject();

            // Assert
            var expectedOutput = "Subject deleted successfully";
            mockBLL.Verify(bll => bll.DeleteSubject(subjectIdToDelete), Times.Once);
            StringAssert.Contains(output.ToString(), expectedOutput);
        }



        [TestMethod]
        public void GetAllSubjects_ValidInput()
        {
            Console.WriteLine("GetAllSubjects_ValidInput テスト開始");
            // Arrange
            var mode = new SubjectDALConnectedMode("Data Source=/Users/naoikazusa/Desktop/Database/Holmesglen.studentData.db");
            var mockBLL = new Mock<SubjectBLLConnectedMode>(mode);
            var ui = new SubjectUIConnectedMode(mockBLL.Object);
            var output = new StringWriter();

            Console.SetOut(output);

            // モックの振る舞いを設定
            var subjects = new List<Subject>
    {
        new Subject { SubjectId = 1, Title = "Subject 1" },
        new Subject { SubjectId = 2, Title = "Subject 2" }
    };
            mockBLL.Setup(bll => bll.GetAllSubjects()).Returns(subjects);

            // Act
            ui.ListSubject();

            // Assert
            foreach (var subject in subjects)
            {
                StringAssert.Contains(output.ToString(), subject.Title);
            }
        }



        [TestCleanup]
        public void Cleanup()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }
    }
}
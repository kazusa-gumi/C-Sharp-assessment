using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class StudentBLL
    {
        // Database context to interact with the database
        private AppDBContext _db;

        // Constructor initializing the database context
        public StudentBLL(AppDBContext appDBContext)
        {
            _db = appDBContext;
        }

        public bool ValidateStudentID(string studenteId)
        {
            // データベースから科目IDを使って科目を検索
            var existingStudent = _db.Student.FirstOrDefault(s => s.StudentId == studenteId);
            // 科目が存在すれば true, そうでなければ false を返す
            return existingStudent != null;
        }
        // Retrieve all students from the database
        public List<Student> GetAllStudents()
        {
            // Converts the collection of students to a list and returns it
            return _db.Student.ToList();
        }

        // Fetch a single student by their ID
        public Student GetStudentById(string id)
        {
            // Searches the first student with matching StudentId or returns null if not found
            return _db.Student.FirstOrDefault(s => s.StudentId == id);
        }

        // Add a new student to the database
        public bool AddStudent(Student newStudent)
        {
            // Check if a student already exists with the same StudentId
            var existingStudent = _db.Student.FirstOrDefault(s => s.StudentId == newStudent.StudentId);
            if (existingStudent == null)
            {
                // If the student does not exist, add the new student to the database
                _db.Student.Add(newStudent);
                // Save changes to the database
                _db.SaveChanges();
                return true; // Return true to signify a successful add operation
            }
            // If the student already exists, return false to indicate a failed add operation
            return false;
        }

        // Update existing student information
        public bool UpdateStudent(Student student)
        {
            // Find the student that matches the StudentId
            var studentToUpdate = _db.Student.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (studentToUpdate != null)
            {
                // Update the student's properties if found
                studentToUpdate.FirstName = student.FirstName;
                studentToUpdate.LastName = student.LastName;
                studentToUpdate.Email = student.Email;
                // Save changes to the database
                _db.SaveChanges();
                return true; // Return true to indicate success
            }
            // If the student is not found, return false to indicate a failed update operation
            return false;
        }

        // Delete a student from the database by their ID
        public bool DeleteStudent(string id)
        {
            // Find the student that matches the ID
            var studentToDelete = _db.Student.FirstOrDefault(s => s.StudentId == id);
            if (studentToDelete != null)
            {
                // If found, remove the student from the database
                _db.Student.Remove(studentToDelete);
                // Save changes to the database
                _db.SaveChanges();
                return true; // Return true to indicate success
            }
            // If the student is not found, return false to indicate a failed delete operation
            return false;
        }
    }
}


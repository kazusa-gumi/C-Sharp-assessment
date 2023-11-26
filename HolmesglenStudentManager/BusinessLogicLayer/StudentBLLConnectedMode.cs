using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class StudentBLLConnectedMode
    {
        private readonly StudentDALConnectedMode _studentDal;

        public StudentBLLConnectedMode(StudentDALConnectedMode studentDal)
        {
            _studentDal = studentDal;
        }

        public List<Student> GetAllStudents()
        {
            try
            {
                // DALを使用してすべての学生を取得
                return _studentDal.GetAllStudents();
            }
            catch (Exception ex)
            {
                // エラーハンドリング
                Console.WriteLine($"Error in GetAllStudents: {ex.Message}");
                return new List<Student>();
            }
        }

        public Student GetStudentById(int id)
        {
            try
            {
                // 特定のIDを持つ学生を取得
                return _studentDal.GetStudentById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetStudentById: {ex.Message}");
                return null;
            }
        }

        public bool AddStudent(Student newStudent)
        {
            try
            {
                // ここで学生の有効性を検証するロジックを追加することができます
                // 例: IDの重複チェック
                if (_studentDal.GetStudentById(newStudent.StudentId) == null)
                {
                    return _studentDal.AddStudent(newStudent);
                }
                else
                {
                    Console.WriteLine("Student already exists with this ID.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddStudent: {ex.Message}");
                return false;
            }
        }

        public bool UpdateStudent(Student existingStudent)
        {
            try
            {
                // 学生情報の更新
                return _studentDal.UpdateStudent(existingStudent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateStudent: {ex.Message}");
                return false;
            }
        }

        public bool DeleteStudent(int studentId)
        {
            try
            {
                // 学生の削除
                return _studentDal.DeleteStudent(studentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteStudent: {ex.Message}");
                return false;
            }
        }

        public bool ImportStudentsFromCsv(string csvFilePath)
        {
            try
            {
                var studentsToImport = _studentDal.ReadStudentsFromCsv(csvFilePath);

                foreach (var student in studentsToImport)
                {
                    _studentDal.AddOrUpdateStudent(student);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ImportStudentsFromCsv: {ex.Message}");
                return false;
            }
        }

        public bool ExportStudentsToCsv(string csvFilePath)
        {
            try
            {
                var students = GetAllStudents(); 

                return _studentDal.ExportStudentsToCsv(students, csvFilePath);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error in ExportStudentsToCsv: {ex.Message}");
                return false;
            }
        }
    }
}

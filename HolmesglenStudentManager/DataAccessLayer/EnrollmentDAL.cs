using HolmesglenStudentManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace HolmesglenStudentManager.DataAccess
{
    public class EnrollmentDAL
    {
        private readonly AppDBContext _context;

        public EnrollmentDAL(AppDBContext context)
        {
            _context = context;
        }

        public Enrollment GetEnrollmentById(int id)
        {
            return _context.Enrollment.Find(id);
        }

        public bool EnrollmentExists(int studentId, int subjectId)
        {
            return _context.Enrollment.Any(e => e.StudentID_FK == studentId && e.SubjectID_FK == subjectId);
        }

        public List<Enrollment> GetAllEnrollments()
        {
            return _context.Enrollment.ToList();
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            _context.Enrollment.Add(enrollment);
            _context.SaveChanges();
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            _context.Enrollment.Update(enrollment);
            _context.SaveChanges();
        }

        public void DeleteEnrollment(Enrollment enrollmentToDelete)
        {
            // 削除操作
            _context.Enrollment.Remove(enrollmentToDelete);
            _context.SaveChanges();
        }

    }
}
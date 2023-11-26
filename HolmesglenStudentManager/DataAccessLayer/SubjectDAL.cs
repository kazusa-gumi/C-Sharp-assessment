using HolmesglenStudentManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace HolmesglenStudentManager.DataAccess
{
    public class SubjectDAL
    {
        private readonly AppDBContext _context;

        public SubjectDAL(AppDBContext context)
        {
            _context = context;
        }

        public List<Subject> GetAllSubjects()
        {
            return _context.Subject.ToList();
        }

        public Subject GetSubjectById(int id)
        {
            return _context.Subject.FirstOrDefault(s => s.SubjectId == id);
        }

        public void AddSubject(Subject subject)
        {
            _context.Subject.Add(subject);
            _context.SaveChanges();
        }


        public void UpdateSubject(Subject subject)
        {
            _context.Subject.Update(subject);
            _context.SaveChanges();
        }

        public void DeleteSubject(int id)
        {
            var subject = GetSubjectById(id);
            if (subject != null)
            {
                _context.Subject.Remove(subject);
                _context.SaveChanges();
            }
        }

        public bool ValidateSubjectId(int subjectId)
        {
            return _context.Subject.Any(s => s.SubjectId == subjectId);
        }
    }
}
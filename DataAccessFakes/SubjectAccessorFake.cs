using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class SubjectAccessorFake : ISubjectAccessor
    {
        List<Subject> _subjects = new List<Subject>();        

        public SubjectAccessorFake()
        {
            _subjects.Add(new Subject()
            {
                SubjectID = "Test Subject 1",
                Description = "Test Description 1",
                Active = true
            });
            _subjects.Add(new Subject()
            {
                SubjectID = "Test Subject 2",
                Description = "Test Description 2",
                Active = true
            });
        }



        public List<Subject> SelectSubjects()
        {
            return _subjects;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessFakes;
using DataAccessLayer;

namespace LogicLayer
{
    public class SubjectManager : ISubjectManager
    {
        ISubjectAccessor _subjectAccessor = null;


        public SubjectManager()
        {
            _subjectAccessor = new SubjectAccessor();
        }

        public SubjectManager(ISubjectAccessor subjectAccessor)
        {
            _subjectAccessor = subjectAccessor;
        }

        public List<Subject> RetrieveSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            //subjects.Add(new Subject());
            //subjects.Add(new Subject());

            try
            {
                subjects = _subjectAccessor.SelectSubjects();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            if (subjects == null)
            {
                throw new ApplicationException("No subjects retrieved.");
            }

            return subjects;
        }


    }
}

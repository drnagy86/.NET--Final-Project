using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterFaces;
using DataObjects;

namespace DataAccessFakes
{
    public class RubricSubjectAccessorFake : IRubricSubjectAccessor
    {
        List<RubricSubject> _rubricSubjects = new List<RubricSubject>();

        public RubricSubjectAccessorFake()
        {
            _rubricSubjects.Add(new RubricSubject()
            {
                SubjectID = "Test Subject 1",
                RubricID = 100000,                
                Active = true
            });

            _rubricSubjects.Add(new RubricSubject()
            {
                SubjectID = "Test Subject 2",
                RubricID = 100000,
                Active = true
            });

            _rubricSubjects.Add(new RubricSubject()
            {
                SubjectID = "Test Subject 1",
                RubricID = 100001,
                Active = true
            });

            _rubricSubjects.Add(new RubricSubject()
            {
                SubjectID = "Test Subject 2",
                RubricID = 100001,
                Active = true
            });
        }

        public int DeleteRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID)
        {
            int rowsAffected = 0;

            foreach (var item in _rubricSubjects)
            {
                if (item.RubricID == rubricID && item.SubjectID == subjectID)
                {
                    _rubricSubjects.Remove(item);
                    rowsAffected++;
                    break;
                }
            }

            return rowsAffected;
        }

        public int InsertRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID, string description)
        {
            int rowsAffected = 0;
            _rubricSubjects.Add(new RubricSubject() {
                SubjectID = subjectID,
                RubricID = rubricID                
            }
            );

            rowsAffected++;

            return rowsAffected;

        }

        public List<RubricSubject> SelectRubricSubjectsByRubricID(int rubricID)
        {
            return _rubricSubjects.FindAll(rs => rs.RubricID == rubricID);
        }
    }
}

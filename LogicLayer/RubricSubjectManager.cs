using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterFaces;
using DataObjects;
using DataAccessFakes;
using DataAccessLayer;

namespace LogicLayer
{
    public class RubricSubjectManager : IRubricSubjectManager
    {
        IRubricSubjectAccessor _rubricSubjectAccessor = null;


        public RubricSubjectManager()
        {
            _rubricSubjectAccessor = new RubricSubjectAccessor();
        }

        public RubricSubjectManager(IRubricSubjectAccessor rubricSubjectAccessor)
        {
            _rubricSubjectAccessor = rubricSubjectAccessor;
        }

        public bool CreateRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID, string description)
        {
            bool result = false;

            try
            {
                result = (1 == _rubricSubjectAccessor.InsertRubricSubjectBySubjectIDAndRubricID(subjectID, rubricID, description));
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public bool RemoveRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID)
        {
            bool result = false;

            try
            {
                result = (1 == _rubricSubjectAccessor.DeleteRubricSubjectBySubjectIDAndRubricID(subjectID, rubricID));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!result)
            {
                throw new ApplicationException("Problem deleting the rubric.");
            }

            return result;
        }

        public List<RubricSubject> RetrieveRubricSubjectsByRubricID(int rubricID)
        {
            List<RubricSubject> rubricSubjects = new List<RubricSubject>();

            //rubricSubjects.Add(new RubricSubject());
            //rubricSubjects.Add(new RubricSubject());

            try
            {
                rubricSubjects = _rubricSubjectAccessor.SelectRubricSubjectsByRubricID(rubricID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rubricSubjects;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace DataAccessInterFaces
{
    public interface IRubricSubjectAccessor
    {
        List<RubricSubject> SelectRubricSubjectsByRubricID(int rubricID);
        int InsertRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID, string description);
        int DeleteRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID);
    }
}

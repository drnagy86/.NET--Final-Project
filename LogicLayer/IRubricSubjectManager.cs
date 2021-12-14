using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IRubricSubjectManager
    {
        List<RubricSubject> RetrieveRubricSubjectsByRubricID(int rubricID);
        bool CreateRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID, string description);
        bool RemoveRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID);
    }
}

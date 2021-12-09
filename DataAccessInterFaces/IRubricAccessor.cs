using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessInterFaces
{
    public interface IRubricAccessor
    {
        Rubric SelectRubricByRubricID(int rubricID);
        List<Rubric> SelectRubrics();
        int InsertRubric(string name, string description, string scoreType, string rubricCreator);
        Rubric SelectRubricByRubricDetials(string name, string description, string scoreType, string rubricCreator);

        int UpdateRubricByRubricID(int rubricID, string oldName, string newName, string oldDescription, string newDescription, string oldScoreType, string newScoreType);

        int DeactivateRubricByRubricID(int rubricID);

        int DeleteRubricByRubricID(int rubricID);

    }
}

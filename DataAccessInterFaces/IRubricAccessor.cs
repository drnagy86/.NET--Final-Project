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
    }
}

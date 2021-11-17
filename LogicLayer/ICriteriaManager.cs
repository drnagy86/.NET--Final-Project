using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace LogicLayer
{
    public interface ICriteriaManager
    {
        List<Criteria> RetrieveCriteriasForRubricByRubricID(int rubricID);


    }
}

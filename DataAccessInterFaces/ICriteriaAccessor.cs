using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessInterFaces
{
    public interface ICriteriaAccessor
    {
        List<Criteria> SelectCriteriaByRubricID(int rubricID);
    }
}

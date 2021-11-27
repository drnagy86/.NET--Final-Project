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

        int UpdateCriteriaByCriteriaID(int rubricID, string facetID, string oldCriteriaID, string newCriteriaID, string oldContent, string newContent);
        int UpdateCriteriaContentByCriteriaID(int rubricID, string facetID, string criteriaID, string oldContent, string newContent);
    }
}

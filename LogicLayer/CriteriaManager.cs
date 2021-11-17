using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessLayer;

namespace LogicLayer
{
    public class CriteriaManager : ICriteriaManager
    {
        private ICriteriaAccessor _criteriaAccessor = null;

        public CriteriaManager()
        {
            _criteriaAccessor = new CriteriaAccessor();
        }

        public CriteriaManager(ICriteriaAccessor criteriaAccessor)
        {
            _criteriaAccessor = criteriaAccessor;
        }

        public List<Criteria> RetrieveCriteriasForRubricByRubricID(int rubricID)
        {
            List<Criteria> criteriaList = new List<Criteria>();
            try
            {
                criteriaList = _criteriaAccessor.SelectCriteriaByRubricID(rubricID);
            }
            catch (Exception ex)
            {
                throw;
            }
            return criteriaList;
        }
    }
}

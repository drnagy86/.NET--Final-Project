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



        // Update helper suite
        int UpdateCriteriaByCriteriaID(int rubricID, string facetID, string oldCriteriaID, string newCriteriaID, string oldContent, string newContent);
        Dictionary<Criteria, Criteria> GetDictionaryOfDifferentCriteria(Dictionary<Facet, List<Criteria>> oldFacetCriteria, Dictionary<Facet, List<Criteria>> newFacetCriteria);
        bool UpdateMultipleCriteriaByCriteriaDictionary(Dictionary<Criteria, Criteria> oldKeyNewValueDictionary);
        bool UpdateSingleCriteriaByCriteria(Criteria oldCriteria, Criteria newCriteria);
        int UpdateCriteriaContentByCriteriaID(int rubricID, string facetID, string criteriaID, string oldContent, string newContent);



        // call all update methods
        bool UpdateCriteriaByCriteriaFacetDictionary(Dictionary<Facet, List<Criteria>> oldFacetCriteria, Dictionary<Facet, List<Criteria>> newFacetCriteria);

    }
}

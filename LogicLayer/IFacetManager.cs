using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IFacetManager
    {
        //Rubric RetrieveRubricByRubricID(int rubricID);
        //List<Rubric> RetrieveActiveRubrics();
        List<Facet> RetrieveFacetsByRubricID(int rubricID);
        List<Facet> RetrieveActiveFacets();
        bool CreateFacet(int rubricID, string facetID, string description, string facetType);
        bool UpdateFacetDescriptionByRubricIDAndFacetID(int rubricID, string facetID, string oldDescription, string newDescription);

    }
}

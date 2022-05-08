using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessInterFaces
{
    public interface IFacetAccesor
    {
        List<Facet> SelectFacets();
        List<Facet> SelectFacetsByRubricID(int rubricID);
        int InsertFacet(int rubricID, string facetID, string description, string facetType);
        int UpdateFacetDescriptionByRubricIDAndFacetID(int rubricID, string facetID, string oldDescription, string newDescription);
        int DeleteFacetByRubricIDAndFacetID(int rubricID, string facetID);

        int UpdateFacetAndCriteraWithFacetVM(FacetVM oldFacet, FacetVM newFacet);

        FacetVM SelectFacetVM(int rubricID, string facetID);

    }
}

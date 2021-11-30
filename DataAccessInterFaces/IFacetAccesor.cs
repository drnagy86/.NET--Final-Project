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
    }
}

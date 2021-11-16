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
        Facet RetrieveFacetByFacetID(int facetID);
        List<Facet> RetrieveActiveFacets();


    }
}

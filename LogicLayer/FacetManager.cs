using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessLayer;
using DataAccessFakes;

namespace LogicLayer
{
    public class FacetManager : IFacetManager
    {
        private IFacetAccesor _facetAccesor = null;

        public FacetManager()
        {
            _facetAccesor = new FacetAccessor();
        }

        public FacetManager(IFacetAccesor facetAccessor)
        {
            _facetAccesor = facetAccessor;
        }

        public List<Facet> RetrieveActiveFacets()
        {
            List<Facet> facets = null;

            try
            {
                facets = _facetAccesor.SelectFacets();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return facets;
        }

        public List<Facet> RetrieveFacetsByRubricID(int rubricID)
        {
            List<Facet> facets = null;

            try
            {
                facets = _facetAccesor.SelectFacetsByRubricID(rubricID);
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return facets;

        }
    }
}

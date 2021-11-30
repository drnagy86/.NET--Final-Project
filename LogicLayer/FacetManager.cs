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

        public bool CreateFacet(int rubricID, string facetID, string description, string facetType)
        {
            bool isCreated = false;
            int rowsAffected = 0;

            if (facetID == null || facetID == "")
            {
                throw new ApplicationException("The name of the facet can not be empty.");
            }
            if (description == null || description == "")
            {
                throw new ApplicationException("The description of the facet can not be empty.");
            }
            if (facetType == null || facetType == "")
            {
                throw new ApplicationException("The facet type can not be empty.");
            }

            try
            {
                rowsAffected = _facetAccesor.InsertFacet(rubricID, facetID, description, facetType);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (rowsAffected == 1)
            {
                isCreated = true;
            }

            return isCreated;
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

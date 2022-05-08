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

        public bool DeleteFacetByRubricIDAndFacetID(int rubricID, string facetID)
        {
            bool result = false;

            try
            {
                result = (1 == _facetAccesor.DeleteFacetByRubricIDAndFacetID(rubricID,facetID));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!result)
            {
                throw new ApplicationException("Facet not deleted.");
            }

            return result;
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

        public FacetVM RetrieveFacetVM(int rubricID, string facetID)
        {
            FacetVM facet = null;

            // green
            //facet = new FacetVM()
            //{
            //    FacetID = "Fake Facet 1",
            //    Description = "A longer description",
            //    DateCreated = DateTime.Now,
            //    DateUpdated = DateTime.Now,
            //    Active = true,
            //    RubricID = 100000,
            //    FacetType = "Type1",
            //    Criteria = new List<Criteria>()
            //};
            //facet.Criteria.Add(new Criteria());
            //facet.Criteria.Add(new Criteria());
            //facet.Criteria.Add(new Criteria());

            try
            {
                facet = _facetAccesor.SelectFacetVM(rubricID, facetID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return facet;
        }

        public bool UpdateFacetAndCriteria(FacetVM oldFacet, FacetVM newFacet)
        {
            bool result = false;

            try
            {
                result = _facetAccesor.UpdateFacetAndCriteraWithFacetVM(oldFacet, newFacet) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!result)
            {
                throw new ApplicationException("Could not find the facet");
            }

            return result;
        }

        public bool UpdateFacetDescriptionByRubricIDAndFacetID(int rubricID, string facetID, string oldDescription, string newDescription)
        {
            bool result = false;

            try
            {
                result = (1 ==_facetAccesor.UpdateFacetDescriptionByRubricIDAndFacetID(rubricID, facetID, oldDescription, newDescription));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!result)
            {
                throw new ApplicationException("Facet not found ");
            }

            return result;
        }
    }
}

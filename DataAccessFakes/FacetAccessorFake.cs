using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class FacetAccessorFake: IFacetAccesor
    {
        private List<Facet> _fakeFacetList = new List<Facet>();

        public FacetAccessorFake()
        {
            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 1",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100000,
                FacetType = "Type1"
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 2",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100000,
                FacetType = "Type2"
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 3",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100000,
                FacetType = "Type3"
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 3",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100001,
                FacetType = "Type1"
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 4",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100001,
                FacetType = "Type2"
            });
        }

        public int InsertFacet(int rubricID, string facetID, string description, string facetType)
        {
            int rowsAffected = 0;

            int fakeFacetCountBeforeAdd = _fakeFacetList.Count;

            _fakeFacetList.Add(new Facet()
            {
                RubricID = rubricID,
                FacetID = facetID,
                Description = description,
                FacetType = facetID,

            });

            rowsAffected = _fakeFacetList.Count - fakeFacetCountBeforeAdd;

            return rowsAffected;
        }

        public List<Facet> SelectFacets()
        {
            return _fakeFacetList;
        }

        public List<Facet> SelectFacetsByRubricID(int rubricID)
        {

            var facetByID = _fakeFacetList.FindAll(f => f.RubricID == rubricID);

            return facetByID;

        }
    }
}

﻿using System;
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
                RubricID = 100000
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 2",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100000
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 3",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100000
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 3",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100001
            });

            _fakeFacetList.Add(new Facet()
            {
                FacetID = "Fake Facet 4",
                Description = "A longer description",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 100001
            });
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
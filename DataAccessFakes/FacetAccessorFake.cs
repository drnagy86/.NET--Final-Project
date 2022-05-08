using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class FacetAccessorFake : IFacetAccesor
    {
        private List<Facet> _fakeFacetList = new List<Facet>();
        private List<FacetVM> _fakeFacetVMList = new List<FacetVM>();
        private List<Criteria> _criteriaList = new List<Criteria>();

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

            setupFakeCriteria();

            setupFakeFacetVMList();


        }

        public int DeleteFacetByRubricIDAndFacetID(int rubricID, string facetID)
        {
            int rowsAffected = 0;

            foreach (Facet facet in _fakeFacetList)
            {
                if (facet.FacetID == facetID && facet.RubricID == rubricID)
                {
                    _fakeFacetList.Remove(facet);
                    rowsAffected++;
                    return rowsAffected;

                }
            }


            return rowsAffected;
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

        public FacetVM SelectFacetVM(int rubricID, string facetID)
        {
            return _fakeFacetVMList.Find(f => f.RubricID == rubricID && f.FacetID == facetID);         
        }

        public int UpdateFacetDescriptionByRubricIDAndFacetID(int rubricID, string facetID, string oldDescription, string newDescription)
        {
            int rowsAffected = 0;

            foreach (Facet facet in _fakeFacetList)
            {
                if (facet.RubricID == rubricID && facet.FacetID == facetID && facet.Description == oldDescription)
                {
                    facet.Description = newDescription;
                    rowsAffected++;
                    break;
                }
            }



            return rowsAffected;

        }

        private void setupFakeFacetVMList()
        {
            foreach (Facet facet in _fakeFacetList)
            {
                _fakeFacetVMList.Add(new FacetVM(facet, new List<Criteria>()));
            }

            foreach (Criteria criteria in _criteriaList)
            {
                foreach (FacetVM facet in _fakeFacetVMList)
                {
                    if (criteria.RubricID == facet.RubricID)
                    {
                        facet.Criteria.Add(criteria);
                    }
                }
            }

        }

        private void setupFakeCriteria()
        {
            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });
        }

        public int UpdateFacetAndCriteraWithFacetVM(FacetVM oldFacet, FacetVM newFacet)
        {
            int rowsAffected = 0;

            int facetIndex = _fakeFacetVMList.FindIndex(f => f.RubricID == oldFacet.RubricID && f.FacetID == oldFacet.FacetID && f.Description == oldFacet.Description && f.FacetType == oldFacet.FacetType);

            if (facetIndex != -1)
            {
                _fakeFacetVMList[facetIndex].FacetID = newFacet.FacetID;
                _fakeFacetVMList[facetIndex].Description = newFacet.Description;
                _fakeFacetVMList[facetIndex].FacetType = newFacet.FacetType;

                // simulates adding to facet table
                rowsAffected++;

                _fakeFacetVMList[facetIndex].Criteria = newFacet.Criteria;

                // simulates adding to criteria table
                rowsAffected++;
            }

            return rowsAffected;
        }
    }
}

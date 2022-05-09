using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataObjects;

namespace RubricMVC.Models
{
    public class FacetModelView : FacetVM
    {

        public Rubric Rubric { get; set; }
        public List<FacetType> FacetTypeList { get; set; }
        public string OldFacetID { get; set; }

        public bool IsCreating { get; set; } = false ;

        public FacetModelView()
        {

        }

        public FacetModelView(Rubric rubric, List<FacetType> facetTypes)
        {
            this.Rubric = rubric;
            this.RubricID = rubric.RubricID;
            FacetTypeList = facetTypes;

            this.FacetID = "";
            this.Description = "";
            this.DateCreated = DateTime.Now;
            this.Active = true;
            this.FacetType = facetTypes[0].FacetTypeID;

            this.Criteria = new List<Criteria>();

            createBlankCriteria();
        }

        public FacetModelView(FacetVM facet)
        {
            this.FacetID = facet.FacetID;
            this.Description = facet.Description;
            this.DateCreated = facet.DateCreated;
            this.Active = facet.Active;
            this.RubricID = facet.RubricID;
            this.FacetType = facet.FacetType;

            this.Criteria = facet.Criteria;
            this.OldFacetID = facet.FacetID;

        }

        private void createBlankCriteria()
        {
            for (int i = 1; i < this.Rubric.NumberOfCriteria + 1; i++)
            {
                this.Criteria.Add(new Criteria()
                {
                    CriteriaID = "Criteria " + i,
                    RubricID = 0,
                    FacetID = this.FacetID,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Content = "Describe the criteria that earns the score",
                    Score = i,
                    Active = true
                });
            }
        }

    }
}
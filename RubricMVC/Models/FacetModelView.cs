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

        public FacetModelView()
        {

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

    }
}
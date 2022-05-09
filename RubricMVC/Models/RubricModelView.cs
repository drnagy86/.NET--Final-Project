using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataObjects;

namespace RubricMVC.Models
{
    public class RubricModelView : RubricVM
    {
        public List<ScoreType> ScoreTypes { get; set; }
        public bool CanEdit { get; set; } = false;

        public RubricModelView() { }

        public RubricModelView(RubricVM rubric, List<ScoreType> scoreTypes)
        {
            // rubric
            this.RubricID = rubric.RubricID;
            this.Name = rubric.Name;
            this.Description = rubric.Description;
            this.DateCreated = rubric.DateCreated;
            this.DateUpdated = rubric.DateUpdated;
            this.ScoreTypeID = rubric.ScoreTypeID;
            this.RubricCreator = rubric.RubricCreator;
            this.Active = rubric.Active;

            // rubric vm
            this.Facets = rubric.Facets;
            this.Criteria = rubric.Criteria;
            this.FacetCriteria = rubric.FacetCriteria;

            this.ScoreTypes = scoreTypes;
        }
    }
}
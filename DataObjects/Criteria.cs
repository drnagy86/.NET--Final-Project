using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Criteria
    {
        public string CriteriaID { get; set; }
        public int RubricID { get; set; }
        public string FacetID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Content { get; set; }
        public int Score { get; set; }
        public bool Active { get; set; }


        public Criteria()
        {
            this.CriteriaID = "";
            this.RubricID = 0;
            this.FacetID = "";
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Content = "";
            this.Score = 1;
            this.Active = true;
        }

        public Criteria(string facetID, int score)
        {
            this.CriteriaID = " ";
            this.RubricID = 0;
            this.FacetID = facetID;
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Content = " ";
            this.Score = score;
            this.Active = true;
        }

        public Criteria(int rubricID, string facetID)
        {
            this.CriteriaID = "";
            this.RubricID = rubricID;
            this.FacetID = facetID;
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Content = "";
            this.Score = 1;
            this.Active = true;
        }

        public Criteria(int rubricID, string facetID, int score)
        {
            this.CriteriaID = "";
            this.RubricID = rubricID;
            this.FacetID = facetID;
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Content = "";
            this.Score = score;
            this.Active = true;
        }

    }


}

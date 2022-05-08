using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Criteria
    {
        [Display(Name = "Criteria Name")]
        [Required(ErrorMessage = "Please enter a name for the Crtieria")]
        [StringLength(50, ErrorMessage = "The name can not be longer than 50 characters")]
        public string CriteriaID { get; set; }

        public int RubricID { get; set; }
        public string FacetID { get; set; }


        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required(ErrorMessage = "Please describe what the criteria is evaluating")]
        [StringLength(255, ErrorMessage = "The name can not be longer than 100 characters")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please enter a value that represents points possible")]
        [Range(0,Int32.MaxValue)]
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

        public Criteria(string facetID, int score, string criteriaID, string content)
        {
            this.CriteriaID = criteriaID;
            this.RubricID = 0;
            this.FacetID = facetID;
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Content = content;
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

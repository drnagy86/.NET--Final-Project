
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Facet
    {
        [Display(Name = "Facet Name")]
        [Required(ErrorMessage = "Please enter a name for the Facet")]
        [StringLength(100, ErrorMessage = "The name can not be longer than 100 characters")]
        public string FacetID { get; set; }

        [Required(ErrorMessage = "Please enter a description for the rubric")]
        [StringLength(255, ErrorMessage = "The name can not be longer than 255 characters")]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool Active { get; set; }
        public int RubricID { get; set; }

        [Display(Name = "Facet Type")]
        public string FacetType { get; set; }

        public Facet()
        {
            FacetID = "";
            Description = "";
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
            Active = true;
            RubricID = 0;
            FacetType = "";
        }

        public Facet(int rubricID)
        {
            FacetID = "";
            Description = "";
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
            Active = true;
            RubricID = rubricID;
            FacetType = "";
        }

    }

    public class FacetVM : Facet
    {
        public List<Criteria> Criteria { get; set; }

        public FacetVM()
        {
            Facet facet = new Facet();
            List<Criteria> criteria = new List<Criteria>();


            this.FacetID = facet.FacetID;
            this.Description = facet.Description;
            this.DateCreated = facet.DateCreated;
            this.Active = facet.Active;
            this.RubricID = facet.RubricID;
            this.FacetType = facet.FacetType;

            this.Criteria = criteria;
        }

        public FacetVM(Facet facet, List<Criteria> criteria)
        {
            this.FacetID = facet.FacetID;
            this.Description = facet.Description;
            this.DateCreated = facet.DateCreated;
            this.Active = facet.Active;
            this.RubricID = facet.RubricID;
            this.FacetType = facet.FacetType;

            this.Criteria = criteria;


        }
    }
}

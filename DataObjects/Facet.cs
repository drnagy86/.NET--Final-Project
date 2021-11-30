
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Facet
    {
        public string FacetID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool Active { get; set; }
        public int RubricID { get; set; }
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
}

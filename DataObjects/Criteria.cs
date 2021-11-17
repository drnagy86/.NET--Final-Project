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
    }
}

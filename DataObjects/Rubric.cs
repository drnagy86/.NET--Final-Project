using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Rubric
    {
        public int RubricID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public String ScoreTypeID { get; set; }
        public User RubricCreator { get; set; }
        public bool Active { get; set; }

    }
}

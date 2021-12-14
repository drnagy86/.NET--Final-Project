using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class RubricSubject
    {
        public string SubjectID { get; set; }
        public int RubricID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

    }
}

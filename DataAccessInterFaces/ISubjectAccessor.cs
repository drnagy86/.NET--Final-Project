using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessInterFaces
{
    public interface ISubjectAccessor
    {
        List<Subject> SelectSubjects();

    }
}

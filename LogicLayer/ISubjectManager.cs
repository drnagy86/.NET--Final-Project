using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessFakes;
using DataAccessLayer;

namespace LogicLayer
{
    public interface ISubjectManager
    {
        List<Subject> RetrieveSubjects();


    }
}

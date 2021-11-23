
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace LogicLayer
{
    public interface IRubricManager<T> where T:Rubric
    {
        T RetrieveRubricByRubricID(int rubricID);
        List<T> RetrieveActiveRubrics();
    }
}

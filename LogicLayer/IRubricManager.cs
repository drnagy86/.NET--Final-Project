
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
        bool CreateRubric(string name, string description, string scoreTypeID, string rubricCreator);

        T RetrieveRubricByNameDescriptionScoreTypeIDRubricCreator(string name, string description, string scoreTypeID, string rubricCreator);

        bool DeactivateRubricByRubricID(int rubricID);

        bool DeleteRubricByRubricID(int rubricID);

        bool UpdateRubricByRubricID(int rubricID, string oldName, string newName, string oldDescription, string newDescription, string oldScoreType, string newScoreType);



        int CreateRubric(RubricVM rubricVM);

    }
}

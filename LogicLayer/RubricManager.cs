using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessLayer;

namespace LogicLayer
{
    public class RubricManager : IRubricManager
    {
        private IRubricAccessor _rubricAccessor = null;

        public RubricManager()
        {
            _rubricAccessor = new RubricAccessor();
        }

        public RubricManager(IRubricAccessor rubricAccessor)
        {
            _rubricAccessor = rubricAccessor;
        }

        public List<Rubric> RetrieveActiveRubrics()
        {
            List<Rubric> rubricList = null;
            try
            {
                rubricList = _rubricAccessor.SelectRubrics();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rubricList;
        }

        public Rubric RetrieveRubricByRubricID(int rubricID)
        {
            Rubric rubric = null;

            try
            {
                rubric = _rubricAccessor.SelectRubricByRubricID(rubricID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rubric;
        }


    }
}

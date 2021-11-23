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
    public class RubricManager : IRubricManager<Rubric>
    {
        private IRubricAccessor _rubricAccessor = null;
        private IUserAccessor _userAccessor = null;

        public RubricManager()
        {
            _rubricAccessor = new RubricAccessor();
            _userAccessor = new UserAccessor();
        }

        public RubricManager(IRubricAccessor rubricAccessor, IUserAccessor userAccessor)
        {
            _rubricAccessor = rubricAccessor;
            _userAccessor = userAccessor;
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
            User rubricCreator = null;

            try
            {
                rubric = _rubricAccessor.SelectRubricByRubricID(rubricID);
                // need to get the user information
                rubricCreator = _userAccessor.SelectUserByUserID(rubric.RubricCreator.UserID);
                rubric.RubricCreator = rubricCreator;                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rubric;
        }


    }
}

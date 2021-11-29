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

        public bool CreateRubric(string name, string description, string scoreTypeID, string rubricCreator)
        {
            bool isCreated = false;
            int rowsAffected = 0;

            if (name == "" || name == null)
            {
                throw new ApplicationException("Name can not be empty.");
            }

            if (description == "" || description == null)
            {
                throw new ApplicationException("Description can not be empty.");
            }

            if (scoreTypeID == "" || scoreTypeID == null)
            {
                throw new ApplicationException("The score type can not be empty.");
            }

            try
            {
                rowsAffected = _rubricAccessor.InsertRubric(name, description, scoreTypeID, rubricCreator);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (rowsAffected == 1)
            {
                isCreated = true;
            }

            return isCreated;
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

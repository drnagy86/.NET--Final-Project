using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterFaces;
using DataAccessLayer;


namespace LogicLayer
{
    public class RubricVMManager : IRubricManager<RubricVM>
    {
        private IRubricManager<Rubric> _rubricManager = null;
        private IUserManager _userManager = null;
        private IFacetManager _facetManager = null;
        private ICriteriaManager _criteriaManager = null;

        public RubricVMManager()
        {
            _rubricManager = new RubricManager();
            _userManager = new UserManager();
            _facetManager = new FacetManager();
            _criteriaManager = new CriteriaManager();            
        }

        public RubricVMManager(IRubricManager<Rubric> rubricManager, IUserManager userManager, IFacetManager facetManager, ICriteriaManager criteriaManager)
        {
            _rubricManager = rubricManager;
            _userManager = userManager;
            _facetManager = facetManager;
            _criteriaManager = criteriaManager;
        }

        public bool CreateRubric(string name, string description, string scoreTypeID, string rubricCreator)
        {
            bool isCreated = false;

            try
            {
                _rubricManager.CreateRubric(name, description, scoreTypeID, rubricCreator);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return isCreated;

        }

        public List<RubricVM> RetrieveActiveRubrics()
        {
            throw new NotImplementedException();
        }

        public RubricVM RetrieveRubricByNameDescriptionScoreTypeIDRubricCreator(string name, string description, string scoreTypeID, string rubricCreator)
        {
            Rubric rubric = null;
            List<Facet> facetList = null;
            List<Criteria> criteriaList = null;

            try
            {
                rubric = _rubricManager.RetrieveRubricByNameDescriptionScoreTypeIDRubricCreator(name, description, scoreTypeID, rubricCreator);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                facetList = _facetManager.RetrieveFacetsByRubricID(rubric.RubricID);
            }
            catch (Exception)
            {
                facetList = new List<Facet>();
                facetList.Add(new Facet());
            }

            try
            {
                criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(rubric.RubricID);
            }
            catch (Exception)
            {
                criteriaList = new List<Criteria>();
                criteriaList.Add(new Criteria(rubric.RubricID,""));
            }

            RubricVM rubricVM = new RubricVM(rubric, facetList, criteriaList);
            return rubricVM;
        }

        public RubricVM RetrieveRubricByRubricID(int rubricID)
        {
            Rubric rubric = null;
            List<Facet> facetList = null;
            List<Criteria> criteriaList = null;

            try
            {
                rubric = _rubricManager.RetrieveRubricByRubricID(rubricID);                
                rubric.RubricCreator = _userManager.GetUserByUserID(rubric.RubricCreator.UserID);

                facetList = _facetManager.RetrieveFacetsByRubricID(rubricID);
                criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(rubricID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was a problem creating the rubric view.\n" + ex.Message);
            }
            RubricVM rubricVM = new RubricVM(rubric, facetList, criteriaList);
            return rubricVM;

            // Green
            //Rubric rubric = new Rubric();
            //List<Facet> facetList = new List<Facet>();
            //List<Criteria> criteriaList = new List<Criteria>();

            //RubricVM rubricVM = new RubricVM(rubric, facetList, criteriaList);
            //rubricVM.RubricID = 100000;
        }

        public List<Criteria> CreateBlankCriteriaForCreateRubricVM(int rubricID, string facetID, double bottomScore, double topScore)
        {
            List<Criteria> criteriaList = new List<Criteria>();

            int score = (int)topScore;

            for (int i = 0; i < (int)(topScore - bottomScore) + 1; i++)
            {
                Criteria criteria = new Criteria(rubricID, facetID, score--);
                criteriaList.Add(criteria);
            }
            return criteriaList;
        }

        public bool UpdateRubricByRubricID(int rubricID, string oldName, string newName, string oldDescription, string newDescription, string oldScoreType, string newScoreType)
        {
            return _rubricManager.UpdateRubricByRubricID(rubricID, oldName, newName, oldDescription, newDescription, oldScoreType, newScoreType);
        }

        public bool DeactivateRubricByRubricID(int rubricID)
        {
            return _rubricManager.DeactivateRubricByRubricID(rubricID);
        }

        public bool DeleteRubricByRubricID(int rubricID)
        {
            return _rubricManager.DeleteRubricByRubricID(rubricID);
        }
    }
}

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
        private IRubricAccessor _rubricAccessor = null;

        public RubricVMManager()
        {
            _rubricManager = new RubricManager();
            _userManager = new UserManager();
            _facetManager = new FacetManager();
            _criteriaManager = new CriteriaManager();
            _rubricAccessor = new RubricAccessor();
        }

        public RubricVMManager(IRubricManager<Rubric> rubricManager, IUserManager userManager, IFacetManager facetManager, ICriteriaManager criteriaManager, IRubricAccessor rubricAccessor)
        {
            _rubricManager = rubricManager;
            _userManager = userManager;
            _facetManager = facetManager;
            _criteriaManager = criteriaManager;
            _rubricAccessor = rubricAccessor;
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
            List<RubricVM> rubrics = new List<RubricVM>();

            try
            {
                rubrics = _rubricAccessor.RetrieveActiveRubricsVMs();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rubrics;


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

        public int CreateRubric(RubricVM rubricVM)
        {
            int rubricIDToReturn = 0;

            if (rubricVM.Name == null || rubricVM.Name == "")
            {
                throw new ApplicationException("Rubric name can not be empty");
            }

            if (rubricVM.Name.Length >= 100)
            {
                throw new ApplicationException("Rubric name can not be over 100 characters");
            }

            if (rubricVM.Description == null || rubricVM.Description == "")
            {
                throw new ApplicationException("Rubric description can not be empty");
            }

            if (rubricVM.Description.Length >= 255)
            {
                throw new ApplicationException("Rubric description can not be over 255 characters");
            }

            if (rubricVM.ScoreTypeID == null || rubricVM.ScoreTypeID == "")
            {
                throw new ApplicationException("Rubric scoretype can not be empty");
            }

            if (rubricVM.ScoreTypeID.Length >= 50)
            {
                throw new ApplicationException("Rubric scoretype can not be over 255 characters");
            }

            if (rubricVM.RubricCreator.UserID == null || rubricVM.RubricCreator.UserID == "")
            {
                throw new ApplicationException("Rubric needs a creator");
            }

            if (rubricVM.FacetVMs.Count == 0)
            {
                throw new ApplicationException("Rubric needs a creator");
            }

            foreach (FacetVM facet in rubricVM.FacetVMs)
            {
                if (facet.Criteria.Count == 0)
                {
                    throw new ApplicationException("Each facet needs at least one criteria");
                }
            }

            try
            {
                rubricIDToReturn = _rubricAccessor.InsertRubric(rubricVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return rubricIDToReturn;
        }
    }
}

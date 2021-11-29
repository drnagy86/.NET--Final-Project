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
            throw new NotImplementedException();
        }

        public List<RubricVM> RetrieveActiveRubrics()
        {
            throw new NotImplementedException();
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

    }
}

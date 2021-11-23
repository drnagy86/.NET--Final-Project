using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LogicLayer;
using DataObjects;
using DataAccessFakes;


namespace LogicLayerTests
{
    [TestClass]
    public class RubricVMManagerTests
    {
        IRubricManager<RubricVM> _rubricManager = null;

        [TestInitialize]
        public void TestSetup()
        {
            UserAccessorFake userAccessorFake = new UserAccessorFake();            

            RubricManager rubricManager = new RubricManager(new RubricAccessorFake(), userAccessorFake);
            UserManager userManager = new UserManager(userAccessorFake);
            FacetManager facetManager = new FacetManager(new FacetAccessorFake());
            CriteriaManager criteriaManager = new CriteriaManager(new CriteriaAccessorFake());

            _rubricManager = new RubricVMManager(rubricManager, userManager, facetManager, criteriaManager);
        }


        [TestMethod]
        public void TestRetrieveRubricByRubricIDReturnsCorrectRubric()
        {
            // arrange
            const int expectedRubricID = 100000;
            RubricVM actualRubricVM = null;
            int actualRubicID;

            // act
            actualRubricVM = _rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            actualRubicID = actualRubricVM.RubricID;

            //assert
            Assert.AreEqual(expectedRubricID, actualRubicID);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveRubricByRubricIDThrowsApplicationExceptionForBadRubricID()
        {
            // arrange
            const int expectedRubricID = 10000000;
            
            // act
            _rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            // no need to assert, expecting exception
        }
    }
}

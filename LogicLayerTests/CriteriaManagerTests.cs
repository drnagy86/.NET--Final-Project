using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataObjects;
using LogicLayer;
using DataAccessInterFaces;
using DataAccessFakes;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class CriteriaManagerTests
    {
        private ICriteriaManager _criteriaManager = null;

        [TestInitialize]
        public void TestSetup()
        {
            _criteriaManager = new CriteriaManager(new CriteriaAccessorFake());
        }        

        [TestMethod]
        public void TestRetrieveCriteriaForARubricReturnsCorrectList()
        {
            // arrange
            const int rubricID = 100000;
            const int expectedCount = 4;
            int actualCount;

            //act
            actualCount = _criteriaManager.RetrieveCriteriasForRubricByRubricID(rubricID).Count;

            // assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestRetrieveCriteriaForAEmptyRubricReturnsEmptyList()
        {
            // arrange
            const int rubricID = 1000000;
            const int expectedCount = 0;
            int actualCount;

            //act
            actualCount = _criteriaManager.RetrieveCriteriasForRubricByRubricID(rubricID).Count;

            // assert
            Assert.AreEqual(expectedCount, actualCount);
        }


        // needs some work
        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void TestRetrieveCriteriaForBadRubricIDThrowsApplicationException()
        //{
        //    // arrange
        //    List<Criteria> criteriaList = null;
        //    const int rubricID = 10000000;

        //    // act
        //    criteriaList = _criteriaManager.RetrieveCriteriasForRubricByRubricID(rubricID);
        //    int throwException = criteriaList[0].RubricID;
        //    // assert
        //    // nothing to assert, exception testing
        //}
    }
}

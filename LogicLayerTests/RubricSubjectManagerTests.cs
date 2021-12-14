using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccessInterFaces;
using DataAccessFakes;
using LogicLayer;
using DataObjects;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class RubricSubjectManagerTests
    {

        IRubricSubjectManager _rubricSubjectManager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _rubricSubjectManager = new RubricSubjectManager(new RubricSubjectAccessorFake());
        }

        [TestMethod]
        public void TestRetriveSubjectsForARubric()
        {
            //arrange
            const int rubricID = 100000;
            const int expectedCount = 2;
            List<RubricSubject> actualSubjectList = null;
            int acutalCount;

            //act
            actualSubjectList = _rubricSubjectManager.RetrieveRubricSubjectsByRubricID(rubricID);
            acutalCount = actualSubjectList.Count;

            //assert
            Assert.AreEqual(expectedCount, acutalCount);

        }

        [TestMethod]
        public void TestCreateSubjectsAndAddRubricSubject()
        {
            //arrange
            const bool expected = true;
            const string subjectID = "Subject";
            const int rubricID = 100001;
            const string description = "Test";
            bool actual;

            //act
            actual = _rubricSubjectManager.CreateRubricSubjectBySubjectIDAndRubricID(subjectID, rubricID, description);
           

            //assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestDeleteRubricSubject()
        {
            //arrange
            const bool expected = true;
            const string subjectID = "Test Subject 1";
            const int rubricID = 100001;
            bool actual;

            //act
            actual = _rubricSubjectManager.RemoveRubricSubjectBySubjectIDAndRubricID(subjectID, rubricID);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDeleteRubricSubjectThrowApplicationExceptionWhenNoRubricSubjectToDelete()
        {
            //arrange
            const bool expected = true;
            const string subjectID = "Subject";
            const int rubricID = 100001;
            bool actual;

            //act
            actual = _rubricSubjectManager.RemoveRubricSubjectBySubjectIDAndRubricID(subjectID, rubricID);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}

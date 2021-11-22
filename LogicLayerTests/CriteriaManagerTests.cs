using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataObjects;
using LogicLayer;
using DataAccessInterFaces;
using DataAccessFakes;
using System.Collections.Generic;
using System.Linq;

namespace LogicLayerTests
{
    [TestClass]
    public class CriteriaManagerTests
    {
        private ICriteriaManager _criteriaManager = null;
        private Dictionary<Facet, List<Criteria>> _oldFacetCriteria = null;
        private List<Criteria> _oldCriteriaList = null;
        private Facet _oldFacet = null;
        private Dictionary<Facet, List<Criteria>> _newFacetCriteria = null;
        private List<Criteria> _newCriteriaList = null;
        private Facet _newFacet = null;
        private Dictionary<Criteria, Criteria> _oldKeyNewValue = null;

        [TestInitialize]
        public void TestSetup()
        {
            _criteriaManager = new CriteriaManager(new CriteriaAccessorFake());

            _oldFacetCriteria = new Dictionary<Facet, List<Criteria>>();

            _oldCriteriaList = new List<Criteria>();

            _oldCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _oldCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _oldCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _oldCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

            _oldFacet = new Facet() { FacetID = "Explaination" };
            _oldFacetCriteria.Add(_oldFacet, _oldCriteriaList);

            _newFacetCriteria = new Dictionary<Facet, List<Criteria>>();

            _newCriteriaList = new List<Criteria>();

            _newCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Changed CriteriaID",
                //CriteriaID = "Excellent",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "A change in the content",
                //Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _newCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _newCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _newCriteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

            _newFacet = new Facet() { FacetID = "Explaination" };
            _newFacetCriteria.Add(_newFacet, _newCriteriaList);


            _oldKeyNewValue = new Dictionary<Criteria, Criteria>();
            _oldKeyNewValue.Add(_oldCriteriaList[0], _newCriteriaList[0]);
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

        [TestMethod]
        public void TestUpdateCriteriaReturnsOneRowAffectedForRecordUpdate()
        {
            // arrange

            const int rubricID = 100000;
            const string facetID = "Explaination";
            const string oldCriteriaID = "Excellent";
            const string newCriteriaID = "Okay";
            const string oldContent = "Shows an excellent explaination";
            const string newContent = "Some updated content";
            const int expected = 1;
            int result;

            // act
            result = _criteriaManager.UpdateCriteriaByCriteriaID(rubricID, facetID, oldCriteriaID, newCriteriaID, oldContent, newContent);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateCriteriaReturnsZeroRowsAffectedForRecordUpdateThrowsException()
        {
            // arrange

            const int rubricID = 110011;
            const string facetID = "Explaination";
            const string oldCriteriaID = "Excellent";
            const string newCriteriaID = "Okay";
            const string oldContent = "Shows an excellent explaination";
            const string newContent = "Some updated content";

            // act, throws exception
            _criteriaManager.UpdateCriteriaByCriteriaID(rubricID, facetID, oldCriteriaID, newCriteriaID, oldContent, newContent);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateCriteriaReturnsThrowsApplicationExceptionForTooManyRowsAffected()
        {
            // arrange

            const int rubricID = 100001;
            const string facetID = "Explaination";
            const string oldCriteriaID = "Excellent";
            const string newCriteriaID = "Okay";
            const string oldContent = "Shows an excellent explaination";
            const string newContent = "Some updated content";

            // act
            _criteriaManager.UpdateCriteriaByCriteriaID(rubricID, facetID, oldCriteriaID, newCriteriaID, oldContent, newContent);

            // assert
            //nothing to assert, exception testing
        }

        [TestMethod]
        public void TestGetDictionaryOfDifferentCriteriaReturnsCorrectOldCriteriaID()
        {
            // arrange
            const string expectedOldCriteriaID = "Excellent";
            string actualOldCriteriaID = "";
            Dictionary<Criteria, Criteria> actual = null;

            // act
            actual = _criteriaManager.GetDictionaryOfDifferentCriteria(_oldFacetCriteria, _newFacetCriteria);
            actualOldCriteriaID = actual.ElementAt(0).Key.CriteriaID;

            // assert
            Assert.AreEqual(expectedOldCriteriaID, actualOldCriteriaID);

        }

        [TestMethod]
        public void TestGetDictionaryOfDifferentCriteriaReturnsCorrectNewCriteriaID()
        {
            // arrange
            const string expectedNewCriteriaID = "Changed CriteriaID";
            string actualNewCriteriaID = "";

            Dictionary<Criteria, Criteria> actual = null;

            // act
            actual = _criteriaManager.GetDictionaryOfDifferentCriteria(_oldFacetCriteria, _newFacetCriteria);

            actualNewCriteriaID = actual.ElementAt(0).Value.CriteriaID;

            // assert
            Assert.AreEqual(expectedNewCriteriaID, actualNewCriteriaID);
        }

        [TestMethod]
        public void TestGetDictionaryOfDifferentCriteriaReturnsCorrectOldContent()
        {
            // arrange
            const string expectedOldContent = "Shows an excellent explaination";
            string actualOldContent = "";
            Dictionary<Criteria, Criteria> actual = null;

            // act
            actual = _criteriaManager.GetDictionaryOfDifferentCriteria(_oldFacetCriteria, _newFacetCriteria);
            actualOldContent = actual.ElementAt(0).Key.Content;

            // assert
            Assert.AreEqual(expectedOldContent, actualOldContent);

        }

        [TestMethod]
        public void TestGetDictionaryOfDifferentCriteriaReturnsCorrectNewContent()
        {
            // arrange
            const string expectedNewContent = "A change in the content";
            string actualNewContent = "";

            Dictionary<Criteria, Criteria> actual = null;

            // act
            actual = _criteriaManager.GetDictionaryOfDifferentCriteria(_oldFacetCriteria, _newFacetCriteria);

            actualNewContent = actual.ElementAt(0).Value.Content;

            // assert
            Assert.AreEqual(expectedNewContent, actualNewContent);
        }

        [TestMethod]
        public void TestGetDictionaryOfDifferentCriteriaReturnsListWithOneCorrectItem()
        {
            // arrange
            const int expectedCount = 1;
            Dictionary<Criteria, Criteria> actual = null;
            int actualCount;

            // act
            actual = _criteriaManager.GetDictionaryOfDifferentCriteria(_oldFacetCriteria, _newFacetCriteria);
            actualCount = actual.Count;
            // assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestGetDictionaryOfDifferentCriteriaReturnsEmptyListWhenTheSame()
        {
            // arrange
            const int expectedCount = 0;
            Dictionary<Criteria, Criteria> actual = null;
            int actualCount;

            // act
            actual = _criteriaManager.GetDictionaryOfDifferentCriteria(_oldFacetCriteria, _oldFacetCriteria);
            actualCount = actual.Count;
            // assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestUpdateSingleCriteriaByCriteriaUpdatesRecord()
        {
            // assert
            Assert.IsTrue(_criteriaManager.UpdateSingleCriteriaByCriteria(_oldCriteriaList[0], _newCriteriaList[0]));
        }

        [TestMethod]
        public void TestUpdateSingleCriteriaByCriteriaReturnsFalseWhenNoUpdate()
        {
            // assert
            Assert.IsTrue(!_criteriaManager.UpdateSingleCriteriaByCriteria(_oldCriteriaList[0], _oldCriteriaList[0]));
        }

        [TestMethod]
        public void TestUpdateMultipleCriteriaByCriteriaDictionaryCorrectlyUpdatesCriteria()
        {
            // arrange - test init
            //act - nothing to capture, testing for truth
            //assert
            Assert.IsTrue(_criteriaManager.UpdateMultipleCriteriaByCriteriaDictionary(_oldKeyNewValue));
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateMultipleCriteriaByCriteriaDictionaryThrowsApplicationException()
        {
            // arrange - test init
            Dictionary<Criteria, Criteria> oldKeyOldValue = new Dictionary<Criteria, Criteria>();
            oldKeyOldValue.Add(_oldCriteriaList[0], _oldCriteriaList[0]);

            //act - nothing to capture
            _criteriaManager.UpdateMultipleCriteriaByCriteriaDictionary(oldKeyOldValue);
            //assert
            // nothing to assert, exception testing
        }


        [TestMethod]
        public void TestUpdateCriteriaByCriteriaFacetDictionaryReturnsTrue()
        {
            //bool UpdateCriteriaByCriteriaFacetDictionary(Dictionary<Facet, List<Criteria>> oldFacetCriteria, Dictionary<Facet, List<Criteria>> newFacetCriteria)
            Assert.IsTrue(_criteriaManager.UpdateCriteriaByCriteriaFacetDictionary(_oldFacetCriteria, _newFacetCriteria));

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateCriteriaByCriteriaFacetDictionaryThrowApplicationException()
        {
            //bool UpdateCriteriaByCriteriaFacetDictionary(Dictionary<Facet, List<Criteria>> oldFacetCriteria, Dictionary<Facet, List<Criteria>> newFacetCriteria)
            Assert.IsTrue(_criteriaManager.UpdateCriteriaByCriteriaFacetDictionary(_oldFacetCriteria, _oldFacetCriteria));

        }

    }
}

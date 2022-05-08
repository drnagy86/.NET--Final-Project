using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccessFakes;
using LogicLayer;
using DataObjects;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class FacetManagerTests
    {
        private IFacetManager _facetManager = null;

        private FacetVM newFacetVM = null;
        private FacetVM oldFacetVM = null;

        [TestInitialize]
        public void TestSetup()
        {
            _facetManager = new FacetManager(new FacetAccessorFake());
            oldFacetVM = new FacetVM()
            {
                FacetID = "Fake Facet 1",
                Description = "A longer description",
                Active = true,
                RubricID = 100000,
                FacetType = "Type1",
                Criteria = new List<Criteria>()
            };

            oldFacetVM.Criteria.Add(new Criteria()
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

            oldFacetVM.Criteria.Add(new Criteria()
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

            oldFacetVM.Criteria.Add(new Criteria()
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

            oldFacetVM.Criteria.Add(new Criteria()
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

            newFacetVM = new FacetVM()
            {
                FacetID = "Fake Facet 1 edit",
                Description = "A longer description edit",
                Active = true,
                RubricID = 100000,
                FacetType = "Type1",
                Criteria = new List<Criteria>()
            };

            newFacetVM.Criteria.Add(new Criteria()
            {
                CriteriaID = "Excellent edit",
                RubricID = 100000,
                FacetID = "Explaination edit",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination edit",
                Score = 4,
                Active = true,
            });

            newFacetVM.Criteria.Add(new Criteria()
            {
                CriteriaID = "Above Average edit",
                RubricID = 100000,
                FacetID = "Explaination edit",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination edit",
                Score = 3,
                Active = true,
            });

            newFacetVM.Criteria.Add(new Criteria()
            {
                CriteriaID = "Average edit",
                RubricID = 100000,
                FacetID = "Explaination edit",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination edit",
                Score = 2,
                Active = true,
            });

            newFacetVM.Criteria.Add(new Criteria()
            {
                CriteriaID = "Poor edit",
                RubricID = 100000,
                FacetID = "Explaination edit",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination edit",
                Score = 1,
                Active = true,
            });
        }

        [TestMethod]
        public void TestRetrieveActiveFacetsReturnsCorrectNumberOfFacets()
        {
            // arrange
            const int expectedNumberOfFacets = 5;
            int actualNumber;

            // act
            actualNumber = _facetManager.RetrieveActiveFacets().Count;

            // assert
            Assert.AreEqual(expectedNumberOfFacets, actualNumber);
        }

        [TestMethod]
        public void TestRetrieveFacetsByRubricIDReturnsCorrectFacet()
        {
            // arrange
            const int rubricID = 100000;

            const int expectedFacetCount = 3;
            int actualFacetCount;

            // act
            actualFacetCount = _facetManager.RetrieveFacetsByRubricID(rubricID).Count;

            // assert
            Assert.AreEqual(expectedFacetCount, actualFacetCount);
        }

        [TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveFacetsByBadRubricIDReturnsEmptyList()
        {
            // arrange
            const int rubricID = 100111;
            const int expectedCount = 0;
            int actualCount;
            
            // act
            actualCount = _facetManager.RetrieveFacetsByRubricID(rubricID).Count;

            // assert
            Assert.AreEqual(expectedCount,actualCount);
        }

        [TestMethod]        
        public void TestCreateFacetReturnsTrueIfSuccessfull()
        {
            // arrange
            const int rubricID = 100004;
            const string facetID = "test";
            const string description = "asdfasf";
            const string facetTypeID = "Explaination";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);            
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateFacetThrowsApplicationExceptionFacetIDIsEmpty()
        {
            // arrange
            const int rubricID = 100004;
            //const string facetID = "test";
            const string facetID = "";
            const string description = "asdfasf";
            const string facetTypeID = "Explaination";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateFacetThrowsApplicationExceptionFacetIDIsNull()
        {
            // arrange
            const int rubricID = 100004;
            //const string facetID = "test";
            const string facetID = null;
            const string description = "asdfasf";
            const string facetTypeID = "Explaination";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateFacetThrowsApplicationExceptionDescriptionIsEmpty()
        {
            // arrange
            const int rubricID = 100004;
            const string facetID = "test";
            const string description = "";
            //const string description = "asdfasf";
            const string facetTypeID = "Explaination";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateFacetThrowsApplicationDescriptionIsNull()
        {
            // arrange
            const int rubricID = 100004;
            const string facetID = "test";            
            //const string description = "asdfasf";
            const string description = null;
            const string facetTypeID = "Explaination";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateFacetThrowsApplicationExceptionFacetTypeIsEmpty()
        {
            // arrange
            const int rubricID = 100004;
            const string facetID = "test";
            const string description = "asdfasf";
            //const string facetTypeID = "Explaination";
            const string facetTypeID = "";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateFacetThrowsApplicationFacetTypeIsNull()
        {
            // arrange
            const int rubricID = 100004;
            const string facetID = "test";
            const string description = "asdfasf";
            
            //const string facetTypeID = "Explaination";
            const string facetTypeID = null;
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.CreateFacet(rubricID, facetID, description, facetTypeID);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpdateFacetDescriptionReturnsTrueIfSuccessfull()
        {
            // arrange
            const int rubricID = 100000;
            const string facetID = "Fake Facet 1";
            const string oldDescription = "A longer description";
            const bool expected = true;
            bool actual;

            const string expectedDescription = "Change";
            //string actualDescription = "";

            // act
            actual = _facetManager.UpdateFacetDescriptionByRubricIDAndFacetID(rubricID, facetID, oldDescription, expectedDescription);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateFacetDescriptionThrowsExceptionIfNotFound()        
        {
            // arrange
            const int rubricID = 100009;
            const string facetID = "Fake Facet 1";
            const string oldDescription = "A longer description";
            
            
            const string expectedDescription = "Change";
            //string actualDescription = "";

            // act
            _facetManager.UpdateFacetDescriptionByRubricIDAndFacetID(rubricID, facetID, oldDescription, expectedDescription);

            // assert
            // nothing to assert, expecting exception
        }


        [TestMethod]
        public void TestDeleteFacetByRubricIDAndFacetIDReturnsTrue()
        {
            // arrange
            const int rubricID = 100000;
            const string facetID = "Fake Facet 1";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.DeleteFacetByRubricIDAndFacetID(rubricID, facetID);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDeleteFacetByRubricIDAndFacetIDThrowsExceptionWithBadRubricID()
        {
            const int rubricID = 1000000;
            const string facetID = "Fake Facet 1";
            const bool expected = true;
            bool actual;

            // act
            actual = _facetManager.DeleteFacetByRubricIDAndFacetID(rubricID, facetID);

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TestRetrieveFacetVMReturnsCorrectFacet()
        {
            // arrange
            const int rubricID = 100000;
            const string facetID = "Fake Facet 1";
            const string expectedDescription = "A longer description";
            const string expectedFacetType = "Type1";
            const int expectedCriteriaCount = 4;

            int actualRubricID = 0;
            string actualFacetID = "";
            string actualDescription = "";
            string actualFacetType = "";
            int actualCriteriaCount = 0;

            // act
            FacetVM returnedFacet = _facetManager.RetrieveFacetVM(rubricID, facetID);

            // assert
            Assert.AreEqual(rubricID, returnedFacet.RubricID);
            Assert.AreEqual(facetID, returnedFacet.FacetID);
            Assert.AreEqual(expectedDescription, returnedFacet.Description);
            Assert.AreEqual(expectedFacetType, returnedFacet.FacetType);
            Assert.AreEqual(expectedCriteriaCount, returnedFacet.Criteria.Count);
        }


        [TestMethod]
        public void TestUpdateFacetVMReturnsTrueIfSuccessful()
        {
            // arrange
            // old and new facet set up in initializer
            const bool expected = true;
            bool actual = false;

            // act
            actual = _facetManager.UpdateFacetAndCriteria(oldFacetVM, newFacetVM);

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUpdateFacetVMThrowsExceptionIfNotFound()
        {
            // arrange
            // old and new facet set up in initializer
            oldFacetVM.FacetID = "x";

            // act
            _facetManager.UpdateFacetAndCriteria(oldFacetVM, newFacetVM);

            // assert
            // nothing to assert, expecting exception
        }


    }
}

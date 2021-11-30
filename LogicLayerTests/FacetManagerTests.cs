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

        [TestInitialize]
        public void TestSetup()
        {
            _facetManager = new FacetManager(new FacetAccessorFake());            
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
    }
}

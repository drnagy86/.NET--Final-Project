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
    }
}

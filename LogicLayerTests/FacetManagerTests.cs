using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccessFakes;
using LogicLayer;

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
            //  rubricManager = new RubricManager(new RubricAccessorFake(), new UserAccessorFake());
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
    }
}

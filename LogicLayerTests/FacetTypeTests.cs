using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccessInterFaces;
using DataAccessFakes;
using LogicLayer;
using DataObjects;



namespace LogicLayerTests
{
    [TestClass]
    public class FacetTypeTests
    {

        IFacetTypeManager _facetTypeManager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _facetTypeManager = new FacetTypeManager(new FacetTypeFakes());
        }


        [TestMethod]
        public void TestRetrieveAllFacets()
        {
            const int expectedCount = 2;
            List<FacetType> actualFacetTypeList = null;
            int actualCount;

            actualFacetTypeList = _facetTypeManager.RetrieveFacetTypes();
            actualCount = actualFacetTypeList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccessFakes;
using DataAccessInterFaces;
using DataAccessLayer;
using DataObjects;
using LogicLayer;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class ScoreTypeTests
    {
        IScoreTypeManager _scoreTypeManager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _scoreTypeManager = new ScoreTypeManager(new ScoreTypeFake());
        }

        [TestMethod]
        public void TestRetrieveAllScoreTypes()
        {
            // arrange
            const int expectedCount = 2;
            List<ScoreType> actualScoreTypeList = null;
            int acutalCount;

            // act
            actualScoreTypeList = _scoreTypeManager.RetrieveScoreTypes();
            acutalCount = actualScoreTypeList.Count;

            // assert
            Assert.AreEqual(expectedCount, acutalCount);
        }


    }
}

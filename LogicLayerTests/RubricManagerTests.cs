﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataObjects;
using DataAccessInterFaces;
using LogicLayer;
using DataAccessFakes;


namespace LogicLayerTests
{
    [TestClass]
    public class RubricManagerTests
    {
        private IRubricManager<Rubric> rubricManager = null;

        [TestInitialize]
        public void TestSetup()
        {
            rubricManager = new RubricManager(new RubricAccessorFake(), new UserAccessorFake());
        }

        [TestMethod]
        public void TestRetrieveActiveRubricsReturnsCorrectList()
        {
            // arrange

            const int expectedCount = 3;
            int actualCount;

            // act
            actualCount = (rubricManager.RetrieveActiveRubrics().Count);

            // assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestRetrieveRubricByRubricIDReturnsCorrectRubric()
        {
            // arrange
            Rubric rubric = null;
            const int expectedRubricID = 100000;
            int actualRubricID;

            // act
            rubric = rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            actualRubricID = rubric.RubricID;

            // assert
            Assert.AreEqual(expectedRubricID, actualRubricID);

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveRubricByBadRubricIDThrowsApplicationException()
        {
            // arrange
            Rubric rubric = null;
            const int expectedRubricID = 100111;
            int actualRubricID;

            // act
            rubric = rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            actualRubricID = rubric.RubricID;

            // assert
            // nothing to assert, exception testing
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveRubricByDuplicateRubricIDThrowsApplicationException()
        {
            // arrange
            Rubric rubric = null;
            const int expectedRubricID = 100003;
            int actualRubricID;

            // act
            rubric = rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            actualRubricID = rubric.RubricID;
            // assert
            // nothing to assert, exception testing
        }

        [TestMethod]
        public void TestRetrieveRubricReturnsCorrectRubricCreator()
        {
            // arrange
            Rubric rubric = null;
            const string expectedUserID = "tess@company.com";
            string actualUserID;

            // act
            rubric = rubricManager.RetrieveRubricByRubricID(100000);
            actualUserID = rubric.RubricCreator.UserID;

            // assert
            Assert.AreEqual(expectedUserID, actualUserID);

        }

        [TestMethod]
        public void TestCreateRubricReturnsTrueIfCreated()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = "Test Rubric";
            const string description = "A description";
            const string scoreType = "Percentage";
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = true;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricThrowsApplicationErrorIfNoName()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = "";
            const string description = "A description";
            const string scoreType = "Percentage";
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = false;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricThrowsApplicationErrorIfNullName()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = null;
            const string description = "A description";
            const string scoreType = "Percentage";
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = false;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricThrowsApplicationErrorIfNoDescription()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = "Test";
            const string description = "";
            const string scoreType = "Percentage";
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = false;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricThrowsApplicationErrorIfNullDescription()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = "Test";
            const string description = null;
            const string scoreType = "Percentage";
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = false;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricThrowsApplicationErrorIfNoScoreType()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = "Test";
            const string description = "asdfsaf";
            const string scoreType = "";
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = false;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricThrowsApplicationErrorIfNullScoreType()
        {
            // arrange
            //         public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
            const string name = "Test";
            const string description = "sadfasdf";
            const string scoreType = null;
            const string rubricCreator = "tess@company.com";
            //const string expectedID = "100005";
            //string actualID = "";
            const bool expected = false;
            bool actual;

            // act
            actual = rubricManager.CreateRubric(name, description, scoreType, rubricCreator);

            // assert
            Assert.AreEqual(expected, actual);

        }


    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LogicLayer;
using DataObjects;
using DataAccessFakes;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class RubricVMManagerTests
    {
        IRubricManager<RubricVM> _rubricManager = null;

        [TestInitialize]
        public void TestSetup()
        {
            UserAccessorFake userAccessorFake = new UserAccessorFake();            

            RubricManager rubricManager = new RubricManager(new RubricAccessorFake(), userAccessorFake);
            UserManager userManager = new UserManager(userAccessorFake);
            FacetManager facetManager = new FacetManager(new FacetAccessorFake());
            CriteriaManager criteriaManager = new CriteriaManager(new CriteriaAccessorFake());
            

            _rubricManager = new RubricVMManager(rubricManager, userManager, facetManager, criteriaManager, new RubricAccessorFake());
        }


        [TestMethod]
        public void TestRetrieveRubricByRubricIDReturnsCorrectRubric()
        {
            // arrange
            const int expectedRubricID = 100000;
            RubricVM actualRubricVM = null;
            int actualRubicID;

            // act
            actualRubricVM = _rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            actualRubicID = actualRubricVM.RubricID;

            //assert
            Assert.AreEqual(expectedRubricID, actualRubicID);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveRubricByRubricIDThrowsApplicationExceptionForBadRubricID()
        {
            // arrange
            const int expectedRubricID = 10000000;
            
            // act
            _rubricManager.RetrieveRubricByRubricID(expectedRubricID);
            // no need to assert, expecting exception
        }

        [TestMethod]
        public void TestRetrieveRubrics()
        {
            // arrange

            const int expectedCount = 3;
            int actualCount = 0;


            // act
            List<RubricVM> rubrics = _rubricManager.RetrieveActiveRubrics();
            actualCount = rubrics.Count;
            

            //assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TestCreateRubricVMSuccessReturnsNewRubricID()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "Test.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            test.FacetVMs.Add(new FacetVM());
            test.FacetVMs[0].Criteria.Add(new Criteria());
            test.FacetVMs[0].Criteria.Add(new Criteria());
            test.FacetVMs[0].Criteria.Add(new Criteria());
            
            const int expectedID = 100004;
            int actualID = 0;


            // act
            actualID = _rubricManager.CreateRubric(test);

            //assert
            Assert.AreEqual(expectedID, actualID);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForNoName()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "",
                Description = "Test.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert
            // nothing to assert, excecption testing
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForNameOver100Char()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "c2Je2Y1wP7j9ws2D6L1GrLdBLcYNP5snss6LJ0hE4RrefrzmfebRh3TdXCQOCTocPKEUVmyfPAEzuDlDpdyi4IUMiuVhNqtGuy6k",
                Description = "Test.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            // assert
            // nothing to assert, exception testing

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForNoDescription()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForDescriptionOver255Char()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "xPShzq0EamLJWcWyOGu2fgpuizCBdf1ntwOuI9MI0OI3p7qKU9YHuXzeMJrPMSKhlTYvYeqYolj4wD14xROyFjyXLl229L09e2ENtMnmCojXrA1KPlYIYnboAcsM0KkMMkpI8sWD2wmVAoulHRBhrf9mIwjVr2khPMnhC2TMHZzp5kClylTGsJEuEfjKq7M6Sw5cS23Z2SIioaNDeqGUKrTA1tQFiU9tjtO2Dsd5eGwHValGguYhlqCupGK2Cgq",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForNoScoreType()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "Test",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForScoreTypeOver50Char()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "Test",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "c2Je2Y1wP7j9ws2D6L1GrLdBLcYNP5snss6LJ0hE4RrefrzmfebRh3TdXCQOCTocPKEUVmyfPAEzuDlDpdyi4IUMiuVhNqtGuy6k",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionForNoUserID()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "Test",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionIfFacetVMsIsEmpty()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "Test",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "test",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            // act
            _rubricManager.CreateRubric(test);

            //assert
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCreateRubricVMThrowsExceptionIfNoCriteria()
        {
            // arrange
            RubricVM test = new RubricVM()
            {
                RubricID = 0,
                Name = "Test",
                Description = "Test.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreTypeID = "Percentage",

                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true,
                FacetVMs = new List<FacetVM>()
            };

            test.FacetVMs.Add(new FacetVM());

            // act
            _rubricManager.CreateRubric(test);

            //assert
            // nothing to assert, exception testing
        }




    }
}

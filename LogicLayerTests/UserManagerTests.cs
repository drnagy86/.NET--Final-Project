using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccessInterFaces;
using LogicLayer;
using DataAccessFakes;
using DataObjects;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class UserManagerTests
    {
        IUserManager userManager = null;

        [TestInitialize]

        public void TestSetup()
        {
            userManager = new UserManager(new UserAccessorFake());
        }

        [TestMethod]
        public void TestSha256ReturnsCorrectValue()
        {
            // arrange
            const string source = "newuser";
            const string expectedResult = "9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E";
            string actualResult = "";

            // Act
            actualResult = userManager.HashSha256(source);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);

        }


        [TestMethod]
        public void TestAuthenticateUserPassesWithCorrectUserNamePasswordHash()
        {
            // arrange
            const string userID = "tess@company.com";
            const string passwordHash = "9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E";
            const bool expectedResult = true;
            bool actualResult;

            // act
            actualResult = userManager.AuthenticateUser(userID, passwordHash);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAuthenticateUserFailsWithBadUserName()
        {
            // arrange
            const string userID = "xrtess@company.com";
            const string passwordHash = "9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E";
            const bool expectedResult = false;
            bool actualResult;

            // act
            actualResult = userManager.AuthenticateUser(userID, passwordHash);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void TestAuthenticateUserFailsWithBadPasswordHash()
        {
            // arrange
            const string userID = "tess@company.com";
            const string passwordHash = "x9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E";
            const bool expectedResult = false;
            bool actualResult;

            // act
            actualResult = userManager.AuthenticateUser(userID, passwordHash);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAuthenticateUserFailsDuplicateUsers()
        {
            // arrange
            const string userID = "duplicate@company.com";
            const string passwordHash = "dup-9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E";
            const bool expectedResult = false;
            bool actualResult;

            // act
            actualResult = userManager.AuthenticateUser(userID, passwordHash);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSelectUserByUserIDReturnsCorrectUser()
        {
            //arrange
            User user = null;
            const string expectedUserID = "tess@company.com";
            string actualUserID = "";

            //act
            user = userManager.GetUserByUserID(expectedUserID);
            actualUserID = user.UserID;

            // assert
            Assert.AreEqual(expectedUserID, actualUserID);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSelectUserByUserIDReutrnsApplicationException()
        {
            // arrange
            User user = null;
            const string badUserID = "xtess@company.com";
            // act
            user = userManager.GetUserByUserID(badUserID);
            // assert
            // nothing to assert, expecting exception
        }

        [TestMethod]
        public void TestGetRolesForUserReturnsCorrectList()
        {
            // arrange
            const string userID = "tess@company.com";
            var expectedRoles = new List<string>();
            expectedRoles.Add("Admin");
            expectedRoles.Add("Creator");

            List<string> actualRoles = null;

            //act
            actualRoles = userManager.GetRolesForUser(userID);

            //assert
            CollectionAssert.AreEqual(expectedRoles, actualRoles);

        }

        [TestMethod]
        public void TestGetRolesForUserFailsWithIncorrectList()
        {
            // arrange
            const string userID = "tess@company.com";
            var expectedRoles = new List<string>();
            expectedRoles.Add("xAdmin");
            expectedRoles.Add("Creator");

            List<string> actualRoles = null;

            //act
            actualRoles = userManager.GetRolesForUser(userID);

            //assert
            CollectionAssert.AreNotEquivalent(expectedRoles, actualRoles);

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestGetRolesForBadEmployeeIDThrowsApplicationException()
        {
            // arrange
            const string badUserID = "bad@company.com";

            // act
            userManager.GetRolesForUser(badUserID);

            // assert
            // exception checking


        }


    }
}

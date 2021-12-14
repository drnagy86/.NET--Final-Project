using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccessInterFaces;
using DataAccessFakes;
using LogicLayer;
using DataObjects;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class SubjectManagerTests
    {
        ISubjectManager _subjectManager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _subjectManager = new SubjectManager(new SubjectAccessorFake()
                );
        }

        [TestMethod]
        public void TestRetriveAllSubject()
        {
            //_facetTypes.Add(new FacetType()
            //{
            //    FacetTypeID = "Explaination",
            //    Description = "A long description",
            //    Active = true

            //});

            //arrange
            const int expectedCount = 2;
            List<Subject> actualSubjectList = null;
            int acutalCount;

            //act
            actualSubjectList = _subjectManager.RetrieveSubjects();
            acutalCount = actualSubjectList.Count;

            //assert
            Assert.AreEqual(expectedCount, acutalCount);

        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class RubricAccessorFake : IRubricAccessor
    {

        private List<Rubric> _fakeRubrics = new List<Rubric>();

        public RubricAccessorFake()
        {
            _fakeRubrics.Add(new Rubric()
            {
                RubricID = 100000,
                Name = "Test Rubric",
                Description = "A long description of the rubric.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreType = new ScoreType() { 
                    ScoreTypeID = "Percentage",
                    Description = "Correct divided by total as a percent",
                    Active = true
                },
                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()

                },
                Active = true
            });

            _fakeRubrics.Add(new Rubric()
            {
                RubricID = 100001,
                Name = "Test Rubric2",
                Description = "A long description of the rubric asfgsdfsdh.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreType = new ScoreType()
                {
                    ScoreTypeID = "Percentage",
                    Description = "Correct divided by total as a percent",
                    Active = true
                },
                RubricCreator = new User()
                {
                    UserID = "tess@company.com",
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()

                },
                Active = true
            });

            _fakeRubrics.Add(new Rubric()
            {
                RubricID = 100002,
                Name = "Test Rubric3",
                Description = "A long description of the rubric asfgsdfsdh.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreType = new ScoreType()
                {
                    ScoreTypeID = "Avg. Facet Score Round Down",
                    Description = "Correct divided by total as a percent",
                    Active = true
                },
                RubricCreator = new User()
                {
                    UserID = "another@company.com",
                    GivenName = "Another",
                    FamilyName = "Tess",
                    Active = true,
                    Roles = new List<string>()

                },
                Active = true
            });

            _fakeRubrics.Add(new Rubric()
            {
                RubricID = 100003,
                Name = "Test Duplicate",
                Description = "A long description of the rubric asfgsdfsdh.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreType = new ScoreType()
                {
                    ScoreTypeID = "Avg. Facet Score Round",
                    Description = "Correct divided by total as a percent",
                    Active = true
                },
                RubricCreator = new User()
                {
                    UserID = "dup@company.com",
                    GivenName = "Dup",
                    FamilyName = "Licate",
                    Active = true,
                    Roles = new List<string>()

                },
                Active = false
            });

            _fakeRubrics.Add(new Rubric()
            {
                RubricID = 100003,
                Name = "Test Duplicate",
                Description = "A long description of the rubric asfgsdfsdh.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ScoreType = new ScoreType()
                {
                    ScoreTypeID = "Avg. Facet Score Round",
                    Description = "Correct divided by total as a percent",
                    Active = true
                },
                RubricCreator = new User()
                {
                    UserID = "dup@company.com",
                    GivenName = "Dup",
                    FamilyName = "Licate",
                    Active = true,
                    Roles = new List<string>()

                },
                Active = false
            });


        }

        public Rubric SelectRubricByRubricID(int rubricID)
        {
            Rubric rubric = null;

            try
            {
                
                
                if (_fakeRubrics.Exists(r => r.RubricID == rubricID))
                {
                    if (_fakeRubrics.FindAll( r => r.RubricID == rubricID).Count > 1 )
                    {
                        throw new ApplicationException("Rubric ID is shared with another rubric.");
                    }
                    rubric = _fakeRubrics.Find(r => r.RubricID == rubricID);

                }
                else
                {
                    throw new NullReferenceException("No rubric with that ID could be found");
                }

            }
            catch (NullReferenceException ex)
            {
                throw new ApplicationException("No rubric with that rubric ID could be found" + ex.Message);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }            

            return rubric;
        }

        public List<Rubric> SelectRubrics()
        {
            List<Rubric> rubrics = new List<Rubric>();

            foreach (var rubric in _fakeRubrics)
            {
                if (rubric.Active == true)
                {
                    rubrics.Add(rubric);
                }
            }

            return rubrics;

        }
    }
}

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
                ScoreTypeID = "Percentage",                   
                
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
                ScoreTypeID = "Percentage",
                    
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
                ScoreTypeID = "Avg. Facet Score Round Down",
                    
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
                ScoreTypeID =  "Avg. Facet Score Round",
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
                ScoreTypeID = "Avg. Facet Score Round",
                    
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

        public int DeactivateRubricByRubricID(int rubricID)
        {
            int rowsAffected = 0;

            foreach (Rubric rubric in _fakeRubrics)
            {
                if (rubric.RubricID == rubricID)
                {
                    rubric.Active = false;
                    rowsAffected++;
                    break;
                }
            }

            return rowsAffected;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rubricID"></param>
        /// <returns></returns>
        public int DeleteRubricByRubricID(int rubricID)
        {

            int rowsAffected = 0;

            foreach (Rubric rubric in _fakeRubrics)
            {
                if (rubric.RubricID == rubricID)
                {
                    _fakeRubrics.Remove(rubric);

                    rowsAffected++;
                    break;
                }
            }

            return rowsAffected;

        }

        public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
        {
            int rowsAffected = 0;
            _fakeRubrics.Add(new Rubric()
            {
                RubricID = _fakeRubrics.ElementAt(_fakeRubrics.Count - 1).RubricID + 1,
                Name = name,
                Description = description,
                ScoreTypeID = scoreType,
                RubricCreator = new User()
                {
                    UserID = rubricCreator,
                    GivenName = "Tess",
                    FamilyName = "Data",
                    Active = true,
                    Roles = new List<string>()
                },
                Active = true
            });

            rowsAffected++;

            return rowsAffected;
        }

        public List<RubricVM> RetrieveActiveRubricsVMs()
        {
            List<RubricVM> rubrics = new List<RubricVM>();


            foreach (var rubric in _fakeRubrics)
            {
                if (rubric.Active == true)
                {
                    rubrics.Add(new RubricVM()
                    {
                        Name = rubric.Name,
                        Description = rubric.Description,
                        DateCreated = rubric.DateCreated,
                        DateUpdated = rubric.DateUpdated,
                        ScoreTypeID = rubric.ScoreTypeID,
                        RubricCreator = rubric.RubricCreator,
                        Active = rubric.Active
                    });
                }
            }

            return rubrics;
        }

        public Rubric SelectRubricByRubricDetials(string name, string description, string scoreType, string rubricCreator)
        {
            return _fakeRubrics.Find(r => r.Name == name && r.Description == description && r.ScoreTypeID == scoreType && r.RubricCreator.UserID == rubricCreator);
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

        public int UpdateRubricByRubricID(int rubricID, string oldName, string newName, string oldDescription, string newDescription, string oldScoreType, string newScoreType)
        {
            int rowsAffected = 0;

            foreach (Rubric rubric in _fakeRubrics)
            {
                if (rubric.RubricID == rubricID && rubric.Name == oldName && rubric.Description == oldDescription && rubric.ScoreTypeID == oldScoreType)
                {
                    rubric.Name = newName;
                    rubric.Description = newDescription;
                    rubric.ScoreTypeID = newScoreType;
                    rowsAffected++;
                    break;
                }
            }

            return rowsAffected;
        }
    }
}

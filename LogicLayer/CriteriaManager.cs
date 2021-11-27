using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessLayer;

namespace LogicLayer
{
    public class CriteriaManager : ICriteriaManager
    {
        private ICriteriaAccessor _criteriaAccessor = null;

        public CriteriaManager()
        {
            _criteriaAccessor = new CriteriaAccessor();
        }

        public CriteriaManager(ICriteriaAccessor criteriaAccessor)
        {
            _criteriaAccessor = criteriaAccessor;
        }

        public Dictionary<Criteria, Criteria> GetDictionaryOfDifferentCriteria(Dictionary<Facet, List<Criteria>> oldFacetCriteria, Dictionary<Facet, List<Criteria>> newFacetCriteria)
        {
            Dictionary<Criteria, Criteria> differentCriteria = new Dictionary<Criteria, Criteria>();

            foreach (var oldEntry in oldFacetCriteria)
            {
                foreach (var newEntry in newFacetCriteria)
                {
                    if (oldEntry.Key.FacetID == newEntry.Key.FacetID && oldEntry.Key.RubricID == newEntry.Key.RubricID)
                    {
                        List<Criteria> oldListOfCriteria = oldEntry.Value;
                        List<Criteria> newListOfCriteria = newEntry.Value;

                        for (int i = 0; i < oldListOfCriteria.Count; i++)
                        {
                            if (oldListOfCriteria[i].CriteriaID != newListOfCriteria[i].CriteriaID || oldListOfCriteria[i].Content != newListOfCriteria[i].Content)
                            {
                                differentCriteria.Add(oldListOfCriteria[i], newListOfCriteria[i]);
                            }
                        }
                    }
                }
            }

            return differentCriteria;

            // Green step
            //Criteria oldCriteria = new Criteria()
            //{
            //    CriteriaID = "Excellent",
            //    RubricID = 100000,
            //    FacetID = "Explaination",
            //    DateCreated = DateTime.Now,
            //    DateUpdated = DateTime.Now,
            //    Content = "Shows an excellent explaination",
            //    Score = 4,
            //    Active = true,
            //};

            //Criteria newCriteria = new Criteria()
            //{
            //    CriteriaID = "Changed CriteriaID",
            //    //CriteriaID = "Excellent",
            //    RubricID = 100000,
            //    FacetID = "Explaination",
            //    DateCreated = DateTime.Now,
            //    DateUpdated = DateTime.Now,
            //    Content = "A change in the content",
            //    //Content = "Shows an excellent explaination",
            //    Score = 4,
            //    Active = true,
            //};

        }

        public List<Criteria> RetrieveCriteriasForRubricByRubricID(int rubricID)
        {
            List<Criteria> criteriaList = new List<Criteria>();
            try
            {
                criteriaList = _criteriaAccessor.SelectCriteriaByRubricID(rubricID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return criteriaList;
        }

        public bool UpdateCriteriaByCriteriaFacetDictionary(Dictionary<Facet, List<Criteria>> oldFacetCriteria, Dictionary<Facet, List<Criteria>> newFacetCriteria)
        {
            bool result = false;

            try
            {
                result = UpdateMultipleCriteriaByCriteriaDictionary(GetDictionaryOfDifferentCriteria(oldFacetCriteria, newFacetCriteria));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!result)
            {
                throw new ApplicationException("No change in criteria to save.");
            }

            return result;
        }

        public int UpdateCriteriaByCriteriaID(int rubricID, string facetID, string oldCriteriaID, string newCriteriaID, string oldContent, string newContent)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = _criteriaAccessor.UpdateCriteriaByCriteriaID(rubricID, facetID, oldCriteriaID, newCriteriaID, oldContent, newContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (rowsAffected > 1)
            {
                throw new ApplicationException("Too many records were changed.");
            }
            if (rowsAffected == 0)
            {
                throw new ApplicationException("The criteria was not updated.");
            }

            return rowsAffected;
        }

        public int UpdateCriteriaContentByCriteriaID(int rubricID, string facetID, string criteriaID, string oldContent, string newContent)
        {
            int rowsAffected = 0;

            try
            {
                rowsAffected = _criteriaAccessor.UpdateCriteriaContentByCriteriaID(rubricID, facetID, criteriaID, oldContent, newContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (rowsAffected > 1)
            {
                throw new ApplicationException("Too many records were changed.");
            }
            if (rowsAffected == 0)
            {
                throw new ApplicationException("The criteria was not updated.");
            }

            return rowsAffected;
        }

        public bool UpdateMultipleCriteriaByCriteriaDictionary(Dictionary<Criteria, Criteria> oldKeyNewValueDictionary)
        {
            bool result = false;

            foreach (var entry in oldKeyNewValueDictionary)
            {
                try
                {
                    result = UpdateSingleCriteriaByCriteria(entry.Key, entry.Value);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Problem with updating Rubric \"{0}\" Facet \"{1}\" with old criteria \"{2}\" to new criteria ID of \"{3}\" and content of \"{4}\"\n{5}", entry.Key.RubricID, entry.Key.FacetID, entry.Key.CriteriaID, entry.Value.CriteriaID, entry.Value.Content, ex.Message));
                }  
            }

            if (!result)
            {
                throw new ApplicationException("No criteria were updated"); 
            }

            return result;
        }

        public bool UpdateSingleCriteriaByCriteria(Criteria oldCriteria, Criteria newCriteria)
        {
            bool result = false;
            int rowsAffected = 0;           

            bool differentCriteriaID = oldCriteria.CriteriaID != newCriteria.CriteriaID;
            bool differentContent = oldCriteria.Content != newCriteria.Content;

            if (!differentCriteriaID && differentContent)
            {
                try
                {
                    rowsAffected = UpdateCriteriaContentByCriteriaID(oldCriteria.RubricID, oldCriteria.FacetID, oldCriteria.CriteriaID, oldCriteria.Content, newCriteria.Content);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            else if (differentCriteriaID && differentContent)
            {
                try
                {
                    rowsAffected = UpdateCriteriaByCriteriaID(oldCriteria.RubricID, oldCriteria.FacetID, oldCriteria.CriteriaID, newCriteria.CriteriaID, oldCriteria.Content, newCriteria.Content);
                }
                catch (Exception ex)
                {
                    // this was tested in UpdateCriteriaByCriteriaID tests
                    throw ex;
                }
            }

            if (rowsAffected == 1)
            {
                result = true;
            }

            return result;
        }
    }
}

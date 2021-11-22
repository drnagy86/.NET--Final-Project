using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;

namespace DataAccessFakes
{
    public class CriteriaAccessorFake : ICriteriaAccessor
    {
        List<Criteria> _criteriaList = new List<Criteria>();


        public CriteriaAccessorFake()
        {
            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated= DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100000,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Excellent",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an excellent explaination",
                Score = 4,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Above Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an good explaination",
                Score = 3,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Average",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows an average explaination",
                Score = 2,
                Active = true,
            });

            _criteriaList.Add(new Criteria()
            {
                CriteriaID = "Poor",
                RubricID = 100001,
                FacetID = "Explaination",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Content = "Shows a poor explaination",
                Score = 1,
                Active = true,
            });

        }

        public List<Criteria> SelectCriteriaByRubricID(int rubricID)
        {
            return _criteriaList.FindAll(c => c.RubricID == rubricID);
        }

        public int UpdateCriteriaByCriteriaID(int rubricID, string facetID, string oldCriteriaID, string newCriteriaID, string oldContent, string newContent)
        {
            int rowsAffected = 0;

            foreach (Criteria criteria in _criteriaList)
            {
                if (criteria.RubricID == rubricID && criteria.FacetID == facetID && criteria.CriteriaID == oldCriteriaID && criteria.Content == oldContent)
                {
                    criteria.CriteriaID = newCriteriaID;
                    criteria.Content = newContent;
                    rowsAffected++;
                }
            }

            return rowsAffected;
        }
    }
}

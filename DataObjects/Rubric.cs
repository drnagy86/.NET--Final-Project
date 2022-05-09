using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Rubric
    {
        public int RubricID { get; set; }

        [Required(ErrorMessage = "Please enter a name for the rubric")]
        [StringLength(100, ErrorMessage = "The name can not be longer than 100 characters")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter a description for the rubric")]
        [StringLength(255, ErrorMessage = "The name can not be longer than 255 characters")]
        public string Description { get; set; }

        [Display(Name = "Date Created")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateUpdated { get; set; }

        [Display(Name = "Score Type")]
        public String ScoreTypeID { get; set; }

        public User RubricCreator { get; set; }
        public bool Active { get; set; }

        [Display(Name = "Number of Criteria")]
        [Required(ErrorMessage = "Please enter a value for a number of criteria that each facet will have")]
        [Range(0, 9)]
        public int NumberOfCriteria { get; set; }

        public Rubric()
        {

        }

        public Rubric(string name, string description, string scoreTypeID, User rubricCreator, bool active = true)
        {
            this.Name = name;
            this.Description = description;
            this.ScoreTypeID = scoreTypeID;
            this.RubricCreator = rubricCreator;
            this.Active = active;
            this.NumberOfCriteria = 3;

        }

    }


    public class RubricVM : Rubric
    {
        public List<Facet> Facets { get; set; }
        public List<Criteria> Criteria { get; set; }
        public Dictionary<Facet, List<Criteria>> FacetCriteria { get; set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public List<FacetVM> FacetVMs { get; set; }

        public RubricVM()
        {

        }

        public RubricVM(User creator)
        {
            this.RubricID = 0;
            this.Name = "";
            this.Description = "";
            this.ScoreTypeID = "";
            this.RubricCreator = creator;
            this.Active = true;
            this.DateCreated = DateTime.Now;
            this.DateUpdated= DateTime.Now;
            this.NumberOfCriteria = 3;

            this.FacetVMs = new List<FacetVM>();

            


        }

        public RubricVM(Rubric rubric, List<Facet> facets, List<Criteria> criteria)
        {

            this.RubricID = rubric.RubricID;
            this.Name = rubric.Name;
            this.Description = rubric.Description;
            this.DateCreated = rubric.DateCreated;
            this.DateUpdated = rubric.DateCreated;
            this.ScoreTypeID = rubric.ScoreTypeID;
            this.RubricCreator = rubric.RubricCreator;
            this.Active = rubric.Active;
            this.Facets = facets;
            this.Criteria = criteria;
            this.FacetCriteria = createFacetCriteriaDictionary(facets, criteria);
            this.NumberOfCriteria = 3;
            this.ColumnCount = FacetCriteria.Count;


        }

        public List<int> RubricScoreColumn()
        {
            List<int> rubricScores = new List<int>();

            foreach (var entry in FacetCriteria)
            {
                foreach (var criteria in entry.Value)
                {
                    if (!rubricScores.Contains(criteria.Score))
                    {
                        rubricScores.Add(criteria.Score);
                    }                    
                }
            }
            
            return rubricScores;
        }

        public void AddBlankFacetWithCritria()
        {
            this.FacetVMs = new List<FacetVM>();

            this.FacetVMs.Add(new FacetVM()
            {
                FacetID = "Name the facet",
                Description = "Describe what this facet is attempting to evaluate.",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Active = true,
                RubricID = 0,
                Criteria = new List<Criteria>(),
                FacetType = "Explanation"
            });

            for (int i = 0; i < this.NumberOfCriteria; i++)
            {
                this.FacetVMs[0].Criteria.Add(new Criteria()
                {
                    CriteriaID = "Criteria " + i,
                    RubricID = 0,
                    FacetID = "Name the facet",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Content = "Describe the criteria that earns the score",
                    Score = i,
                    Active = true
                });
            }



            
        }

        private Dictionary<Facet, List<Criteria>> createFacetCriteriaDictionary(List<Facet> facets, List<Criteria> criteriaList)
        {

            Dictionary<Facet, List<Criteria>> facetCriteriaDictionary = new Dictionary<Facet, List<Criteria>>();

            foreach (Facet facet in facets)
            {
                List<Criteria> criteriaForFacet = new List<Criteria>();

                foreach (Criteria criteria in criteriaList)
                {
                    if (criteria.FacetID == facet.FacetID)
                    {
                        criteriaForFacet.Add(criteria);
                    }
                }

                if (criteriaForFacet.Count == 0)
                {
                    // find out the amount of rows needed
                    Dictionary<String, int> facetCriteriaCount = new Dictionary<string, int>();
                    for (int i = 0; i < criteriaList.Count; i++)
                    {
                        if (!facetCriteriaCount.ContainsKey(criteriaList[i].FacetID))
                        {
                            facetCriteriaCount.Add(criteriaList[i].FacetID, 1);
                        }
                        else
                        {
                            facetCriteriaCount[criteriaList[i].FacetID]++;
                        }
                    }
                    this.RowCount = facetCriteriaCount.Values.Max();

                    for (int i = 0; i < this.RowCount; i++)
                    {
                        criteriaForFacet.Add(new Criteria(this.RubricID, facet.FacetID));
                    }
                }

                facetCriteriaDictionary.Add(facet, criteriaForFacet);
                //this.FacetCriteria.Add(facet, criteriaForFacet.FindAll(c => c.FacetID == facet.FacetID));               

            }

            return facetCriteriaDictionary;
        }

    }
}

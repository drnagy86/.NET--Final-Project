using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class FacetAccessor : IFacetAccesor
    {
        private CriteriaAccessor _criteriaAccessor = null;

        public FacetAccessor()
        {
            _criteriaAccessor = new CriteriaAccessor();
        }

        public int DeleteFacetByRubricIDAndFacetID(int rubricID, string facetID)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_delete_facet_by_rubric_id_and_facet_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RubricID", rubricID);
            cmd.Parameters.AddWithValue("@FacetID", facetID);


            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;
        }

        public int InsertFacet(int rubricID, string facetID, string description, string facetType)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_create_facet";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters.Add("@FacetID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@FacetTypeID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@RubricID"].Value = rubricID;
            cmd.Parameters["@FacetID"].Value = facetID;
            cmd.Parameters["@Description"].Value = description;
            cmd.Parameters["@FacetTypeID"].Value = facetType;

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }


            return rowsAffected;
        }

        public List<Facet> SelectFacets()
        {
            List<Facet> facets = new List<Facet>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_facets";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Facet facet = new Facet()
                        {
                            FacetID = reader.GetString(0),
                            Description = reader.GetString(1),
                            DateCreated = reader.GetDateTime(2),
                            DateUpdated = reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            RubricID = reader.GetInt32(5),
                            FacetType = reader.GetString(6)
                        };
                        facets.Add(facet);
                    }
                }
                else
                {
                    throw new ApplicationException("No facets found");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return facets;
        }

        public List<Facet> SelectFacetsByRubricID(int rubricID)
        {
            List<Facet> facets = new List<Facet>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_facets_by_rubric_id";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters["@RubricID"].Value = rubricID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Facet facet = new Facet()
                        {
                            FacetID = reader.GetString(0),
                            Description = reader.GetString(1),
                            DateCreated = reader.GetDateTime(2),
                            DateUpdated = reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            RubricID = reader.GetInt32(5),
                            FacetType = reader.GetString(6)
                        };
                        facets.Add(facet);
                    }
                }
                else
                {
                    throw new ApplicationException("No facets found");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return facets;
        }

        public FacetVM SelectFacetVM(int rubricID, string facetID)
        {
            FacetVM facet = null;
            List<Criteria> criteria = new List<Criteria>();

            var conn = DBConnection.GetConnection();

            var cmdText = "sp_select_facet_by_rubric_id_and_facet_id";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters["@RubricID"].Value = rubricID;

            cmd.Parameters.Add("@FacetID", SqlDbType.NVarChar);
            cmd.Parameters["@FacetID"].Value = facetID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        facet = new FacetVM()
                        {
                            FacetID = reader.GetString(0),
                            Description = reader.GetString(1),
                            DateCreated = reader.GetDateTime(2),
                            DateUpdated = reader.GetDateTime(3),
                            Active = reader.GetBoolean(4),
                            RubricID = reader.GetInt32(5),
                            FacetType = reader.GetString(6)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            var cmd2Text = "sp_select_criteria_by_rubric_id_and_facet_id";
            var cmd2 = new SqlCommand(cmd2Text, conn);

            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd2.Parameters["@RubricID"].Value = rubricID;

            cmd2.Parameters.Add("@FacetID", SqlDbType.NVarChar);
            cmd2.Parameters["@FacetID"].Value = facetID;

            try
            {
                conn.Open();
                var reader = cmd2.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        criteria.Add( new Criteria()
                        {
                            // good
                            CriteriaID = reader.GetString(0),
                            //CriteriaID = "Perspective",

                            // exception
                            RubricID = reader.GetInt32(1),
                            //RubricID = 100000,

                            // Message = "Unable to cast object of type 'System.DateTime' to type 'System.String'."
                            FacetID = reader.GetString(2),
                            //FacetID = "Facet",


                            // ex = {"Specified cast is not valid."}
                            Active = reader.GetBoolean(3),
                            //Active = true,

                            // "Specified cast is not valid."
                            DateCreated = reader.GetDateTime(4),
                            //DateCreated = DateTime.Now,


                            // ex = {"Unable to cast object of type 'System.DateTime' to type 'System.String'."}
                            DateUpdated = reader.GetDateTime(5),
                            //DateUpdated = DateTime.Now,

                            // ex = {"Unable to cast object of type 'System.DateTime' to type 'System.String'."}
                            Content = reader.GetString(6),
                            //Content = "asddg",

                            // ex = {"Unable to cast object of type 'System.DateTime' to type 'System.String'."}
                            Score = reader.GetInt32(7)
                            //Score = 1
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            facet.Criteria = criteria;


            return facet;
        }

        public int UpdateFacetAndCriteraWithFacetVM(FacetVM oldFacet, FacetVM newFacet)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_update_facets_by_rubric_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RubricID", oldFacet.RubricID);
            cmd.Parameters.AddWithValue("@OldFacetID", oldFacet.FacetID);            
            cmd.Parameters.AddWithValue("@OldDescription", oldFacet.Description);
            cmd.Parameters.AddWithValue("@OldFacetType", oldFacet.FacetType);

            cmd.Parameters.Add("@NewFacetID", SqlDbType.NVarChar, 100);
            cmd.Parameters["@NewFacetID"].Value = newFacet.FacetID;

            cmd.Parameters.Add("@NewDescription", SqlDbType.NVarChar, 255);
            cmd.Parameters["@NewDescription"].Value = newFacet.Description;

            cmd.Parameters.AddWithValue("@NewFacetType", newFacet.FacetType);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            for (int i = 0; i < oldFacet.Criteria.Count; i++)
            {
                _criteriaAccessor.UpdateCriteriaByCriteriaID(oldFacet.RubricID, newFacet.FacetID, oldFacet.Criteria[i].CriteriaID, newFacet.Criteria[i].CriteriaID, oldFacet.Criteria[i].Content, newFacet.Criteria[i].Content);
            }

            return rowsAffected;
        }

        public int UpdateFacetDescriptionByRubricIDAndFacetID(int rubricID, string facetID, string oldDescription, string newDescription)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_update_facet_description_by_rubric_id_and_facet_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RubricID", rubricID);
            cmd.Parameters.AddWithValue("@FacetID", facetID);
            cmd.Parameters.AddWithValue("OldDescription", oldDescription);
                        
            cmd.Parameters.Add("@NewDescription", SqlDbType.NVarChar, 100);
            cmd.Parameters["@NewDescription"].Value = newDescription;            

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rowsAffected;
        }
    }
}

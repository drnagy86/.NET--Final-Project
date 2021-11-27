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
    public class CriteriaAccessor : ICriteriaAccessor
    {
        public List<Criteria> SelectCriteriaByRubricID(int rubricID)
        {
            List<Criteria> criteriaList = new List<Criteria>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_criteria_by_rubric_id";

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
                        Criteria criteria = new Criteria()
                        {
                            CriteriaID = reader.GetString(0),
                            RubricID = reader.GetInt32(1),
                            FacetID = reader.GetString(2),
                            Active = reader.GetBoolean(3),
                            DateCreated = reader.GetDateTime(4),
                            DateUpdated = reader.GetDateTime(5),
                            Content =  reader.GetString(6),
                            Score = reader.GetInt32(7)
                        };
                        criteriaList.Add(criteria);
                    }
                }
                else
                {
                    throw new ApplicationException("No criteria found.");
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


            return criteriaList;
        }


        public int UpdateCriteriaByCriteriaID(int rubricID, string facetID, string oldCriteriaID, string newCriteriaID, string oldContent, string newContent)
        {
            int rowsAffected = 0;

            var conn = DBConnection.GetConnection();

            var cmdTxt = "sp_update_criteria_by_criteria_id";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters.Add("@FacetID", SqlDbType.NVarChar, 50);

            cmd.Parameters.Add("@OldCriteriaID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@NewCriteriaID", SqlDbType.NVarChar, 50);

            cmd.Parameters.Add("@OldContent", SqlDbType.NVarChar, 255);
            cmd.Parameters.Add("@NewContent", SqlDbType.NVarChar, 255);

            cmd.Parameters["@RubricID"].Value = rubricID;
            cmd.Parameters["@FacetID"].Value = facetID;

            cmd.Parameters["@OldCriteriaID"].Value = oldCriteriaID;
            cmd.Parameters["@NewCriteriaID"].Value = newCriteriaID;

            cmd.Parameters["@OldContent"].Value = oldContent;
            cmd.Parameters["@NewContent"].Value = newContent;

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

        public int UpdateCriteriaContentByCriteriaID(int rubricID, string facetID, string criteriaID, string oldContent, string newContent)
        {
            int rowsAffected = 0;

            var conn = DBConnection.GetConnection();

            var cmdTxt = "sp_update_criteria_content_by_criteria_id";

            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters.Add("@FacetID", SqlDbType.NVarChar, 50);

            cmd.Parameters.Add("@CriteriaID", SqlDbType.NVarChar, 50);

            cmd.Parameters.Add("@OldContent", SqlDbType.NVarChar, 255);
            cmd.Parameters.Add("@NewContent", SqlDbType.NVarChar, 255);

            cmd.Parameters["@RubricID"].Value = rubricID;
            cmd.Parameters["@FacetID"].Value = facetID;

            cmd.Parameters["@CriteriaID"].Value =criteriaID;

            cmd.Parameters["@OldContent"].Value = oldContent;
            cmd.Parameters["@NewContent"].Value = newContent;

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

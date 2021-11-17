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
    }
}

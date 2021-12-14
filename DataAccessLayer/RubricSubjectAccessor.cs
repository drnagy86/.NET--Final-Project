using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterFaces;
using DataObjects;

namespace DataAccessLayer
{
    public class RubricSubjectAccessor : IRubricSubjectAccessor
    {
        public int DeleteRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_delete_rubric_subject_by_subject_id_and_rubric_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SubjectID", subjectID);
            cmd.Parameters.AddWithValue("@RubricID", rubricID);

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

        public int InsertRubricSubjectBySubjectIDAndRubricID(string subjectID, int rubricID, string description)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_create_rubric_subject";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SubjectID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@RubricID", SqlDbType.Int);
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100);

            cmd.Parameters["@SubjectID"].Value = subjectID;
            cmd.Parameters["@RubricID"].Value = rubricID;
            cmd.Parameters["@Description"].Value = description;

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

        public List<RubricSubject> SelectRubricSubjectsByRubricID(int rubricID)
        {
            List<RubricSubject> rubricSubjects = new List<RubricSubject>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_rubric_subjects_by_rubric_id";

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
                        RubricSubject rubricSubject = new RubricSubject()
                        {
                            SubjectID = reader.GetString(0),
                            RubricID = reader.GetInt32(1),
                            DateCreated = reader.GetDateTime(2),
                            Active = reader.GetBoolean(3)
                        };
                        rubricSubjects.Add(rubricSubject);
                    }
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
            return rubricSubjects;
        }
    }
}

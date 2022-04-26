using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterFaces;
using DataObjects;
using System.Data;

namespace DataAccessLayer
{
    public class RubricAccessor : IRubricAccessor
    {
        public int DeactivateRubricByRubricID(int rubricID)
        {

            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_deactivate_rubric_by_rubric_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

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

        public int DeleteRubricByRubricID(int rubricID)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_delete_rubric_by_rubric_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

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

        public int InsertRubric(string name, string description, string scoreType, string rubricCreator)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_create_rubric";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@ScoreTypeID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@RubricCreator", SqlDbType.NVarChar, 50);

            cmd.Parameters["@Name"].Value = name;
            cmd.Parameters["@Description"].Value = description;
            cmd.Parameters["@ScoreTypeID"].Value = scoreType;
            cmd.Parameters["@RubricCreator"].Value = rubricCreator;

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

        public List<RubricVM> RetrieveActiveRubricsVMs()
        {
            List<RubricVM> rubrics = new List<RubricVM>();


            // connection
            var conn = DBConnection.GetConnection();

            // command text
            var cmdTxt = "sp_select_active_rubrics";

            // command
            var cmd = new SqlCommand(cmdTxt, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RubricVM rubric = new RubricVM()
                        {
                            RubricID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateCreated = reader.GetDateTime(3),
                            DateUpdated = reader.GetDateTime(4),
                            ScoreTypeID = reader.GetString(5),
                            RubricCreator = new User()
                            {
                                UserID = reader.GetString(6),
                                GivenName = reader.GetString(7),
                                FamilyName = reader.GetString(8)
                            }
                        };

                        rubrics.Add(rubric);
                    }
                }
                else
                {
                    throw new ApplicationException("Rubric not found");
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

            return rubrics;

            // list of facets

            // list of criteria



            return rubrics;
         
        }

        public Rubric SelectRubricByRubricDetials(string name, string description, string scoreType, string rubricCreator)
        {
            Rubric rubric = null;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_rubric_by_name_description_score_type_id_rubric_creator";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@ScoreTypeID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@RubricCreator", SqlDbType.NVarChar, 50);

            cmd.Parameters["@Name"].Value = name;
            cmd.Parameters["@Description"].Value = description;
            cmd.Parameters["@ScoreTypeID"].Value = scoreType;
            cmd.Parameters["@RubricCreator"].Value = rubricCreator;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    int rowCount = 0;

                    while (reader.Read())
                    {
                        rubric = new Rubric()
                        {
                            RubricID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateCreated = reader.GetDateTime(3),
                            DateUpdated = reader.GetDateTime(4),
                            ScoreTypeID = reader.GetString(5),
                            RubricCreator = new User()
                            {
                                UserID = reader.GetString(6),
                                GivenName = reader.GetString(7),
                                FamilyName = reader.GetString(8),
                                Active = reader.GetBoolean(9),
                                Roles = new List<string>()
                            },
                            Active = reader.GetBoolean(10)

                        };

                        rowCount++;

                    }

                    if (rowCount > 1)
                    {
                        throw new ApplicationException("Problem retrieving a unique rubric");
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

            return rubric;

        }

        public Rubric SelectRubricByRubricID(int rubricID)
        {
            Rubric rubric = null;

            var conn = DBConnection.GetConnection();

            var cmdText = "sp_select_rubric_by_rubric_id";

            var cmd = new SqlCommand(cmdText, conn);

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
                        rubric = new Rubric()
                        {
                            RubricID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateCreated = reader.GetDateTime(3),
                            DateUpdated = reader.GetDateTime(4),
                            ScoreTypeID = reader.GetString(5),
                            RubricCreator = new User() {
                                UserID = reader.GetString(6),
                                GivenName = reader.GetString(7),
                                FamilyName = reader.GetString(8),
                                Active = reader.GetBoolean(9),
                                Roles = new List<string>()
                            },
                            Active = reader.GetBoolean(10)
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
            return rubric;
        }

        public List<Rubric> SelectRubrics()
        {
            List<Rubric> rubrics = new List<Rubric>();

            // connection
            var conn = DBConnection.GetConnection();

            // command text
            var cmdTxt = "sp_select_active_rubrics";

            // command
            var cmd = new SqlCommand(cmdTxt, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Rubric rubric = new Rubric()
                        {
                            RubricID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateCreated = reader.GetDateTime(3),
                            DateUpdated = reader.GetDateTime(4),
                            ScoreTypeID = reader.GetString(5), 
                            RubricCreator = new User() { 
                                UserID = reader.GetString(6),
                                GivenName = reader.GetString(7),
                                FamilyName = reader.GetString(8)
                            }
                        };

                        rubrics.Add(rubric);
                    }
                }
                else
                {
                    throw new ApplicationException("Rubric not found");
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

            return rubrics;
        }

        public int UpdateRubricByRubricID(int rubricID, string oldName, string newName, string oldDescription, string newDescription, string oldScoreType, string newScoreType)
        {
            int rowsAffected = 0;

            // connection
            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_update_rubric_by_rubric_id";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RubricID", rubricID);
            cmd.Parameters.AddWithValue("@OldName", oldName);
            cmd.Parameters.AddWithValue("OldDescription", oldDescription);
            cmd.Parameters.AddWithValue("OldScoreTypeID", oldScoreType);

            cmd.Parameters.Add("@NewName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@NewDescription", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewScoreTypeID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@NewName"].Value = newName;
            cmd.Parameters["@NewDescription"].Value = newDescription;
            cmd.Parameters["@NewScoreTypeID"].Value = newScoreType;
            

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

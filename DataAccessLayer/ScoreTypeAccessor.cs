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
    public class ScoreTypeAccessor : IScoreTypeAccessor
    {
        public List<ScoreType> SelectScoreTypes()
        {
            List<ScoreType> scoreTypes = new List<ScoreType>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_score_types";

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
                        ScoreType scoreType = new ScoreType()
                        {
                            ScoreTypeID = reader.GetString(0),
                            Description = reader.GetString(1)
                        };
                        scoreTypes.Add(scoreType);
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
            return scoreTypes;
        }
    }
}

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
        public Rubric SelectRubricByRubricID(int rubricID)
        {
            throw new NotImplementedException();
        }

        public List<Rubric> SelectRubrics()
        {
            List<Rubric> rubrics = new List<Rubric>();

            // connection
            var conn = DBConnection.GetConnection();

            // command text
            var cmdText = "sp_select_active_rubrics";

            // command
            var cmd = new SqlCommand(cmdText, conn);

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
                            //ScoreType = null,
                            //RubricCreator = null

                            ScoreTypeID = reader.GetString(5), 
                            RubricCreator = new User() { UserID = reader.GetString(6) }

                            // with the score type id, I would need to make another call to that table to get the rest of that information?

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
            return rubrics;
        }
    }
}

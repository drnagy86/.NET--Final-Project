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
    public class SubjectAccessor : ISubjectAccessor
    {


        public List<Subject> SelectSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_select_subjects";

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
                        Subject subject= new Subject()
                        {
                            SubjectID = reader.GetString(0),
                            Description = reader.GetString(1),
                            DateCreated = reader.GetDateTime(2),
                            DateUpdated = reader.GetDateTime(3),
                            Active = reader.GetBoolean(4)
                        };
                        subjects.Add(subject);
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
            return subjects;
        }
    }
}

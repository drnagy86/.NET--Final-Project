using DataAccessInterFaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public int AuthenticateUserWithUserIDAndPasswordHash(string userID, string passwordHash)
        {
            int result = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = "sp_authenticate_user";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@UserID"].Value = userID;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                conn.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public List<string> SelectRolesByUserID(string userID)
        {
            List<string> roles = new List<string>();

            //connection
            var conn = DBConnection.GetConnection();

            //command text
            var cmdText = "sp_select_user_roles_by_userID";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar);

            //values
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                // open connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                //* added this 
                conn.Close();
            }
            return roles;
        }

        public User SelectUserByUserID(string userID)
        {
            User user = null;

            //connection
            var conn = DBConnection.GetConnection();

            //command text
            string cmdText = "sp_select_user_by_userID";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 50);

            // value
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                // open the connection
                conn.Open();

                // execute the command and capture results
                var reader = cmd.ExecuteReader();

                // process the results
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            UserID = reader.GetString(0),
                            GivenName = reader.GetString(1),
                            FamilyName = reader.GetString(2),
                            Active = reader.GetBoolean(3)
                        };
                    }
                    
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;


        }


    }
}

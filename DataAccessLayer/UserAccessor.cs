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

        public int UpdatePasswordHash(string userID, string oldPassword, string newPassword)
        {
            int rowsAffected = 0;

            //connection
            var conn = DBConnection.GetConnection();
            // set cmd Text
            var cmdText = "sp_update_passwordHash";
            // create command object
            var cmd = new SqlCommand(cmdText, conn);
            // command type
            cmd.CommandType = CommandType.StoredProcedure;


            // add parameters
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);
            // param values
            cmd.Parameters["@UserID"].Value = userID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPassword;
            cmd.Parameters["@NewPasswordHash"].Value = newPassword;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return rowsAffected;


        }

        public List<string> SelectAllRoles()
        {
            List<string> roles = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_all_roles");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // open connection
                conn.Open();

                // execute the first command

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public List<string> SelectRolesByEmployeeID(int employeeID)
        {
            List<string> roles = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_roles_by_employeeID");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = employeeID;

            try
            {
                // open connection
                conn.Open();

                // execute the first command

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }


        public User GetUserByEmail(string userID)
        {
            User user = null;

            // connection
            var conn = DBConnection.GetConnection();

            // command objects (2)
            var cmd1 = new SqlCommand("sp_select_user_by_userID");
            var cmd2 = new SqlCommand("sp_select_roles_by_userID");

            cmd1.Connection = conn;
            cmd2.Connection = conn;

            cmd1.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd1.Parameters.Add("@UserID", SqlDbType.NVarChar, 50);
            cmd1.Parameters["@UserID"].Value = userID;

            cmd2.Parameters.Add("@UserID", SqlDbType.NVarChar, 50);
            cmd2.Parameters["@UserID"].Value = userID;

            try
            {
                // open connection
                conn.Open();

                // execute the first command
                var reader1 = cmd1.ExecuteReader();

                if (reader1.Read())
                {
                    user = new User()
                    {
                        UserID = reader1.GetString(0),
                        GivenName = reader1.GetString(1),
                        FamilyName = reader1.GetString(2)
                    };
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
                reader1.Close(); // this is no longer needed

                var reader2 = cmd2.ExecuteReader();

                List<string> roles = new List<string>();
                while (reader2.Read())
                {
                    string role = reader2.GetString(0);
                    roles.Add(role);
                }
                user.Roles = roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        public User AuthenticateUser(string username, string passwordHash)
        {
            User result = null; // change this to 1 if the user is authenticated

            // first, get a connection
            var conn = DBConnection.GetConnection();

            // next, we need a command object
            var cmd = new SqlCommand("sp_authenticate_user");
            cmd.Connection = conn;

            // set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameters for the procedure
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // set the values for the parameters
            cmd.Parameters["@UserID"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // now that the command is set up, we can execute it
            try
            {
                // open the connection
                conn.Open();

                // execute the command
                if (1 == Convert.ToInt32(cmd.ExecuteScalar()))
                {
                    // if the command worked correctly, get a user
                    // object
                    result = GetUserByEmail(username);
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
            finally
            {
                conn.Close();
            }
            return result;
        }

        public bool InsertUser(User user)
        {
            bool result = false;
            int rowsAffected = 0;

            var conn = DBConnection.GetConnection();

            string cmdTxt = "sp_create_user";
            var cmd = new SqlCommand(cmdTxt, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@GivenName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@FamilyName", SqlDbType.NVarChar, 50);            

            cmd.Parameters["@UserID"].Value = user.UserID;
            cmd.Parameters["@GivenName"].Value = user.GivenName;
            cmd.Parameters["@FamilyName"].Value = user.FamilyName;

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

            result = (1 == rowsAffected);

            return result;
        }

        public int InsertOrDeleteEmployeeRole(string userID, string role, bool delete = false)
        {
            int rows = 0;

            string cmdText = delete ? "sp_delete_employee_role" : "sp_insert_employee_role";

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@RoleID", role);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

    }
}

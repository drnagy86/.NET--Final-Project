using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessInterFaces;
using DataAccessLayer;
using System.Security.Cryptography;


namespace LogicLayer
{
    public class UserManager : IUserManager
    {


        private IUserAccessor _userAccessor = null;

        public UserManager()
        {
            _userAccessor = new UserAccessor();

        }

        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public bool AuthenticateUserReturnsTrueOnSuccess(string userID, string passwordHash)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.AuthenticateUserWithUserIDAndPasswordHash(userID,passwordHash));
            }
            catch (Exception)
            {

                throw;
            }
            return result;


        }

        public List<string> GetRolesForUser(string userID)
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectRolesByUserID(userID);
            }
            catch (Exception)
            {

                throw;
            }


            return roles;
        }

        public User GetUserByUserID(string userID)
        {
            User requestedUser = null;

            try
            {
                requestedUser = _userAccessor.SelectUserByUserID(userID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return requestedUser;

        }

        public string HashSha256(string source)
        {
            string result = "";

            byte[] data;

            using (SHA256 sha256Hasher = SHA256.Create())
            {
                data = sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            var s = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString();


            return result.ToUpper();
        }

        public User LoginUser(string userID, string password)
        {
            User loggedInUser = null;
            try
            {
                if (userID == "")
                {
                    throw new ArgumentException("Missing email.");
                }
                if (password == "") // this is where complexity rules can be added
                {
                    throw new ArgumentException("Missing password.");
                }

                password = HashSha256(password);
                if (AuthenticateUserReturnsTrueOnSuccess(userID, password))
                {
                    loggedInUser = GetUserByUserID(userID);
                    loggedInUser.Roles = GetRolesForUser(userID);
                }

                else
                {
                    throw new ArgumentException("Bad email or password. Please try again.");
                }


            }
            catch (Exception ex)
            {

                throw new ApplicationException("Login failed, please check credentials.", ex);
            }

            return loggedInUser;
        }

        public bool ResetPassword(string userID, string oldPassword, string newPassword)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.UpdatePasswordHash(
                    userID.ToLower(),
                    HashSha256(oldPassword),
                    HashSha256(newPassword)
                    )
                    );

                if (!result)
                {
                    throw new ApplicationException("Update Failed");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public List<string> RetrieveEmployeeRoles(int employeeID)
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectRolesByEmployeeID(employeeID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }

        public List<string> RetrieveEmployeeRoles()
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }

        public bool FindUser(string email)
        {
            try
            {
                return _userAccessor.GetUserByEmail(email) != null;
            }
            catch (ApplicationException ax)
            {
                if (ax.Message == "User not found.")
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Database Error", ex);
            }
        }

        public User AuthenticateUser(string email, string password)
        {
            User result = null;

            // we need to hash the password
            var passwordHash = hashPassword(password);
            password = null;

            try
            {
                result = _userAccessor.AuthenticateUser(email, passwordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login failed!", ex);
            }

            return result;
        }

        private string hashPassword(string source)
        {
            // use SHA256
            string result = null;

            // we need a byte array because cryptography is bits and bytes
            byte[] data;

            // create a has provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            // build a string from the result
            var s = new StringBuilder();

            // loop through the bytes to build a string
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString().ToUpper();

            return result;
        }

        public bool AddUser(User user)
        {
            bool result = false;

            if (user.UserID == "" || user.UserID == null)
            {
                throw new ApplicationException("Must include an email for the UserID");
            }


            if (user.GivenName == "" || user.GivenName == null)
            {
                throw new ApplicationException("Must include a given name.");
            }


            if (user.FamilyName == "" || user.FamilyName == null)
            {
                throw new ApplicationException("Must include a family name");
            }

            try
            {
                result = _userAccessor.InsertUser(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;           

        }

        public bool AddUserRole(string userID, string role)
        {
            bool result = false;
            try
            {
                result = (1 == _userAccessor.InsertOrDeleteEmployeeRole(userID, role));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Role not added!", ex);
            }
            return result;
        }
        public bool DeleteUserRole(string userID, string role)
        {
            bool result = false;
            try
            {
                result = (1 == _userAccessor.InsertOrDeleteEmployeeRole(userID, role, delete: true));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Role not removed!", ex);
            }
            return result;
        }


    }
}

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


        public bool AuthenticateUser(string userID, string passwordHash)
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
            catch (Exception)
            {

                throw;
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
                if (AuthenticateUser(userID, password))
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
    }
}

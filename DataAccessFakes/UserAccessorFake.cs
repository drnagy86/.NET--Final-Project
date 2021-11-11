using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterFaces;
using DataObjects;


namespace DataAccessFakes
{
    public class UserAccessorFake : IUserAccessor
    {
        private List<User> fakeUsers = new List<User>();
        private List<string> passwordHashes = new List<string>();

        public UserAccessorFake()
        {
            fakeUsers.Add(new User()
            {
                UserID = "tess@company.com",
                GivenName = "Tess",
                FamilyName = "Data",
                Active = true,
                Roles = new List<string>()

            });

            fakeUsers.Add(new User()
            {
                UserID = "duplicate@company.com",
                GivenName = "Tess",
                FamilyName = "Data",
                Active = true,
                Roles = new List<string>()

            });

            fakeUsers.Add(new User()
            {
                UserID = "duplicate@company.com",
                GivenName = "Tess",
                FamilyName = "Data",
                Active = true,
                Roles = new List<string>()
            });


            passwordHashes.Add("9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E");
            //passwordHashes.Add("5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8");
            passwordHashes.Add("dup-9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E");
            passwordHashes.Add("dup-9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E");

            fakeUsers[0].Roles.Add("Admin");
            fakeUsers[0].Roles.Add("Creator");
        }

        public int AuthenticateUserWithUserIDAndPasswordHash(string userID, string passwordHash)
        {
            int numAuthenticated = 0;
            // check for user records in fake data
            for (int i = 0; i < fakeUsers.Count; i++)
            {
                int userIndex = -1; // invalid list index
                if (fakeUsers[i].UserID == userID)
                {                   // found the email
                    userIndex = i;
                    // is the password correct
                    if (passwordHashes[userIndex] == passwordHash &&
                        fakeUsers[userIndex].Active)
                    {               // then the user is authenticated
                        numAuthenticated += 1;
                    }
                }
            }


            return numAuthenticated;
        }

        public List<string> SelectRolesByUserID(string userID)
        {
            List<string> roles = new List<string>();
            bool foundUser = false;

            for (int i = 0; i < fakeUsers.Count; i++)
            {
                if(fakeUsers[i].UserID == userID)
                {
                    roles = fakeUsers[i].Roles;
                    foundUser = true;
                    break;
                }
            }
            if (!foundUser)
            {
                throw new ApplicationException("Employee roles unavailable. Employee not found");
            }

            return roles;


        }

        public User SelectUserByUserID(string userID)
        {
            User user = null;

            foreach (var fakeUser in fakeUsers)
            {
                if (fakeUser.UserID == userID)
                {
                    user = fakeUser;
                }
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
            }

            return user;
        }

        public int UpdatePasswordHash(string userID, string oldPassword, string newPassword)
        {
            int rowsAffected = 0;

            for (int i = 0; i < fakeUsers.Count; i++)
            {
                if (fakeUsers[i].UserID == userID)
                {
                    if (passwordHashes[i] == oldPassword)
                    {
                        passwordHashes[i] = newPassword;
                        rowsAffected = 1;
                        break;
                    }
                }
            }

            return rowsAffected;
        }
    }
}

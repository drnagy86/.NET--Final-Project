using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IUserManager
    {
        User LoginUser(string userID, string password);

        bool AuthenticateUser(string userID, string passwordHash);
        string HashSha256(string source);
        User GetUserByUserID(string userID);
        List<string> GetRolesForUser(string userID);



    }
}

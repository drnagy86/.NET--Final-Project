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

        bool AuthenticateUserReturnsTrueOnSuccess(string userID, string passwordHash);
        string HashSha256(string source);
        User GetUserByUserID(string userID);
        List<string> GetRolesForUser(string userID);

        bool ResetPassword(string userID, string oldPassword, string newPassword);

        List<string> RetrieveEmployeeRoles(int employeeID);
        List<string> RetrieveEmployeeRoles();

        bool FindUser(string email);
        User AuthenticateUser(string email, string password);

        bool AddUser(User user);
        bool DeleteUserRole(string userID, string role);
        bool AddUserRole(string userID, string role);

        //bool UpdatePassword(string userID, string oldPassword, string newPassword);

    }
}

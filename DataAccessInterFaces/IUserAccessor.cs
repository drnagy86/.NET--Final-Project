using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace DataAccessInterFaces
{
    public interface IUserAccessor
    {
        int AuthenticateUserWithUserIDAndPasswordHash(string userID, string passwordHash);
        User SelectUserByUserID(string userID);
        List<string> SelectRolesByUserID(string userID);
        int UpdatePasswordHash(string userID, string oldPassword, string newPassword);

        List<string> SelectAllRoles();
        List<string> SelectRolesByEmployeeID(int employeeID);

        User GetUserByEmail(string email);
        User AuthenticateUser(string username, string passwordHash);

        bool InsertUser(User user);

        int InsertOrDeleteEmployeeRole(string userID, string role, bool delete = false);

        //int UpdatePassword(string userID, string oldPassword, string newPassword);
    }
}

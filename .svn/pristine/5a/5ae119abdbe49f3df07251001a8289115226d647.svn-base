using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IUserManagementRepository
    {
        List<int> GetMenuKeysbyUserGroupAndUserType(int uG, int uT);

        Task<DO_ReturnParameter> InsertMenukeysIntoUserGroup(DO_UserGroup selectedkeys);
    }
}

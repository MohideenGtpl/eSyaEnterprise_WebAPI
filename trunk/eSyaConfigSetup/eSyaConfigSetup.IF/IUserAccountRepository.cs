using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
    public interface IUserAccountRepository
    {
        Task<List<DO_MainMenu>> GeteSyaMenulist();
    }
}

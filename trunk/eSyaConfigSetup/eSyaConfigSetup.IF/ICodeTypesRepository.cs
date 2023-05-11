using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface ICodeTypesRepository
    {
       Task<List<DO_CodeTypes>> GetCodeTypes();

       Task <DO_ReturnParameter> InsertIntoCodeType(DO_CodeTypes obj);

       Task <DO_ReturnParameter> UpdateCodeType(DO_CodeTypes obj);

       Task <List<DO_CodeTypes>> GetActiveCodeTypes();

       Task <List<DO_CodeTypes>> GetUserDefinedCodeTypesList();

       Task<List<DO_CodeTypes>> GetSystemDefinedCodeTypesList();

       Task<DO_ReturnParameter> ActiveOrDeActiveCodeTypes(bool status, int code_type);
    }
}

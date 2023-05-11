using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eSyaConfigSetup.DO;

namespace eSyaConfigSetup.IF
{
    public interface IDepartmentCodesRepository
    {
        #region Department Code
        Task<DO_ReturnParameter> InsertIntoDepartmentCodes(DO_DepartmentCodes obj);
        Task<DO_ReturnParameter> UpdateDepartmentCodes(DO_DepartmentCodes obj);
        Task<List<DO_DepartmentCodes>> GetDepartmentCodes();
        Task<DO_ReturnParameter> ActiveOrDeActiveDepartmentCodes(bool status, int deptId);
        #endregion Department Code
        #region Department Location Link
        Task<DO_ReturnParameter> InsertIntoDepartmentLocationLink(DO_DepartmentLocation obj);
        Task<DO_ReturnParameter> UpdateDepartmentLocationLink(DO_DepartmentLocation obj);
        Task<List<DO_DepartmentLocation>> GetDepartmentLocation(int businessKey, int departmentId);
        Task<DO_ReturnParameter> ActiveOrDeActiveDepartmentLocationLink(bool status, int deptId, int deptlocId, int Businesskey);
        #endregion Department Location Link
    }
}

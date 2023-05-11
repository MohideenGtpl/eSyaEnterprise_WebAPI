using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IFormApprovalRepository
    {
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType);

        #region Form Task Assign

        Task<List<DO_FormTaskAssign>> GetFormTaskAssignments();

        Task<List<DO_FormNames>> GetAllActiveForms();

        Task<DO_ReturnParameter> InsertFormTaskAssignment(DO_FormTaskAssign obj);

        Task<DO_ReturnParameter> UpdateFormTaskAssignment(DO_FormTaskAssign obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveFormTaskAssignment(bool status, int formId, int taskId);

        #endregion

        #region Form Task Approval

        List<DO_FormTaskAssign> GetFormTaskbyFormId(int formId);

        Task<List<DO_FormTaskApproval>> GetFormTaskApprovalsbyBusinesskey(int businesskey);

        Task<DO_ReturnParameter> InsertFormTaskApproval(DO_FormTaskApproval obj);

        Task<DO_ReturnParameter> UpdateFormTaskApproval(DO_FormTaskApproval obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveFormTaskApproval(DO_FormTaskApproval objform);

        #endregion
    }
}

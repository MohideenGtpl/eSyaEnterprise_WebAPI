using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IProcessMasterRepository
    {
        #region Process Master

        Task<List<DO_ProcessMaster>> GetProcessMaster();

        Task<DO_ReturnParameter> InsertIntoProcessMaster(DO_ProcessMaster obj);

        Task<DO_ReturnParameter> UpdateProcessMaster(DO_ProcessMaster obj);

        #endregion

        #region Process Rule

        Task<List<DO_ProcessRule>> GetProcessRule();

        Task<List<DO_ProcessMaster>> GetActiveProcessMaster();

        Task<List<DO_ProcessRule>> GetProcessRuleByProcessId(int processId);

        Task<DO_ReturnParameter> InsertIntoProcessRule(DO_ProcessRule obj);

        Task<DO_ReturnParameter> UpdateProcessRule(DO_ProcessRule obj);

        #endregion
        #region Process Rule by segment wise

        Task<List<DO_ProcessRule>> GetProcessRulebySegmentwise();

        Task<List<DO_ProcessRule>> GetProcessRulebyBusinessKey(int BusinessKey);

        Task<DO_ReturnParameter> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj); 
        #endregion
    }
}

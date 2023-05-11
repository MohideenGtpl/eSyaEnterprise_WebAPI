using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaConfigSetup.DO
{
    public class DO_FormTaskApproval
    {
        public int BusinessKey { get; set; }
        public int FormId { get; set; }
        public int TaskId { get; set; }
        public int ApprovalLevelStage { get; set; }
        public int ApproverPriority { get; set; }
        public int UserRole { get; set; }
        public decimal ApprovalRangeFrom { get; set; }
        public decimal ApprovalRangeTo { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string TaskName { get; set; }
        public string FormName { get; set; }
        public string UserRoleName { get; set; }
        public bool status { get; set; }
        
    }
}

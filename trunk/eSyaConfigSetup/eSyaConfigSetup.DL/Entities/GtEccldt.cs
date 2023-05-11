using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEccldt
    {
        public int BusinessKey { get; set; }
        public decimal FinancialYear { get; set; }
        public int MonthId { get; set; }
        public bool MonthFreezeHis { get; set; }
        public bool MonthFreezeFin { get; set; }
        public bool MonthFreezeHr { get; set; }
        public int PatientIdgen { get; set; }
        public string PatientIdserial { get; set; }
        public string BudgetMonth { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcclco GtEcclco { get; set; }
    }
}

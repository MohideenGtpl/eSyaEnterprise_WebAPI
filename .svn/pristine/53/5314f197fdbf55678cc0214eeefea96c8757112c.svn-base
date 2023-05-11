using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcclco
    {
        public GtEcclco()
        {
            GtEccldt = new HashSet<GtEccldt>();
        }

        public int BusinessKey { get; set; }
        public decimal FinancialYear { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public bool Status { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcbsln BusinessKeyNavigation { get; set; }
        public virtual ICollection<GtEccldt> GtEccldt { get; set; }
    }
}

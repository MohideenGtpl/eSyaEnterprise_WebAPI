using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcdccn
    {
        public int FormId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentCode { get; set; }
        public string DocCodeDesc { get; set; }
        public string DocumentCategory { get; set; }
        public string DocCatgDesc { get; set; }
        public bool IsFinancialYearAppl { get; set; }
        public bool IsStoreLinkAppl { get; set; }
        public bool IsTransactionModeAppl { get; set; }
        public bool IsCustomerGroupAppl { get; set; }
        public bool LinkToDashboard { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcfmfd Form { get; set; }
    }
}

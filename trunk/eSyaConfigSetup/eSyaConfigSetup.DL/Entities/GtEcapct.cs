using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcapct
    {
        public GtEcapct()
        {
            GtEcapcd = new HashSet<GtEcapcd>();
        }

        public int CodeType { get; set; }
        public string CodeTyepDesc { get; set; }
        public string CodeTypeControl { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcapcd> GtEcapcd { get; set; }
    }
}

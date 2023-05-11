using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcltfc
    {
        public GtEcltfc()
        {
            GtEcltcd = new HashSet<GtEcltcd>();
        }

        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcltcd> GtEcltcd { get; set; }
    }
}

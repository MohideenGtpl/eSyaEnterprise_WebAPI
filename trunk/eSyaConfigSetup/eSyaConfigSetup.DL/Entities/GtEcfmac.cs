using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcfmac
    {
        public GtEcfmac()
        {
            GtEcfmal = new HashSet<GtEcfmal>();
        }

        public int ActionId { get; set; }
        public string ActionDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcfmal> GtEcfmal { get; set; }
    }
}

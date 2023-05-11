using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcfmal
    {
        public int FormId { get; set; }
        public int ActionId { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcfmac Action { get; set; }
        public virtual GtEcfmfd Form { get; set; }
    }
}

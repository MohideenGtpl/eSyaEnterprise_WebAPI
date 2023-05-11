﻿using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcprrl
    {
        public GtEcprrl()
        {
            GtEcaprl = new HashSet<GtEcaprl>();
        }

        public int ProcessId { get; set; }
        public string ProcessDesc { get; set; }
        public bool IsSegmentSpecific { get; set; }
        public bool SystemControl { get; set; }
        public string ProcessControl { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcaprl> GtEcaprl { get; set; }
    }
}

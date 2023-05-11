﻿using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcgrrh
    {
        public int BusinessKey { get; set; }
        public int ReportHeader { get; set; }
        public string ReportHeaderDesc { get; set; }
        public string ReportHeaderTemplate { get; set; }
        public int HeaderHeight { get; set; }
        public bool IsHeaderInvisible { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcbsln BusinessKeyNavigation { get; set; }
    }
}

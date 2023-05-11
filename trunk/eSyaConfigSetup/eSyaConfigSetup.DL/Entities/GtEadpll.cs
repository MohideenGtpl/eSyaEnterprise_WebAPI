using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEadpll
    {
        public int BusinessKey { get; set; }
        public int DepartmentId { get; set; }
        public int DeptLocnId { get; set; }
        public string LocationDescription { get; set; }
        public string LocnShortDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEadpcd Department { get; set; }
    }
}

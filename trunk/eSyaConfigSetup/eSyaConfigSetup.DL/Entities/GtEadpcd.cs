using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEadpcd
    {
        public GtEadpcd()
        {
            GtEadpll = new HashSet<GtEadpll>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentDesc { get; set; }
        public string DeptShortDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEadpll> GtEadpll { get; set; }
    }
}

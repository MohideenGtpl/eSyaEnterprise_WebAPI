using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEctm01
    {
        public GtEctm01()
        {
            GtEctm02 = new HashSet<GtEctm02>();
        }

        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int DispSeqId { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEctm02> GtEctm02 { get; set; }
    }
}

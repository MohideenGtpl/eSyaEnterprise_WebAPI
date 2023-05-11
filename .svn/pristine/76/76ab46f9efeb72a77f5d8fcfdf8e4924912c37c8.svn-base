using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEclttm
    {
        public GtEclttm()
        {
            GtEclttl = new HashSet<GtEclttl>();
        }

        public int TableCode { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEclttl> GtEclttl { get; set; }
    }
}

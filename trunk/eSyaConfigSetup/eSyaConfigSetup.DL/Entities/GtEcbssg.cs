using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcbssg
    {
        public GtEcbssg()
        {
            GtEcbsln = new HashSet<GtEcbsln>();
        }

        public int BusinessId { get; set; }
        public int SegmentId { get; set; }
        public string SegmentDesc { get; set; }
        public bool IsMultiLocationApplicable { get; set; }
        public int Isdcode { get; set; }
        public string CurrencyCode { get; set; }
        public string OrgnDateFormat { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEcbsen Business { get; set; }
        public virtual ICollection<GtEcbsln> GtEcbsln { get; set; }
    }
}

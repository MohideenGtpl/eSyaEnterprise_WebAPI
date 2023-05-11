using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEcmamn
    {
        public GtEcmamn()
        {
            GtEcmnfl = new HashSet<GtEcmnfl>();
            GtEcsbmn = new HashSet<GtEcsbmn>();
        }

        public int MainMenuId { get; set; }
        public string MainMenu { get; set; }
        public string ImageUrl { get; set; }
        public int MenuIndex { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcmnfl> GtEcmnfl { get; set; }
        public virtual ICollection<GtEcsbmn> GtEcsbmn { get; set; }
    }
}

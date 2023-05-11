using System;
using System.Collections.Generic;

namespace eSyaConfigSetup.DL.Entities
{
    public partial class GtEclttl
    {
        public string LanguageCode { get; set; }
        public int TableCode { get; set; }
        public int TablePrimaryKeyId { get; set; }
        public string FieldDescLanguage { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEclttm TableCodeNavigation { get; set; }
    }
}

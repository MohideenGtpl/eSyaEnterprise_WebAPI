using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaConfigSetup.DO
{
    public class DO_TaxRule
    {
        public int ISDCode { get; set; }
        public int TaxCode { get; set; }
        public int SerialNumber { get; set; }
        public string TaxShortCode { get; set; }
        public string TaxDescription { get; set; }
        public string SlabOrPerc { get; set; }
        public bool IsSplitApplicable { get; set; }
        public decimal SplitCategoryPerc { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormId { get; set; }
        public string TerminalID { get; set; }
    }
}

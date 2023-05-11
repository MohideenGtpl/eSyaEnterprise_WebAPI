using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaConfigSetup.DO
{
   public class DO_CalendarDefinition
    {
        public decimal FinancialYear { get; set; }
        public int BusinessKey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public bool Status { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }

    }
}

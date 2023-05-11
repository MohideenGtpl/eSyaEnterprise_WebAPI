using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaConfigSetup.DO
{
   public class DO_CalendarDetails
    {
        public int BusinessKey { get; set; }
        public decimal FinancialYear { get; set; }
        public int MonthId { get; set; }
        public bool MonthFreezeHis { get; set; }
        public bool MonthFreezeFin { get; set; }
        public bool MonthFreezeHr { get; set; }
        public int PatientIdgen { get; set; }
        public string PatientIdserial { get; set; }
        public string BudgetMonth { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public int EditMonthId { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Tilldate { get; set; }
        public string MonthDescription { get; set; }

    }
}
